using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Text scoreOverText;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private Slider comboMeter;

    [SerializeField] private GameObject enemyPrefab;
    private AudioSource audio;

    private int score;
    private int combo;
    private bool canRestart;
    private bool comboActive;
    private float yLimit;
    private float xLimit;

    private bool canSpawnEnemy;

    [Header ("Enemy Spawn Settings")]
    [SerializeField] private float enemySpawnTime = 5f;
    [SerializeField] private float enemyBaseSpeed = 5f;
    [SerializeField] private float bulletsBaseSpeed = 9f;

    [Header("Spawn difficulty Settings")]
    [SerializeField] private float spawnAcceleration = 0.05f;
    [SerializeField] private float speedAcceleration = 0.05f;
    [SerializeField] private float bulletAcceleration = 0.05f;

    private void Start()
    {
        Cursor.visible = false;
        audio = GetComponent<AudioSource>();
        xLimit = 8.25f;
        yLimit = 4.33f;
        comboActive = false;
        canSpawnEnemy = true;
        comboMeter.value = 0;
        gameOverImage.SetActive(false);
        combo = 1;
        score = 0;
        canRestart = false;
    }

    public void AddScore(int points)
    {
        score += points * combo;
        comboMeter.value = 1;
        comboActive = true;
        combo++;
        scoreText.text = "score " + score;
        comboText.text = "combo x" + combo;
    }

    public void GameOver()
    {
        audio.Play();
        gameOverImage.SetActive(true);
        scoreOverText.text = "You scored " + score + " points!";
        canRestart = true;
    }

    private void Update()
    {
        if (comboMeter.value >= 0)
        {
            comboMeter.value -= Time.deltaTime;
        }

        if(comboMeter.value <= 0)
        {
            comboActive = false;
            combo = 1;
            comboText.text = "combo x" + combo;
        }

        if (canSpawnEnemy)
        {
            StartCoroutine(SpawnEnemy());
        }

        if (canRestart && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator SpawnEnemy()
    {

        float x = 0;
        float y = 0;
 
        while(x < xLimit+1 && x > -xLimit-1 && y < yLimit+1 && y > -yLimit - 1)
        {
            x = Random.Range(-xLimit - 2, xLimit + 2);
            y = Random.Range(-yLimit - 2, yLimit + 2);
        }
        Vector2 pos = new Vector2(x, y);

        GameObject aux = Instantiate(enemyPrefab, pos, new Quaternion(), transform.parent);
        Enemy enemy = aux.GetComponent<Enemy>();
        enemy.setSpeed(enemyBaseSpeed);
        enemy.setBulletSpeed(bulletsBaseSpeed);

        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemySpawnTime);
        enemyBaseSpeed += speedAcceleration;
        bulletsBaseSpeed += bulletAcceleration;
        enemySpawnTime -= spawnAcceleration;

        canSpawnEnemy = true;
    }
}
