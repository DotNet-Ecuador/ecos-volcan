using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsTrampa : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject Player; // Asigna el jugador desde el Inspector
    public GameObject BulletPrefab;

    [Header("Movimiento")]
    public float speed = 2.0f;
    public float followDistance = 1.5f;

    [Header("Disparo")]
    public bool puedeDisparar = true;
    public bool aimAtPlayer = false;
    public float distanciaDisparo = 5f;
    public float shootCooldown = 1.5f;
    private float lastShootTime;

    [Header("Cuerpo a cuerpo")]
    public bool puedeAtacarCuerpoACuerpo = false;
    public float distanciaMelee = 1.5f;
    public Collider2D meleeZone;
    public LayerMask playerLayer;
    public float meleeCooldown = 2f;
    private float lastMeleeTime;

    [Header("Vida")]
    public int Health = 3;

    [Header("Detección de jugador")]
    public float visionRange = 5f;         // Qué tan lejos puede ver
    public float visionAngle = 60f;        // Ángulo de visión (como un cono frontal)

    [Header("Instancia de Prefab Especial")]
    public bool instanciarPrefabEnVision = false;
    public GameObject prefabAAInstanciar;
    public Vector3 offsetInstancia = Vector3.zero;
    public float cooldownInstancia = 3f;
    private float tiempoUltimaInstancia = -Mathf.Infinity;

    [Header("Avalancha sobre el jugador")]
    public bool puedeAvalanzarse = false;
    public float distanciaAvalancha = 2f;
    public float fuerzaSalto = 8f;
    public float fuerzaSaltoHorizontal = 4f;
    public float cooldownAvalancha = 3f;
    private float ultimoSaltoTiempo;
    private bool estaEnElAire = false;
    [Header("Daño por contacto")]
    public bool dañoAlTocar ;
    [Header("Activación visual al tocar")]
    public GameObject spriteActivable;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Player == null) return;

        Vector3 direction = Player.transform.position - transform.position;

        Vector3 newScale = transform.localScale;
        newScale.x = (direction.x >= 0.0f) ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        float distanceX = Mathf.Abs(Player.transform.position.x - transform.position.x);
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        if (puedeAvalanzarse && distanceToPlayer <= distanciaAvalancha && Time.time >= ultimoSaltoTiempo + cooldownAvalancha)
        {
            Avalanzarse();
        }
        else if (distanceX > followDistance && JugadorEnVision())
        {
            transform.position += new Vector3(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0, 0);
        }

        if (puedeAtacarCuerpoACuerpo && distanceToPlayer <= distanciaMelee)
        {
            MeleeAttack();
        }
        else if (puedeDisparar && distanceToPlayer <= distanciaDisparo && Time.time >= lastShootTime + shootCooldown)
        {
            Disparar();
            lastShootTime = Time.time;
        }

        if (instanciarPrefabEnVision && prefabAAInstanciar != null && JugadorEnVision())
        {
            if (Time.time >= tiempoUltimaInstancia + cooldownInstancia)
            {
                InstanciarPrefabEspecial();
                tiempoUltimaInstancia = Time.time;
            }
        }
    }

    void Avalanzarse()
    {
        if (rb == null) return;

        Vector2 direccion = (Player.transform.position - transform.position).normalized;
        Vector2 impulso = new Vector2(direccion.x * fuerzaSaltoHorizontal, fuerzaSalto);
        rb.velocity = impulso;
        estaEnElAire = true;
        ultimoSaltoTiempo = Time.time;
        Debug.Log("¡El enemigo se abalanzó sobre el jugador!");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaEnElAire && collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Hit();
                Debug.Log("El enemigo golpeó al jugador desde el aire.");
            }
        }

        if (collision.contacts[0].normal.y > 0.5f)
        {
            estaEnElAire = false;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // Activar sprite si está asignado
            if (spriteActivable != null)
            {
                SpriteRenderer sr = spriteActivable.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = true;
            }
        }
    }

    private void InstanciarPrefabEspecial()
    {
        Vector3 instanciaPos = transform.position + offsetInstancia;
        Instantiate(prefabAAInstanciar, instanciaPos, Quaternion.identity);
        Debug.Log("Instanciado prefab especial al detectar jugador en visión.");
    }

    private void Disparar()
    {
        
        if (BulletPrefab == null) return;

        Vector3 shootDirection;

        if (aimAtPlayer && Player != null)
        {
            shootDirection = (Player.transform.position - transform.position).normalized;
        }
        else
        {
            shootDirection = new Vector3(Mathf.Sign(transform.localScale.x), 0, 0);
        }

        GameObject bullet = Instantiate(BulletPrefab, transform.position + shootDirection * 0.5f, Quaternion.identity);

        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.SetDirection(shootDirection);
        bulletScript.isEnemyBullet = true;
    }

    bool JugadorEnVision()
    {
        Vector2 direccionAlJugador = Player.transform.position - transform.position;
        float distanciaAlJugador = direccionAlJugador.magnitude;

        if (distanciaAlJugador > visionRange) return false;

        direccionAlJugador.Normalize();
        Vector2 direccionMirada = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;

        float angulo = Vector2.Angle(direccionMirada, direccionAlJugador);

        return angulo < visionAngle / 2f;
    }

    private void MeleeAttack()
    {
        if (Time.time < lastMeleeTime + meleeCooldown) return;

        lastMeleeTime = Time.time;

        if (meleeZone == null) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            meleeZone.bounds.center,
            meleeZone.bounds.size,
            0f,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            PlayerMovement player = hit.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Hit();
                Debug.Log("Ataque cuerpo a cuerpo exitoso al jugador.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (meleeZone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(meleeZone.bounds.center, meleeZone.bounds.size);
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
