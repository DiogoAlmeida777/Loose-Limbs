using System;
using UnityEngine;

public class CeillingSensor : MonoBehaviour
{

    public event Action ceillingCollision;

    private void OnTriggerEnter(Collider other)
    {
        ceillingCollision?.Invoke();
    }
}
