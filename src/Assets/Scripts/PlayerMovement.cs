using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public SpriteRenderer childSpriteRenderer; // Asigna el SpriteRenderer del hijo (sprite con animaciones)
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
    private bool haDescubiertoCuartoSecreto = false;
    public GameObject darkOverlay;      // Asignar en el Inspector
    public GameObject globalDarkness;   // Asignar en el Inspector
    public GameObject globalDarknessRight; // ← Nuevo campo para la oscuridad derecha
    public float tiempoOscurecimiento = 2.5f;
    public GameObject meleeEffectPrefab;
    [Header("Disparo")]
    public bool dañoAlTocar ;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Vector2 bulletOffset = new Vector2(0.5f, 0.5f); // Desfase opcional
    public float bulletSpeed = 5f;
    public Collider2D meleeDetectionZone; // Zona de ataque cuerpo a cuerpo
    public LayerMask enemyLayer; // Layer de enemigos para detectar
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool puedeSaltar = true;
    public Rigidbody2D rb2d;
    public float stunTime = 1f; // Tiempo de stun en segundos, puedes ajustarlo desde el Inspector
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float Horizontal;
    private float LastShoot;

    private float progress = 0f;
    private float maxProgress = 100f;
    private bool tieneMascara = false;

    private HUDManager hudManager;

    [Header("Respawn System")]
    public Transform respawnPoint; // Asigna este Empty GameObject desde el Inspector
    public float invulnerabilityTime = 2f;
    private bool isInvulnerable = false;
    
        [Header("Comienzo atrapado")]
    public bool empiezaAtrapado = true;      // ¿Arranca atado a la pared?
    public float impulsoAlSoltar = 2f;       // Pequeño salto al liberarse

    private float gravedadOriginal;          // Guardamos la gravedad real

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hudManager = FindObjectOfType<HUDManager>();
        originalSpriteScale = spriteTransform.localScale;

            if (globalDarkness != null)
        globalDarkness.SetActive(false); // Ocúltalo al iniciar el juego

          gravedadOriginal = rb2d.gravityScale;

        if (empiezaAtrapado)                 // ← ¡Se despierta colgado!
        {
            isStunned        = true;        // No puede moverse
            rb2d.gravityScale = 0f;         // Sin gravedad = pegado a la pared
            rb2d.velocity     = Vector2.zero;
        }
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
     /* ① Si está atrapado, vigilamos la combinación
           tecla X  +  cualquiera de las flechas */
        if (empiezaAtrapado && isStunned)
        {
            bool pulsaX      = Input.GetKeyDown(KeyCode.X);
            bool pulsaFlecha = Input.GetKey(KeyCode.LeftArrow)  ||
                               Input.GetKey(KeyCode.RightArrow) ||
                               Input.GetKey(KeyCode.UpArrow)    ||
                               Input.GetKey(KeyCode.DownArrow);

            if (pulsaX && pulsaFlecha)
                Liberarse();
            return;                         // Mientras, ignoramos el resto
        }

     if (isStunned)
    {
        Horizontal = 0f;
        spriteAnimator.SetFloat("Speed", 0);
        return;
    }

   Horizontal = 0f;
if (Input.GetKey(KeyCode.LeftArrow))
    Horizontal = -1f;
else if (Input.GetKey(KeyCode.RightArrow))
    Horizontal = 1f;

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
    if (Input.GetKey("z") && CheckGround.isGrounded && puedeSaltar)
{
    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
}


    // Disparo o ataque cuerpo a cuerpo
    if (Input.GetKey("x") && Time.time > LastShoot + 0.25f)
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
        
        if ((other.CompareTag("Enemy") || other.CompareTag("Bullet")) && !isInvulnerable)
        {
            Hit();
            StartCoroutine(Invulnerability());
        }

        if (other.CompareTag("CuartoOculto") && !haDescubiertoCuartoSecreto)
        {
            haDescubiertoCuartoSecreto = true;

            if (darkOverlay != null)
                darkOverlay.SetActive(false);

            if (globalDarkness != null)
                globalDarkness.SetActive(true); // Oscurece la izquierda

            if (globalDarknessRight != null)
                globalDarknessRight.SetActive(true); // Oscurece la derecha también

            StartCoroutine(DesactivarOscurecimiento());
        }
    // Al salir del cuarto oculto (por ejemplo, atravesando la salida)
