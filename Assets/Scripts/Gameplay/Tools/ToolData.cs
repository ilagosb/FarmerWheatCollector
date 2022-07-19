using UnityEngine;


[CreateAssetMenu(fileName = "ToolData", menuName = "Assets/Tool/ToolData", order = 0)]
public class ToolData : ScriptableObject
{
    #region Fields

    [SerializeField] private float _activeDistance;

    #endregion

    #region Properties

    public float ActiveDistance => _activeDistance;

    #endregion
}