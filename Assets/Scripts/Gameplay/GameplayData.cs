using UnityEngine;


[CreateAssetMenu(fileName = "GameplayData", menuName = "Assets/GameplayData", order = 0)]
public class GameplayData : ScriptableObject
{
    #region Fields

    [SerializeField] private uint _costWheatBlock;

    #endregion

    #region Properties

    public uint CostWheatBlock => _costWheatBlock;

    #endregion
    
}