if (other.CompareTag("SalidaCuartoOculto"))
{
    if (globalDarkness != null)
        globalDarkness.SetActive(false);

    if (globalDarknessRight != null)
        globalDarknessRight.SetActive(false);

    Debug.Log("¡El jugador ha salido del cuarto oculto! Oscuridad desactivada.");
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

        Vector3 direction;
        Quaternion rotation = Quaternion.identity;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Disparo hacia arriba
            direction = Vector3.up;
            rotation = Quaternion.Euler(0, 0, -91.04f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Disparo hacia abajo
            direction = Vector3.down;
            rotation = Quaternion.Euler(0, 0, 90.093f);
        }
        else
        {
            // Disparo lateral
            float directionX = Mathf.Sign(spriteTransform.localScale.x);
            direction = new Vector3(directionX, 0, 0);
            rotation = Quaternion.identity;
        }

        GameObject bulletToUse = (score >= 40 && PowerBulletPrefab != null) ? PowerBulletPrefab : BulletPrefab;

        Vector3 spawnPosition = bulletSpawnPoint.position;
        GameObject bullet = Instantiate(bulletToUse, spawnPosition, rotation);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }
}

public void RespawnPlayer()
{
    if (isInvulnerable) return;

    Hit(); // Le quita vida al jugador
    StartCoroutine(Invulnerability());

    // Mover al punto de reaparición
    if (respawnPoint != null)
        transform.position = respawnPoint.position;
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


public IEnumerator Invulnerability()
{
    isInvulnerable = true;

    // Ignora colisiones con enemigos y balas
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bala"), true);

    float elapsed = 0f;
    while (elapsed < invulnerabilityTime)
    {
        if (childSpriteRenderer != null)
            childSpriteRenderer.enabled = !childSpriteRenderer.enabled;

        yield return new WaitForSeconds(0.1f);
        elapsed += 0.1f;
    }

    // Reactiva las colisiones
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bala"), false);

    if (childSpriteRenderer != null)
        childSpriteRenderer.enabled = true;

    isInvulnerable = false;
}



    private IEnumerator DesactivarOscurecimiento()
    {
        yield return new WaitForSeconds(tiempoOscurecimiento);

        if (globalDarkness != null)
            globalDarkness.SetActive(false); // Vuelve a iluminar el entorno
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

public void HitConKnockback(Vector2 direccion)
{ 
    float fuerzaRetrocesoX = 10f;
    float fuerzaRetrocesoY = 4f;

    // Asegura que siempre haya dirección horizontal
    float direccionX = direccion.x;
    if (Mathf.Abs(direccionX) < 0.1f)
        direccionX = (transform.position.x < direccion.x) ? -1 : 1;

    Vector2 impulso = new Vector2(-Mathf.Sign(direccionX) * fuerzaRetrocesoX, fuerzaRetrocesoY);

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Resetear velocidad antes del empuje
            rb.AddForce(impulso, ForceMode2D.Impulse);
    }

    Debug.Log("Jugador golpeado con retroceso mejorado");
    
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

     private void Liberarse()
    {
        isStunned          = false;         // Recupera control
        empiezaAtrapado    = false;         // No volverá a esta lógica
        rb2d.gravityScale  = gravedadOriginal;
        rb2d.velocity      = new Vector2(rb2d.velocity.x, impulsoAlSoltar);
        // Si quieres un empujón lateral, añade aquí otro componente X
    }
}
