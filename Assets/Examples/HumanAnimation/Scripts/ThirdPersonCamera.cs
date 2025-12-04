using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float mouseSensitivity = 5f;

    private Vector2 _delta;
    private float _pitch = 0f;
    private float _yaw = 0f;

    void Start()
    {
        Vector3 rotation = target.rotation.eulerAngles;
        _pitch = rotation.y;
        _yaw = rotation.x;
    }

    void LateUpdate()
    {
        _pitch -= _delta.y * mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        _yaw += _delta.x * mouseSensitivity * Time.deltaTime;
        target.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    public void OnLook(InputValue value)
    {
        _delta = value.Get<Vector2>();
    }
}
