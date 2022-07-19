using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GUIView : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI _wheatMaxValue;
    [SerializeField] private TextMeshProUGUI _wheatCurrentValue;
    [SerializeField] private TextMeshProUGUI _coinCurrentValue;

    #endregion

    private void CounterAnimation(uint updateValue)
    {
        uint stepValue = Convert.ToUInt32(_coinCurrentValue.text);
        DOTween.Sequence()
            .Append(DOTween
                .To(() => stepValue, value => stepValue = value, updateValue, .6f)
                .SetEase(Ease.InOutCubic)
                .OnUpdate(() =>
                {
                    _coinCurrentValue.text = stepValue.ToString();
                }))
            .Join(_coinCurrentValue.transform.DOShakePosition(.7f, 10f, 30));
    }

    public void WheatCountUpdate(uint updateValue, uint maxValue)
    {
        _wheatCurrentValue.text = updateValue.ToString();
        _wheatMaxValue.text = maxValue.ToString();
    }

    public void CoinValueUpdate(uint updateValue)
    {
        CounterAnimation(updateValue);
    }
    
    
}