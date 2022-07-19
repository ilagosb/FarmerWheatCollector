using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    #region Fields

    [SerializeField] private Joystick _joystick;
    [SerializeField] private Farmer _farmer;

    #endregion
    
    private void FixedUpdate()
    {
        var moveVector = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        moveVector.Normalize();
        _farmer.Move(moveVector);
    }
}