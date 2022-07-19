using UnityEngine;

public class Tool : MonoBehaviour
{
    #region Fields

    [SerializeField] private ToolData _toolData;

    #endregion

    #region Properties

    public float Distance => _toolData.ActiveDistance;

    #endregion
}