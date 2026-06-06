using UnityEngine;

public class DeltaTransform : MonoBehaviour
{

    private Vector3 oldPosition, oldRotation, oldScale;
    private Vector3 _dPosition, _dRotation, _dScale;
    private Vector3 _movSpeed, _rotSpeed;
    private float _fwdSpeed, _sideSpeed;

    void Awake()
    {
        oldPosition = transform.position;
        oldRotation = transform.eulerAngles;
        oldScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _dPosition = transform.position - oldPosition;
        _dRotation = transform.eulerAngles - oldRotation;

        _movSpeed = _dPosition / Time.deltaTime;
        _rotSpeed = _dRotation / Time.deltaTime;

        _fwdSpeed = Vector3.Dot(transform.forward, _movSpeed);
        _sideSpeed = Vector3.Dot(transform.right, _movSpeed);

        oldPosition = transform.position;
        oldRotation = transform.eulerAngles;
        oldScale = transform.localScale;
    }

    public Vector3 movSpeed() { return _movSpeed; }
    public Vector3 rotSpeed() { return _rotSpeed; }
    public float fwdSpeed() { return _fwdSpeed; }
    public float sideSpeed() { return _sideSpeed; }

}
