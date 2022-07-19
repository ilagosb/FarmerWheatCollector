using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    #region Fields

    [SerializeField] private GUIView _guiView;
    [SerializeField] private Farmer _farmer;

    #endregion


    private void Awake()
    {
        _guiView.WheatCountUpdate(_farmer.WheatValues, _farmer.MaxWheatInCollection);
        _guiView.CoinValueUpdate(_farmer.Coin);
    }

    private void OnEnable()
    {
        _farmer.EventWheatChange += _guiView.WheatCountUpdate;
        _farmer.EventCoinChange += _guiView.CoinValueUpdate;
    }

    private void OnDisable()
    {
        _farmer.EventWheatChange -= _guiView.WheatCountUpdate;
        _farmer.EventCoinChange -= _guiView.CoinValueUpdate;
    }
}