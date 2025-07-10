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
    public float distanciaMinimaSegura = 0.7f;

    [Header("Disparo")]
    public bool puedeDisparar = true;
    public float distanciaDisparo = 6f;
    public float shootCooldown = 2f;
    private float lastShootTime;

    [Header("Embiste")]
    public bool puedeAtacarConEmbiste = false;
    public float distanciaEmbiste = 2f;
    public float velocidadEmbiste = 5f;
    public float distanciaRetroceso = 2f;
    public float velocidadRetroceso = 3f;
    public float cooldownEmbiste = 3f;
    private float ultimoEmbisteTime;
    private bool estaEmbistiendo = false;
    private bool enCooldownEmbiste = false;

    [Header("Vida")]
    public int Health = 2;

    [Header("Apariencia")]
    public bool aimAtPlayer = true;

    [Header("Rango de visión")]
    public float visionRange = 7f;
    public float visionAngle = 90f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (Player == null || !JugadorEnVision()) return;

        Vector3 direction = Player.transform.position - transform.position;
        Vector3 scale = transform.localScale;
        scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (puedeAtacarConEmbiste)
        {
            ProcesarEmbiste();
        }
        else
        {
            SeguirAlJugador();
        }

        if (puedeDisparar && !estaEmbistiendo)
        {
            IntentarDisparar();
        }
    }

    void SeguirAlJugador()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 direction = (Player.transform.position - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
        else if (distance < distanciaMinimaSegura)
        {
            transform.position -= (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    void IntentarDisparar()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if (distance <= distanciaDisparo && Time.time >= lastShootTime + shootCooldown)
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

    void ProcesarEmbiste()
    {
        if (enCooldownEmbiste || estaEmbistiendo) return;

        float distancia = Vector2.Distance(transform.position, Player.transform.position);
        if (distancia <= distanciaEmbiste && Time.time >= ultimoEmbisteTime + cooldownEmbiste)
        {
            StartCoroutine(EmbisteCoroutine());
        }
    }

IEnumerator EmbisteCoroutine()
{
    estaEmbistiendo = true;

    Vector3 objetivo = Player.transform.position;

    // 1. Movimiento directo hacia el jugador (embiste)
    while (Vector2.Distance(transform.position, objetivo) > 0.1f)
    {
        Vector3 dir = (objetivo - transform.position).normalized;
        transform.position += dir * velocidadEmbiste * Time.deltaTime;
        yield return null;
    }

    yield return new WaitForSeconds(0.2f); // Pausa tras embestir

    // 2. Movimiento curvo hacia adelante y arriba
    Vector3 puntoInicio = transform.position;
    float direccionX = transform.localScale.x > 0 ? 1f : -1f;

    // Usa la distanciaRetroceso como longitud de salida, y velocidadRetroceso para duración
    Vector3 puntoFinal = puntoInicio + new Vector3(direccionX * distanciaRetroceso, distanciaRetroceso * 0.6f, 0f);

    float duracion = distanciaRetroceso / Mathf.Max(velocidadRetroceso, 0.01f); // Evita dividir por 0
    float tiempo = 0f;

    while (tiempo < duracion)
    {
        float t = tiempo / duracion;

        // Movimiento lineal hacia el punto final
        Vector3 posicion = Vector3.Lerp(puntoInicio, puntoFinal, t);

        // Eleva en forma de arco
        posicion.y += Mathf.Sin(t * Mathf.PI) * (distanciaRetroceso * 0.4f);

        transform.position = posicion;

        tiempo += Time.deltaTime;
        yield return null;
    }

    // 3. Cooldown
    enCooldownEmbiste = true;
    estaEmbistiendo = false;
    ultimoEmbisteTime = Time.time;

    yield return new WaitForSeconds(cooldownEmbiste);
    enCooldownEmbiste = false;
}

    public void TakeDamage(float amount)
    {
        Health -= Mathf.RoundToInt(amount);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 forward = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Quaternion leftRayRotation = Quaternion.Euler(0, 0, visionAngle / 2);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, -visionAngle / 2);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftRayDirection * visionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRange);
    }
}
