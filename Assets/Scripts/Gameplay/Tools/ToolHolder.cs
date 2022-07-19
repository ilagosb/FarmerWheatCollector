using System;
using UnityEngine;

public class ToolHolder : MonoBehaviour
{
    #region Events

    public event Action<int> EventChangeCurrentTool;

    #endregion

    #region Fields

    private Tool[] _tools;
    private int _currentTool;

    #endregion

    #region Properties

    public Tool[] Tools => _tools;
    public Tool ActiveTool => _tools[_currentTool];
    public int CurrentTool => _currentTool;

    #endregion

    private void Awake()
    {
        _currentTool = 0;
        _tools = GetComponentsInChildren<Tool>();
        foreach (var tool in _tools)
        {
            tool.gameObject.SetActive(false);
        }
    }


    public void ChangeCurrentTool(int currentTool)
    {
        _currentTool = currentTool;
        EventChangeCurrentTool?.Invoke(_currentTool);
    }
}