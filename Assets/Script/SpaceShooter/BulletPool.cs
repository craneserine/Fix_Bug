using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    private List<GameObject> pooledPlayerBullets = new List<GameObject>();
    private List<GameObject> pooledEnemyBullets = new List<GameObject>();
    private int amountOfBullets = 10;

    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Initialize player bullets
        for (int i = 0; i < amountOfBullets; i++)
        {
            GameObject bullet = Instantiate(playerBulletPrefab);
            bullet.SetActive(false);
            pooledPlayerBullets.Add(bullet);
        }

        // Initialize enemy bullets
        for (int i = 0; i < amountOfBullets; i++)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab);
            bullet.SetActive(false);
            pooledEnemyBullets.Add(bullet);
        }
    }

    public GameObject GetPooledBullet(string bulletType)
    {
        List<GameObject> selectedPool = bulletType == "Player" ? pooledPlayerBullets : pooledEnemyBullets;

        for (int i = 0; i < selectedPool.Count; i++)
        {
            if (selectedPool[i] != null && !selectedPool[i].activeInHierarchy)
            {
                return selectedPool[i];
            }
        }
        return null; // Optionally, you could expand the pool here if needed
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            bullet.SetActive(false);
        }
    }
}