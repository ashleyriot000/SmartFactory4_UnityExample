using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public string targetName = "CameraTarget";
    public Transform cameraTarget;
    public Animator animator;
    public float rotateSpeed = 720f;
    public float moveStrengh = 10f;


    private float _currentSpeed = 0f;
    private Vector2 _direction;

    void Start()
    {
        animator  = GetComponent<Animator>();
        cameraTarget = transform.Find(targetName);
    }

    private void Update()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _direction.magnitude, moveStrengh * Time.deltaTime);
        bool isMoving = _currentSpeed > 0.01f;
        animator.SetBool("IsMoving", isMoving);
    }

    private void OnAnimatorMove()
    {
        Vector3 forward = cameraTarget.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTarget.right;
        right.y = 0;
        right.Normalize();

        if(_direction != Vector2.zero)
        {
            Vector3 moveDirection = forward * _direction.y + right * _direction.x;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(moveDirection),
                rotateSpeed * Time.deltaTime);
        }

        transform.position += animator.deltaPosition;
    }

    public void OnMove(InputValue value)
    {
        _direction = value.Get<Vector2>();
    }
}
