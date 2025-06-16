using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject PowerBulletPrefab;
    public Transform spriteTransform;
    public float runSpeed = 2;
    public Animator spriteAnimator;
    public float jumpSpeed = 3;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ammo = 10;
    public int score = 0;
    public float meleeDamage = 10f;
    public Image progressBar;
    public UIDamageDisplay damageDisplay;
    public Image healthBar;
    public GameObject meleeEffectPrefab;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Vector2 bulletOffset = new Vector2(0.5f, 0.5f);
    public float bulletSpeed = 5f;
    public Collider2D meleeDetectionZone;
    public LayerMask enemyLayer;

    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool puedeSaltar = true;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private HUDManager hudManager;
    private Vector3 originalSpriteScale;
    private float Horizontal;
    private float LastShoot;
    private float progress = 0f;
    private float maxProgress = 100f;
    private bool tieneMascara = false;

    // DASH CONFIG
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float originalGravity;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hudManager = FindObjectOfType<HUDManager>();
        originalSpriteScale = spriteTransform.localScale;
        originalGravity = rb2d.gravityScale;
    }

    void FixedUpdate()
    {
        if (isStunned || isDashing)
        {
            if (isDashing)
                rb2d.velocity = new Vector2(spriteTransform.localScale.x > 0 ? dashSpeed : -dashSpeed, 0f);
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

        Horizontal = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            Horizontal = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            Horizontal = 1f;

        if (!isStunned)
        {
            if (Horizontal < 0)
                spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
            else if (Horizontal > 0)
                spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }

        float speedValue = Mathf.Abs(rb2d.velocity.x);
        spriteAnimator.SetFloat("Speed", speedValue);

        // Saltar
        if (Input.GetKey("z") && CheckGround.isGrounded && puedeSaltar)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        // DASH
        if (Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(PerformDash());
        }

        // RESET DASH si toca el suelo una vez
        if (CheckGround.isGrounded && !isDashing)
        {
            canDash = true;
        }

        // Disparo
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

    IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb2d.gravityScale = originalGravity;
    }

    private void Shoot()
    {
        if (tieneMascara) return;
        if (ammo <= 0) return;

        ammo--;
        if (hudManager != null) hudManager.UpdateHUD();

        Vector3 direction;
        Quaternion rotation = Quaternion.identity;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector3.up;
            rotation = Quaternion.Euler(0, 0, -91.04f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector3.down;
            rotation = Quaternion.Euler(0, 0, 90.093f);
        }
        else
        {
            float directionX = Mathf.Sign(spriteTransform.localScale.x);
            direction = new Vector3(directionX, 0, 0);
        }

        GameObject bulletToUse = (score >= 40 && PowerBulletPrefab != null) ? PowerBulletPrefab : BulletPrefab;

        Vector3 spawnPosition = bulletSpawnPoint.position;
        GameObject bullet = Instantiate(bulletToUse, spawnPosition, rotation);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
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
        if (tieneMascara) return;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(
            meleeDetectionZone.bounds.center,
            meleeDetectionZone.bounds.size,
            0f,
            enemyLayer
        );

        if (meleeEffectPrefab != null)
        {
            GameObject effect = Instantiate(meleeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        foreach (Collider2D enemy in enemies)
        {
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(meleeDamage);
            }
        }
    }

    public void Hit()
    {
        health -= 10;
        if (damageDisplay != null)
            damageDisplay.QuitarVida();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        int previousScore = score;
        score += amount;

        if (previousScore < 40 && score >= 40)
        {
            ammo += 60;
        }

        if (hudManager != null)
            hudManager.UpdateHUD();
    }

    public void AddProgress(float amount)
    {
        progress += amount;
        progress = Mathf.Clamp(progress, 0, maxProgress);
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
