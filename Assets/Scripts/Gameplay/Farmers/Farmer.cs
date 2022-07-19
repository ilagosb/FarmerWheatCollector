using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using EzySlice;
using UnityEngine;
using Plane = EzySlice.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]
public class Farmer : MonoBehaviour, ICollector
{
    #region Events

    public event Action<uint> EventCoinChange;
    public event Action<uint, uint> EventWheatChange;

    #endregion

    #region Fields

    private static readonly int Movement = Animator.StringToHash("Movement");
    private static readonly int Overweight = Animator.StringToHash("OverWeight");
    private static readonly int GrapMode = Animator.StringToHash("InGrapMode");
    private static int CuttingMask;
    private static readonly float RotationSpeed = 720f;
    private static readonly float PowerPush = 0.5f;

    [SerializeField] private FarmerData _data;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private ToolHolder _toolHolder;
    [SerializeField] private CollectorView _collectorView;
    private AudioSource _runSound;
    private CharacterController _controller;
    private Animator _animator;
    private Sequence _animationCollectionMove;

    [Header("Farmer parameters")] [SerializeField]
    private uint _maxWheat;

    private uint _wheats = 0;
    private uint _coins = 0;
    private bool _inGrapMode;

    #endregion

    #region Properties;

    public ToolHolder ToolHolder => _toolHolder;
    public float MovementSpeed => _data.MovementSpeed;
    public bool InGrapMode => _inGrapMode;
    public Transform TransformCollector => _collectorView.ViewCollection;
    public uint WheatValues => _wheats;
    public uint MaxWheatInCollection => _maxWheat;
    public uint Coin => _coins;

    #endregion

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        CuttingMask = LayerMask.GetMask("NotCollidingWithPlayer");
        _runSound = GetComponent<AudioSource>();
        SetupSkinColor();
    }

    private void OnEnable()
    {
        _toolHolder.EventChangeCurrentTool += OnChangeTool;
        EventWheatChange += _collectorView.OnChangeCollectionValue;
    }


    private void OnDisable()
    {
        _toolHolder.EventChangeCurrentTool -= OnChangeTool;
        EventWheatChange -= _collectorView.OnChangeCollectionValue;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PushCollideObject(hit);
    }

    private void PushCollideObject(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForceAtPosition(pushDir * PowerPush, transform.position, ForceMode.Impulse);
    }

    private void OnChangeTool(int activeTool)
    {
        _toolHolder.ActiveTool.gameObject.SetActive(false);
    }

    private void SetupSkinColor()
    {
        _mesh.GetComponent<Renderer>().material.color = _data.SkinColor;
    }

    private void UpdateAnimatorParameter(Vector3 moveVector)
    {
        _animator.SetFloat(Movement, moveVector.magnitude);
        _animator.SetFloat(Overweight, _wheats / _maxWheat);
        _animator.SetBool(GrapMode, _inGrapMode);
    }

    private void MoveSoundEffect()
    {
        _runSound.Play();
    }

    private void MoveCollectionEffect()
    {
        _animationCollectionMove = DOTween.Sequence()
            .Append(TransformCollector
                .DOShakeScale(.6f, Vector3.up * .4f, 1)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
            )
            .Join(TransformCollector
                .DOLocalJump(TransformCollector.localPosition, .25f, 1, .6f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutCubic)
            )
            .Join(TransformCollector
                .DOShakeRotation(.4f, 15f, 2)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutCubic)
            );
    }

    private void MoveEffect(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            if (_wheats > 0 && _animationCollectionMove == null || !_animationCollectionMove.IsActive())
                MoveCollectionEffect();
            if (!_runSound.isPlaying)
                MoveSoundEffect();
        }
        else
        {
            _animationCollectionMove.Kill(true);
            _runSound.Stop();
        }
    }

    public void Move(Vector3 moveVector)
    {
        UpdateAnimatorParameter(moveVector);
        MoveEffect(moveVector);
        _controller.Move(moveVector * (MovementSpeed * Time.fixedDeltaTime));
        if (moveVector != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void EnableGrapMode()
    {
        _inGrapMode = true;
        _toolHolder.ActiveTool.gameObject.SetActive(true);
    }

    public void DisableGrapMode()
    {
        _inGrapMode = false;
        _toolHolder.ActiveTool.gameObject.SetActive(false);
    }

    public void OnGrap()
    {
        var tool = _toolHolder.ActiveTool;
        var colliders = Physics.OverlapSphere(tool.transform.position, tool.Distance, CuttingMask,
            QueryTriggerInteraction.Ignore);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Wheat wheat))
            {
                wheat.Slice();
            }
        }
    }

    public bool TryCollect()
    {
        if (_wheats < _maxWheat)
        {
            _wheats++;
            _animationCollectionMove.Kill(true);
            EventWheatChange?.Invoke(_wheats, _maxWheat);
            return true;
        }

        return false;
    }

    public bool Throw()
    {
        if (_wheats > 0)
        {
            _wheats--;
            _animationCollectionMove.Kill(true);
            EventWheatChange?.Invoke(_wheats, _maxWheat);
            return true;
        }

        return false;
    }

    public void AddMoney(uint value)
    {
        _coins += value;
        EventCoinChange?.Invoke(_coins);
    }
}