using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 15.0f;

    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _rb.gravityScale = 0.0f;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _moveVelocity = Vector2.zero;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveVelocity = moveInput.normalized * Speed;

        if(Time.timeScale == 0.0f)
        {
            _animator.Play("Idle");
        }
        else if(_moveVelocity.x > 2.0 )
        {
            _animator.Play("WalkRight");
        }
        else if(_moveVelocity.x < -3.5f)
        {
            _animator.Play("WalkLeft");
        }
        else if (_moveVelocity.y > 3.5f)
        {
            _animator.Play("WalkUp");
        }
        else if(_moveVelocity.y < 0.0f)
        {
            _animator.Play("WalkDown");
        }
        else
        {
            _animator.Play("Idle");
        }

    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);

    }
}