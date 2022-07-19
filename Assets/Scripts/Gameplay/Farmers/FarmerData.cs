using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmerData", menuName = "Assets/Farmer/FarmerData", order = 0)]
public class FarmerData : ScriptableObject
{
    #region Fields

    [SerializeField] private float _movementSpeed;
    [SerializeField] private Color _skinColor;

    #endregion

    #region Properties

    public float MovementSpeed => _movementSpeed;
    public Color SkinColor => _skinColor;

    #endregion
}