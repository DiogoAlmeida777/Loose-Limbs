using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCharacter : MonoBehaviour
{

    [SerializeField] private float speed = 5.0f;
    private Vector3 deltaPosition = Vector3.zero;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        deltaPosition = new Vector3(input.x, -1, input.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.TransformDirection(deltaPosition);
        characterController.Move(move * speed * Time.deltaTime);
    }
}
