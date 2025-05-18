using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab;

    private Vector3 originalSpriteScale;
    public GameObject PowerBulletPrefab;
    public Transform spriteTransform; // arrastra aquí el hijo "Sprite"
    public float runSpeed = 2;
    public Animator spriteAnimator; // Asignar el Animator del objeto hijo "Sprite"
    public float jumpSpeed = 3;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ammo = 10;
    public int score = 0;
    public float meleeDamage = 10f; // Daño cuerpo a cuerpo
    public Image progressBar; // Esta es tu barra de progreso
    public UIDamageDisplay damageDisplay;
    public Image healthBar; // Barra de vida
    public GameObject meleeEffectPrefab;

    public Collider2D meleeDetectionZone; // Zona de ataque cuerpo a cuerpo
    public LayerMask enemyLayer; // Layer de enemigos para detectar
    [HideInInspector] public bool isStunned = false;
    private Rigidbody2D rb2d;
    public float stunTime = 1f; // Tiempo de stun en segundos, puedes ajustarlo desde el Inspector
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float Horizontal;
    private float LastShoot;

    private float progress = 0f;
    private float maxProgress = 100f;
    private bool tieneMascara = false;

    private HUDManager hudManager;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hudManager = FindObjectOfType<HUDManager>();
        originalSpriteScale = spriteTransform.localScale;

    }

    private void FixedUpdate()
{
    if (isStunned)
    {
        rb2d.velocity = Vector2.zero;
        return;
    }

    rb2d.velocity = new Vector2(Horizontal * runSpeed, rb2d.velocity.y);
}


void Update()
{
    if (isStunned)
    {
        spriteAnimator.SetFloat("Speed", 0);
        return;
    }

    // Leer entrada
    Horizontal = Input.GetAxisRaw("Horizontal");

    // Girar sprite si se mueve (solo cambiar X en escala del hijo, no usar -1 ni 1 fijos)
     if (!isStunned)
     {
            if (Horizontal < 0)
                spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
            else if (Horizontal > 0)
                spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
     }
     
    // Usar velocidad real en X para determinar animación
        float speedValue = Mathf.Abs(rb2d.velocity.x);
    spriteAnimator.SetFloat("Speed", speedValue);

    // Saltar
    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckGround.isGrounded)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
    }

    // Disparo o ataque cuerpo a cuerpo
    if (Input.GetKey("space") && Time.time > LastShoot + 0.25f)
    {
        if (IsEnemyNearby())
        {
            MeleeAttack();
        }
        else
        {
            Shoot();
        }

        LastShoot = Time.time;
    }
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mascara"))
        {
            tieneMascara = true;
            Destroy(other.gameObject);
        }
    }

   private void Shoot()
{
    if (tieneMascara)
        return;

    if (ammo > 0)
    {
        ammo--;

        if (hudManager != null)
            hudManager.UpdateHUD();

        // 🔁 Ahora usamos la escala del objeto sprite (el hijo) para determinar la dirección
        float directionX = Mathf.Sign(spriteTransform.localScale.x);
        Vector3 direction = new Vector3(directionX, 0, 0);

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
        if (tieneMascara)
            return;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(
            meleeDetectionZone.bounds.center,
            meleeDetectionZone.bounds.size,
            0f,
            enemyLayer
     );

        // 🟢 Instancia el prefab de animación (slash, golpe visual, etc.)
        if (meleeEffectPrefab != null)
        {
            Debug.Log("Instanciando efecto de melee");
            GameObject effect = Instantiate(meleeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1.5f); // Se destruye automáticamente
        }


        foreach (Collider2D enemy in enemies)
        {
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(meleeDamage);
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
