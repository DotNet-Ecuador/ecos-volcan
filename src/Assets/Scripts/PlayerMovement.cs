using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float runSpeed = 2;
    public float jumpSpeed = 3;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ammo = 10;
    public int score = 0; // Puntaje actual del jugador


    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float Horizontal;
    private bool Grounded;
    private float LastShoot;

    // Referencia al HUDManager
    private HUDManager hudManager;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Buscar HUDManager en la escena
        hudManager = FindObjectOfType<HUDManager>();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Horizontal * runSpeed, rb2d.velocity.y);
    }

    void Update()
    {
        // Capturar movimiento horizontal
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Configurar dirección del sprite
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Configurar animación: Idle cuando no hay movimiento
        float speedValue = Mathf.Abs(Horizontal * runSpeed);
        animator.SetFloat("Speed", speedValue);

        if (speedValue == 0)
        {
            animator.Play("Idle"); // Asegura que la animación de Idle se ejecute
        }

        // Salto
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            && CheckGround.isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        // Disparo
        if (Input.GetKey("space") && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        if (ammo > 0) // Dispara solo si hay balas
        {
            ammo--;

            // Llamar a HUDManager para actualizar la UI
            if (hudManager != null)
            {
                hudManager.UpdateHUD(); // Actualiza la barra de vida y munición
            }
            else
            {
                Debug.LogWarning("HUDManager no encontrado");
            }

            Vector3 direction = (transform.localScale.x == 1) ? Vector2.right : Vector2.left;
            GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(direction);
        }
    }

    public void Hit()
    {
        health -= 10;
        if (health <= 0) Destroy(gameObject);
    }
    public void AddScore(int amount)
    {
        score += amount;

        if (hudManager != null)
        {
            hudManager.UpdateHUD(); // Actualiza la UI
        }
    }

}
