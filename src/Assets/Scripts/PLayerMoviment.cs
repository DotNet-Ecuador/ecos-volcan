using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float runSpeed = 2;
    public float jumpSpeed = 3;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float Horizontal;
    private bool Grounded;
    private float LastShoot;

    // Método para disparar las balas
    private void Shoot()
    {
        Vector3 direction = (transform.localScale.x == 1) ? Vector2.right : Vector2.left; // Verifica hacia donde el personaje está mirando
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction); // Pasa la dirección a la bala
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtener el Animator
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Horizontal * runSpeed, rb2d.velocity.y); // Movimiento horizontal

        // Actualización del Animator con la velocidad
        animator.SetFloat("Speed", Mathf.Abs(Horizontal));
    }


    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Control de salto
        if (Input.GetKey("w") && CheckGround.isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        // Control de dirección y animación
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Mover a la izquierda
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Mover a la derecha

        // Detectar la acción de disparo
        if (Input.GetKey("space") && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }
}
