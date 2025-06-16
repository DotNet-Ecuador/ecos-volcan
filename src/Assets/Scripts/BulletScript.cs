using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed = 5f;
    public bool isEnemyBullet = false;
    private Rigidbody2D rb2d;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        // Orientación visual del sprite
        if (newDirection.x < 0)
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        else if (newDirection.x > 0)
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
    }

    void Start()
{
    rb2d = GetComponent<Rigidbody2D>();
    rb2d.velocity = direction * Speed;

    // Ignora colisión con el jugador si la bala es del jugador
    if (!isEnemyBullet)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D bulletCollider = GetComponent<Collider2D>();
            if (playerCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
        }
    }

    Destroy(gameObject, 4f);
}

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (isEnemyBullet == true)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Hit();
                player.StartCoroutine(player.Invulnerability());
                DestroyBullet();
        }
    }
    else
    {
        // Ignorar si golpea al jugador
        if (collision.CompareTag("Player"))
        {
            return;
        }
        // Hacer daño a enemigos
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);

            HUDManager hud = FindObjectOfType<HUDManager>();
            if (hud != null)
            {
                hud.AddProgress(10);
            }

            DestroyBullet();
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Paredes"))
        {
            DestroyBullet();
        }
    }
}

}
