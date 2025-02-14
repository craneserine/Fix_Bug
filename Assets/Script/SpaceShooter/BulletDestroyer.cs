using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("EnemyBullet"))
        {
            // Return the bullet to the pool instead of destroying it
            BulletPool.instance.ReturnBullet(collision.gameObject);
        }
    }
}