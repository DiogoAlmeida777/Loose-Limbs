using System;
using UnityEngine;

public class CeilingSensor : MonoBehaviour
{

    public event Action ceilingCollision;

    private void OnTriggerEnter(Collider other)
    {
        
        ceilingCollision?.Invoke();
    }
}
