using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TriggerBarnZone : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private ObjectPool _blockPool;
    [SerializeField] private ObjectPool _coinPool;
    [SerializeField] private Transform _pointUnpack;
    [SerializeField] private Transform _pointCoin;
    
    [Header("Animation settings")]
    [SerializeField] private AnimationCurve _animationUnpackCurve;
    [SerializeField] private AnimationCurve _animationPaymentCurve;
    [SerializeField] private float _unpackDelay;
    [SerializeField] private float _unpackTime;
    [SerializeField] private float _paymentDelay;
    [SerializeField] private float _paymentTime;
    
    private AudioSource _paymentSound;
    
    private Coroutine _unpackProcess;

    #endregion

    private void Awake()
    {
        _paymentSound = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Farmer farmer))
        {
            if (_unpackProcess != null) return;
            _unpackProcess = StartCoroutine(UnpackProcess(farmer));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_unpackProcess != null)
        {
            StopCoroutine(_unpackProcess);
        }
        _unpackProcess = null;
    }

    private IEnumerator UnpackProcess(Farmer farmer)
    {
        while (farmer.Throw())
        {
            yield return new WaitForSeconds(_unpackDelay);
            var block = _blockPool.Get(farmer.TransformCollector.position);
            if (block.TryGetComponent(out Collider collider))
            {
                collider.enabled = false;
            }
            block.transform.DOJump(_pointUnpack.position, 2f, 1, _unpackTime)
                .SetEase(_animationUnpackCurve)
                .OnComplete(() =>
                {
                    _blockPool.Release(block);
                    collider.enabled = true;
                    var coin = _coinPool.Get(Camera.main.WorldToScreenPoint(_pointUnpack.position));
                    DOTween.Sequence()
                        .Append(coin.transform.DOMove(_pointCoin.position, _paymentTime))
                        .Join(coin.transform.DOScale(coin.transform.localScale, _paymentTime).From(0))
                        .SetEase(_animationPaymentCurve)
                        .SetDelay(_paymentDelay)
                        .OnComplete(() =>
                        {
                            _coinPool.Release(coin);
                            _paymentSound.Play();
                            farmer.AddMoney(_gameplayData.CostWheatBlock);
                        });
                });
        }
    }
}