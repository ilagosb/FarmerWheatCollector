using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriggerToolZone : MonoBehaviour
{
    #region Fields

    [SerializeField] private ToolHolder _toolHolder;
    private AudioSource _takeSound;

    #endregion

    #region Properties

    private int NextTool => _toolHolder.CurrentTool + 1 >= _toolHolder.Tools.Length ? 0 : _toolHolder.CurrentTool + 1;
    private int PrevTool => _toolHolder.CurrentTool - 1 < 0 ? _toolHolder.Tools.Length - 1 : _toolHolder.CurrentTool - 1;

    #endregion

    private void Awake()
    {
        _takeSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SwapTool(0);
        ToolAnimation();
    }

    private void OnEnable()
    {
        _toolHolder.EventChangeCurrentTool += SwapTool;
    }

    private void OnDisable()
    {
        _toolHolder.EventChangeCurrentTool -= SwapTool;
    }

    private void ToolAnimation()
    {
        DOTween.Sequence()
            .Append(
                _toolHolder.transform.DORotate(Vector3.up * 360, 8, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear)
            )
            .Join(
                _toolHolder.transform.DOJump(_toolHolder.transform.position, 0.4f, 4, 8)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear)
            ).SetLoops(-1);
    }

    private void SwapTool(int activeTool)
    {
        _toolHolder.Tools[PrevTool].gameObject.SetActive(true);
        _toolHolder.ActiveTool.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Farmer farmer))
        {
            _toolHolder.ChangeCurrentTool(NextTool);
            farmer.ToolHolder.ChangeCurrentTool(_toolHolder.CurrentTool);
            _takeSound.Play();
        }
    }
}