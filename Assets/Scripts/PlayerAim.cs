using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    [SerializeField] private Transform cameraTarget;

    private float yaw;
    private float pitch;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    [SerializeField] private float turnSpeed = 180f;

    [SerializeField] private Transform aimPosition;
    [SerializeField] private LayerMask aimLayerMask;

    [SerializeField] private float interactDistance = 4000f;
    private IInteractable currentInteractable;



    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimLayerMask))
        {
            aimPosition.position = Vector3.Lerp(aimPosition.position, hit.point, 20f * Time.deltaTime);

            if(hit.distance <= interactDistance)
            {
                currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
            }
        }

        if (currentInteractable != null)
            Debug.Log(currentInteractable.ToString());

        if (inputHandler.Interacted && currentInteractable != null)
        {
            currentInteractable.Interact(gameObject);
        }

        turnPlayer();
    }

    private void LateUpdate()
    {
        rotateCamera();
    }

    private void rotateCamera()
    {
        Vector2 lookInput = inputHandler.LookInput ;

        yaw += lookInput.x;
        pitch -= lookInput.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void turnPlayer()
    {
        Vector3 aimDirection = (aimPosition.position - transform.position).normalized;
        aimDirection.y = 0f;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, turnSpeed * Time.deltaTime);

    }


}
