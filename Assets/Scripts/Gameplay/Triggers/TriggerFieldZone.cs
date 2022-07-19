using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFieldZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Farmer farmer))
        {
            farmer.EnableGrapMode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Farmer farmer))
        {
            farmer.DisableGrapMode();
        }
    }
}
