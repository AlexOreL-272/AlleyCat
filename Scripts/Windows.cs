using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Windows : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _cat;

    private bool _opened = false;

    private const float _delta = 20.0f;
    private float _offset;
    private float _prevRunTime = 0.0f;

    private Vector2 _size;

    // Start is called before the first frame update
    void Start()
    {
        _offset = Random.Range(0.0f, 15.0f);
        _size = gameObject.GetComponent<BoxCollider2D>().size;
        _animator.SetBool("Opened", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > _prevRunTime + _delta + _offset)
        {
            StartCoroutine(OpenWindow());
        }

        if (_opened && Physics2D.OverlapBox(transform.position, _size, 0, _cat))
        {
            StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Win");
    }

    private IEnumerator OpenWindow()
    {
        _prevRunTime = Time.realtimeSinceStartup;
        _animator.SetBool("Opened", true);
        _opened = true;

        yield return new WaitForSeconds(2.0f);

        _opened = false;
        _animator.SetBool("Opened", false);
    }
}
