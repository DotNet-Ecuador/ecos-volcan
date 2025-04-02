using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed = 5f;  // Velocidad de la bala

    private Rigidbody2D rb2d;
    private Vector2 direction;

    // Método para establecer la dirección de la bala
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;  // Asegura que la dirección esté normalizada
       // Destroy(gameObject, 3f);  // Destruir la bala después de 3 segundos
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();  // Obtener el componente Rigidbody2D
    }

    void Update()
    {
        // Mueve la bala en la dirección establecida, con la velocidad especificada
        rb2d.velocity = direction * Speed;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (player != null )
        {
            player.Hit();
        }
        if (enemy != null)
        {
            enemy.Hit();
        }
        DestroyBullet();
    }
}
