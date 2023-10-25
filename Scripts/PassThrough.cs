using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThrough : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private LayerMask _cat;

    private bool _catOnPlatform = false;

    private float _prevTime = 0.0f;
    private const float _delay = 15.0f;
    private float _offset;

    // Start is called before the first frame update
    void Start()
    {
        _renderer.enabled = false;
        _offset = Random.Range(0.0f, 30.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_catOnPlatform && Input.GetKeyDown(KeyCode.S))
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }

        if (gameObject.layer == 8)
        {
            return;
        }

        if (Time.realtimeSinceStartup > _prevTime + _delay + _offset)
        {
            _prevTime = Time.realtimeSinceStartup;
            _renderer.enabled = true;
            _collider.enabled = false;

            if (Physics2D.OverlapBox(transform.position, _collider.size, 0, _cat))
            {
                // CatMovement.Fall();
            }

            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
        _renderer.enabled = false;
    }

    void SetCatOnPlatform(Collision2D other, bool value)
    {
        bool player = other.gameObject.tag == "Player";

        if (player)
        {
            _catOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        SetCatOnPlatform(collider, true);
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        SetCatOnPlatform(collider, false);
    }
}
