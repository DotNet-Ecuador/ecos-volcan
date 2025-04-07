using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed = 5f;
    public bool isEnemyBullet = false; // Define si la bala es enemiga

    private Rigidbody2D rb2d;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2d.velocity = direction * Speed;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyBullet)
        {
            // Si es bala enemiga, daña al jugador
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Hit();
                DestroyBullet();
            }
        }
        else
        {
            // Si es bala del jugador, daña a enemigos
            EnemyScript enemy = collision.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.Hit();
                DestroyBullet();
            }
        }
    }
}
