using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Transform playerPosition;
    private AudioSource audio;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private GameObject bulletPrefab;

    private float speed = 5;
    private float bulletSpeed;
    private float xLimit;
    private float yLimit;
    private float shootingTime = 0.5f;
    private bool canShoot;
    private Vector2 direction;
    private bool enteredScreen;

	// Use this for initialization
	private void  Awake() {
        audio = GetComponent<AudioSource>();
        enteredScreen = false;
        playerPosition = FindObjectOfType<Player>().transform;
        canShoot = true;
        yLimit = 4.33f;
        xLimit = 8.25f;
        float x = 0.0f;
        float y = 0.0f;

        while (x == 0.0f && y == 0.0f)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);
        }

        direction = new Vector2(x, y);
	}

    public void setSpeed(float s) {
        speed = s;
    }

    public void setBulletSpeed(float s)
    {
        bulletSpeed = s;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;

        if (enteredScreen)
        {
            if (transform.position.y > yLimit)
            {
                transform.position = new Vector2(transform.position.x, yLimit);
                direction = new Vector2(direction.x, Random.Range(-1f, 0f));
            }

            if (transform.position.y < -yLimit)
            {
                transform.position = new Vector2(transform.position.x, -yLimit);
                direction = new Vector2(direction.x, Random.Range(0f, 1f));
            }

            if (transform.position.x > xLimit)
            {
                transform.position = new Vector2(xLimit, transform.position.y);
                direction = new Vector2(Random.Range(-1f, 0f), direction.y);
            }

            if (transform.position.x < -xLimit)
            {
                transform.position = new Vector2(-xLimit, transform.position.y);
                direction = new Vector2(Random.Range(0f, 1f), direction.y);
            }
        }

        else
        {
            float yLimitOut = yLimit + 2;
            float xLimitOut = xLimit + 2;

            if (transform.position.y > yLimitOut)
            {
                transform.position = new Vector2(transform.position.x, yLimitOut);
                direction = new Vector2(direction.x, Random.Range(-1f, 0f));
            }

            if (transform.position.y < -yLimitOut)
            {
                transform.position = new Vector2(transform.position.x, -yLimitOut);
                direction = new Vector2(direction.x, Random.Range(0f, 1f));
            }

            if (transform.position.x > xLimitOut)
            {
                transform.position = new Vector2(xLimitOut, transform.position.y);
                direction = new Vector2(Random.Range(-1f, 0f), direction.y);
            }

            if (transform.position.x < -xLimitOut)
            {
                transform.position = new Vector2(-xLimitOut, transform.position.y);
                direction = new Vector2(Random.Range(0f, 1f), direction.y);
            }

            if (transform.position.y < yLimit && transform.position.y > -yLimit &&
                 transform.position.x < xLimit && transform.position.x > -xLimit)
            {
                enteredScreen = true;
            }
        }

        Vector2 dir = playerPosition.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Destroy(collision.gameObject);
            FindObjectOfType<GameController>().GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            audio.PlayOneShot(explosion);
            FindObjectOfType<GameController>().AddScore(2);
            Destroy(gameObject);
        }
    }


    private IEnumerator Shoot()
    {
        GameObject temp;
        audio.Play();
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, rotation.z + 180);
        temp = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(rotation));
        temp.GetComponent<Bullet>().SetSpeed(bulletSpeed);
        temp.transform.parent = transform.parent;
        canShoot = false;
        yield return new WaitForSeconds(shootingTime);
        canShoot = true;
    }
}
