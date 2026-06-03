using UnityEngine;
using UnityEngine.InputSystem;

public class TurnCharacter : MonoBehaviour
{

    [SerializeField] private float turnSpeed = 180f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        transform.Rotate(Vector3.up, input.x * turnSpeed * Time.deltaTime);
    }

}
