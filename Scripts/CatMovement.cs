using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    private bool _facingRight = true;
    private bool _inAir = false;
    private bool _grabbed = false;

    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private LayerMask _road;
    [SerializeField] private LayerMask _trashBin;
    [SerializeField] private LayerMask _fence;
    [SerializeField] private LayerMask _border;
    [SerializeField] private LayerMask _clothes;
    [SerializeField] private LayerMask _window;

    [SerializeField] private Animator _animator;

    private const float _speed = 10.0f;
    private const float _jumpPower = 6.5f;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 11, true);
        Physics2D.IgnoreLayerCollision(9, 12, true);
        _animator.SetInteger("MoveID", 0);
    }

    void Jump()
    {
        _rb.gravityScale = 1f;
        _animator.SetInteger("MoveID", 2);
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
        _inAir = true;

        StartCoroutine(Ungrab());
    }

    void MoveLeft()
    {
        _animator.SetInteger("MoveID", 1);

        if (_facingRight)
        {
            transform.localScale *= new Vector2(-1, 1);
            _facingRight = false;
        }

        float coeff = _inAir ? 0.2f : 1.0f;

        _rb.velocity += new Vector2(-_speed * Time.deltaTime * coeff, 0.0f);
    }

    void MoveRight()
    {
        _animator.SetInteger("MoveID", 1);

        if (!_facingRight)
        {
            transform.localScale *= new Vector2(-1, 1);
            _facingRight = true;
        }

        float coeff = _inAir ? 0.2f : 1.0f;

        _rb.velocity += new Vector2(_speed * Time.deltaTime * coeff, 0.0f);
    }

    IEnumerator Ungrab()
    {
        yield return new WaitForSeconds(0.25f);
        _grabbed = false;
    }

    void Grab()
    {
        _rb.velocity = new Vector2(0.0f, 0.0f);
        _rb.gravityScale = 0f;
        _grabbed = true;
        _animator.SetInteger("MoveID", 3);
    }

    void ProcessKeys() {
        if ((_grabbed || Stays()) && Input.GetKeyDown(KeyCode.W))
        {
            // jump up
            Jump();
        }
        else if (!_grabbed && Input.GetKey(KeyCode.A))
        {
            // move left
            MoveLeft();
        }
        else if (!_grabbed && Input.GetKey(KeyCode.D))
        {
            // move right
            MoveRight();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.S) && (!_inAir || _grabbed))
        {
            _animator.SetInteger("MoveID", 2);
            _rb.gravityScale = 1f;
            _rb.velocity = new Vector2(_rb.velocity.x, -6.0f);
            _inAir = true;

            StartCoroutine(Ungrab());
        }
        else if (!_inAir)
        {
            _animator.SetInteger("MoveID", 0);
            _rb.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    void Update()
    {
        ProcessKeys();

        if (_rb.velocity.y == 0.0f)
        {
            if (_rb.velocity.x == 0.0f)
            {
                
                _animator.SetInteger("MoveID", 0);
            }
            
            _inAir = _grabbed;
        }

        if (Physics2D.OverlapCircle(transform.position, 0.4f, _border))
        {
            _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
            transform.position = new Vector3(transform.position.x + (_facingRight ? -0.2f : 0.2f), transform.position.y, 0.0f);
        }

        if (!_grabbed && Physics2D.OverlapCircle(transform.position, 0.1f, _clothes))
        {
            Grab();
        }
    }

    bool Stays()
    {
        return OnGround() || OnFence() || OnTrashBin(); 
    }

    bool OnGround()
    {
        return Physics2D.OverlapCircle(transform.position, 0.6f, _road);
    }

    bool OnTrashBin()
    {
        return Physics2D.OverlapCircle(transform.position, 0.6f, _trashBin);
    }

    bool OnFence()
    {
        return Physics2D.OverlapCircle(transform.position, 0.6f, _fence);
    }
}
