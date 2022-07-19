using System;
using DG.Tweening;
using UnityEngine;
using Utils;

public class WheatBlock : MonoBehaviour
{
    #region Fields

    private const float JumpPower = 4f;

    [SerializeField] private float _timeCollectAnimation;
    [SerializeField] private AnimationCurve _curveCollectAnimation;
    private ObjectPoolContainer _objectPoolContainer;

    #endregion

    private void Awake()
    {
        _objectPoolContainer = GetComponentInParent<ObjectPoolContainer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollector collector))
        {
            if (collector.TryCollect())
            {
                transform.DOJump(collector.TransformCollector.position, JumpPower, 1, _timeCollectAnimation)
                    .SetEase(_curveCollectAnimation)
                    .OnComplete(() =>
                        {
                            if (_objectPoolContainer != null)
                            {
                                _objectPoolContainer.ObjectPool.Release(gameObject);
                            }
                            else
                            {
                                Destroy(gameObject);
                            }
                        }
                    ); 
            }
        }
    }
}