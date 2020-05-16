using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float speed = 5;
    [SerializeField] private float cooldownTime = 0.3f;
    [SerializeField] private ScreenShake shake;
    [SerializeField] private float yLimit;
    [SerializeField] private float xLimit;

    private AudioSource audio;
    private Animator anim;
    private bool canAttack;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        canAttack = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical).normalized;

        transform.position = ((Vector2)transform.position + movement * speed * Time.deltaTime);

        if (transform.position.y > yLimit)
        {
            transform.position = new Vector2(transform.position.x, yLimit);
        }

        if (transform.position.x > xLimit)
        {
            transform.position = new Vector2(xLimit, transform.position.y);
        }

        if (transform.position.y < -yLimit-0.8f)
        {
            transform.position = new Vector2(transform.position.x, -yLimit-0.8f);
        }

        if (transform.position.x < -xLimit)
        {
            transform.position = new Vector2(-xLimit, transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            StartCoroutine(Attack());
        }
	}

    private IEnumerator Attack() {
        anim.SetTrigger("Attack");
        audio.Play();
        shake.Shake();
        canAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
    }
}
