using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SpaceshipController : MonoBehaviour
{
    public List<EnemySpaceShooter> Enemies;

    public float Speed;
    public float BulletSpeed;
    public GameObject playerBulletPrefab; // Reference to the player bullet prefab
    public Transform BulletSpawnHere; // Position where bullets will spawn
    public GameObject GameClearScreen;
    public TextMeshProUGUI textValue, hpValue;
    public int score;
    public int hitponts;
    bool isGameClear = false;
    private int storeHP;
    public GameObject GameOverScreen;
    private bool canMove = true;
    private bool canShoot = true;

    void Start()
    {
        storeHP = hitponts;
    }

    void Update()
    {
        textValue.text = score.ToString();
        hpValue.text = hitponts.ToString();

        // Check for shooting input
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            SpawnBullet(); 
        }

        if (hitponts <= 0)
        {
            canShoot = false;
            canMove = false;
            GameOverScreen.SetActive(true);
            hitponts = 0;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 moveInput = new Vector3(horizontalInput, 0, 0);
            transform.position += Time.deltaTime * Speed * moveInput;
        }
    }

    public void SpawnBullet()
    {
        // Call GetPooledBullet with "Player" to get a player bullet
        GameObject bullet = BulletPool.instance.GetPooledBullet("Player");
        if (bullet != null)
        {
            bullet.transform.position = BulletSpawnHere.position; // Set the bullet's position
            bullet.SetActive(true); // Activate the bullet
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = new Vector2(0f, BulletSpeed); // Set the bullet's velocity
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            hitponts--;
            BulletPool.instance.ReturnBullet(collision.gameObject); // Return enemy bullet to pool
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.position = Enemies[i].InitialPosition;
            Enemies[i].gameObject.SetActive(false);
            StartCoroutine(DelayEnemiesActive());
        }
        canMove = true;
        canShoot = true;
        hitponts = storeHP;
        score = 0;
        isGameClear = false;
        GameOverScreen.SetActive(false);
    }

    IEnumerator DelayEnemiesActive()
    {
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].gameObject.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGameClear()
    {
        isGameClear = true;
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject.activeSelf)
            {
                isGameClear = false;
                break;
            }
        }
        if (isGameClear)
        {
            GameClearScreen.SetActive(true);
        }
    }
}