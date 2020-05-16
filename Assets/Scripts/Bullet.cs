using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private float speed = 12f;

    float xLimit = 9;
    float yLimit = 5;

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if(transform.position.y > yLimit+4 || transform.position.y < -yLimit-4 || 
            transform.position.x > xLimit+4 || transform.position.x < -xLimit-4)
        {
            Destroy(this.gameObject);
        }
	}
    public void SetSpeed (float s)
    {
        speed = s;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            FindObjectOfType<GameController>().AddScore(1);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Destroy(collision.gameObject);
            FindObjectOfType<GameController>().GameOver();
        }
    }
}
