using UnityEngine;

public class CollectorView : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _topYScale;
    [SerializeField] private Transform _viewCollection;

    #endregion

    #region Properties

    public Transform ViewCollection => _viewCollection;

    #endregion

    private void Awake()
    {
        UpdateView(0);
    }

    private void UpdateView(float newValue)
    {
        _viewCollection.gameObject.SetActive(newValue > 0);
        var scale = _viewCollection.localScale;
        _viewCollection.localScale = new Vector3(scale.x, newValue, scale.z);
    }

    public void OnChangeCollectionValue(uint value, uint maxCollection)
    {
        var valueScaleY = (float) value / maxCollection * _topYScale;
        UpdateView(valueScaleY);
    }
}