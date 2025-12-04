using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float damping = 5f;
    public float rotateSpeed = 90f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector2 _direction;
    private bool _isMoving;
    private bool _isSprint;
    private float _multifly = 1;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        _direction = value.Get<Vector2>();
        if (_direction.y > 0.1f)
            _direction.y = 1f;
        if (_direction.y < -0.1f)
            _direction.y = -1f;

        if(!_isMoving)
        {
            if (_direction.y > 0.1f || _direction.y < -0.1f)
            {
                _isMoving = true;
            }
        }
        else
        {
            if (_direction.y < 0.1f && _direction.y > -0.1f)
            {
                _isMoving = false;                
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        _isSprint = value.isPressed;        
    }

    
    void Update()
    {
        _multifly = Mathf.Lerp(_multifly, _isSprint ? 2f : 1f, damping * Time.deltaTime);
        _animator.SetBool("IsMoving", _isMoving);
        _animator.SetFloat("Speed", _multifly * _direction.y);

        if (!_isMoving)
        {
            _animator.SetFloat("Rotate", _direction.x);
            return;
        }

        transform.Rotate(0f, _direction.x * rotateSpeed * Time.deltaTime, 0f);
    }

    private void OnAnimatorMove()
    {
        if(!_isMoving)
        {
            //여기 부분 코드를 이렇게 수정할 것. 
            transform.rotation *= _animator.deltaRotation;
        }
        
        Vector3 velocity = _animator.deltaPosition / Time.deltaTime;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }
}
