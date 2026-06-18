using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
