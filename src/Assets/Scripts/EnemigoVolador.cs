using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject Player;
    public GameObject BulletPrefab;

    [Header("Movimiento")]
    public float speed = 2f;
    public float stoppingDistance = 1f;

    [Header("Disparo")]
    public bool puedeDisparar = true;
    public float distanciaDisparo = 6f;
    public float shootCooldown = 2f;
    private float lastShootTime;

    [Header("Vida")]
    public int Health = 2;

    [Header("Apariencia")]
    public bool aimAtPlayer = true;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f; // Desactiva la gravedad
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (Player == null) return;

        SeguirAlJugador();
        IntentarDisparar();

        // Voltear sprite según dirección
        Vector3 direction = Player.transform.position - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void SeguirAlJugador()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if (distance > stoppingDistance)
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    void IntentarDisparar()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if (puedeDisparar && distance <= distanciaDisparo && Time.time >= lastShootTime + shootCooldown)
        {
            Disparar();
            lastShootTime = Time.time;
        }
    }

    void Disparar()
    {
        if (BulletPrefab == null) return;

        Vector3 shootDirection = aimAtPlayer && Player != null
            ? (Player.transform.position - transform.position).normalized
            : Vector3.right;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + shootDirection * 0.5f, Quaternion.identity);

        BulletScript bs = bullet.GetComponent<BulletScript>();
        if (bs != null)
        {
            bs.SetDirection(shootDirection);
            bs.isEnemyBullet = true;
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= Mathf.RoundToInt(amount);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}