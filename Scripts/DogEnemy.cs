using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogEnemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _collider;

    [SerializeField] private LayerMask _cat;
    [SerializeField] private LayerMask _leftBorder;

    private const float _speed = 10.0f;
    private const float _occurenceEveryNSecs = 20.0f;

    private float _prevRunTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(10.8f, -4.3f, 0.0f);
        _rb.velocity = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > _prevRunTime + _occurenceEveryNSecs)
        {
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("GG", true);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(GameOver());
        }

        if (collision.gameObject.tag == "LeftBorder")
        {
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator Respawn()
    {
        _prevRunTime = Time.realtimeSinceStartup;
        
        yield return new WaitForSeconds(1.0f);

        transform.position = new Vector3(10.8f, -4.3f, 0.0f);
        _rb.velocity = new Vector2(0.0f, 0.0f);

        Run();
    }

    private void Run()
    {
        _rb.velocity = new Vector2(-_speed, 0.0f);
    }
}
