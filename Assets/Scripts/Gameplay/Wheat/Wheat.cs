using System;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;
using Utils;

public class Wheat : MonoBehaviour
{
    #region Events

    public event Action<Wheat, GameObject> EventSlice;

    #endregion
    
    #region Fields

    [SerializeField] private ObjectPool _blockPool;
    [SerializeField] private Material _crossSectionMaterial;
    [SerializeField] private ParticleSystem _sliceEffect;
    private AudioSource _sliceSound;
    private bool _isGrowing;
    
    #endregion

    private void Awake()
    {
        _sliceSound = GetComponent<AudioSource>();
    }

    private void EffectSlice()
    {
        _sliceEffect.Play();
        _sliceSound.Play();
    }

    public void EndGrow() => _isGrowing = false;

    public void Slice()
    {
        if (_isGrowing) return;
        _isGrowing = true;
        EffectSlice();
        var hull = gameObject.Slice(transform.position - (transform.position * .7f), transform.up, _crossSectionMaterial);
        var lowerHull = hull?.CreateLowerHull(gameObject, _crossSectionMaterial);
        if (lowerHull != null) 
            lowerHull.transform.position = transform.position;
        WheatBlock block = _blockPool.Get<WheatBlock>(transform.position);
        EventSlice?.Invoke(this, lowerHull);
    }
    
}