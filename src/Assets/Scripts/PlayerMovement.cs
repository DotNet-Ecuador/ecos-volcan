using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject PowerBulletPrefab;
    public float runSpeed = 2;
    public float jumpSpeed = 3;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ammo = 10;
    public int score = 0;
    public float meleeDamage = 10f; // Daño cuerpo a cuerpo
    public Image progressBar; // Esta es tu barra de progreso
    public UIDamageDisplay damageDisplay;
    public Image healthBar; // Barra de vida

    public Collider2D meleeDetectionZone; // Zona de ataque cuerpo a cuerpo
    public LayerMask enemyLayer; // Layer de enemigos para detectar

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float Horizontal;
    private bool Grounded;
    private float LastShoot;

    private float progress = 0f;
    private float maxProgress = 100f;

    private HUDManager hudManager;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hudManager = FindObjectOfType<HUDManager>();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Horizontal * runSpeed, rb2d.velocity.y);
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        float speedValue = Mathf.Abs(Horizontal * runSpeed);
        animator.SetFloat("Speed", speedValue);

        if (speedValue == 0)
            animator.Play("Idle");

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckGround.isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if (Input.GetKey("space") && Time.time > LastShoot + 0.25f)
        {
            if (IsEnemyNearby())
            {
                MeleeAttack(); // Ataque cuerpo a cuerpo si hay un enemigo cerca
            }
            else
            {
                Shoot(); // Disparo normal o de poder
            }

            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        if (ammo > 0)
        {
            ammo--;

            if (hudManager != null)
                hudManager.UpdateHUD();

            Vector3 direction = (transform.localScale.x == 1) ? Vector2.right : Vector2.left;
            GameObject bulletToUse = (score >= 40 && PowerBulletPrefab != null) ? PowerBulletPrefab : BulletPrefab;

            GameObject bullet = Instantiate(bulletToUse, transform.position + direction * 0.5f, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(direction);
        }
    }

    private bool IsEnemyNearby()
    {
        if (meleeDetectionZone == null) return false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            meleeDetectionZone.bounds.center,
            meleeDetectionZone.bounds.size,
            0f,
            enemyLayer
        );

        return hits.Length > 0;
    }

    private void MeleeAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(
        meleeDetectionZone.bounds.center,
        meleeDetectionZone.bounds.size,
        0f,
        enemyLayer
    );

        foreach (Collider2D enemy in enemies)
        {
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(meleeDamage);

                // 🟢 Añade progreso al hacer daño cuerpo a cuerpo
                AddProgress(10f); // Puedes ajustar este valor
            }
        }

        Debug.Log("¡Ataque cuerpo a cuerpo ejecutado!");
    }

    public void Hit()
    {
        health -= 10;

        if (damageDisplay != null)
        {
            damageDisplay.QuitarVida(); // Oculta una vida visual
        }

        if (health <= 0)
        {
            Destroy(gameObject); // Destruye al jugador si su vida numérica llega a 0
        }
    }

    public void AddScore(int amount)
    {
        int previousScore = score;
        score += amount;

        if (previousScore < 40 && score >= 40)
        {
            ammo += 60; // Regala 60 balas al activar modo de poder
            Debug.Log("¡Modo de poder activado! +60 balas");
        }

        if (hudManager != null)
            hudManager.UpdateHUD();
    }

    public void AddProgress(float amount)
    {
        progress += amount;

        // Limita el valor máximo
        progress = Mathf.Clamp(progress, 0, maxProgress);

        // Actualiza visualmente
        if (progressBar != null)
            progressBar.fillAmount = progress / maxProgress;
    }


    private void OnDrawGizmosSelected()
    {
        if (meleeDetectionZone != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(meleeDetectionZone.bounds.center, meleeDetectionZone.bounds.size);
        }
    }
}
