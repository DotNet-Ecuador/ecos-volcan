using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ZonaCuracion : MonoBehaviour
{
    [Header("Configuración de la Zona")]
    public float tiempoParaCurar = 3f;
    public float duracionZona     = 5f;

    [Header("Enemigos que se abalanzan")]
    public GameObject[] enemigosActivables;

    [Header("Enemigos a los que se les amplía la visión")]
    public GameObject[] enemigosVisionAmpliada;   // ← NUEVO

    [Header("Efecto visual / transición")]
    public GameObject spriteActivable;            // sprite + animator apagados de inicio
    public float   tiempoTransicion   = 1f;
    public int     indiceEscenaDestino = 2;

    private float  tiempoEnZona      = 0f;
    private bool   jugadorEnZona     = false;
    public  float  tiempoParaActivarEnemigos = 4f;

    private GameObject     jugador;
    private PlayerMovement playerMovement;
    private Animator       animator;

    [Header("UI Interacción (Flecha y texto)")]
public CanvasGroup uiCanvasGroup;
public float duracionFadeUI = 0.5f;
private Coroutine fadeUICoroutine;

    /* ---------- TRIGGER ---------- */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !jugadorEnZona && Input.GetKey(KeyCode.UpArrow))
        {
            jugador          = other.gameObject;
            playerMovement   = jugador.GetComponent<PlayerMovement>();
            animator         = jugador.GetComponent<Animator>();

            if (playerMovement != null)
            {
                playerMovement.isStunned   = true;
                playerMovement.rb2d.velocity = Vector2.zero;
            }

            jugadorEnZona  = true;
            tiempoEnZona   = 0f;

            // corutinas principales
            StartCoroutine(CurarDespuesDeTiempo(2f));
            StartCoroutine(ActivarEnemigosDespuesDeTiempo(tiempoParaActivarEnemigos));

            // ➜ Amplía la visión a los enemigos especiales
            CambiarVisionEnemigos(13f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerMovement != null)
        {
            jugadorEnZona          = false;
            playerMovement.isStunned = false;
        }
    }

    /* ---------- LÓGICA ---------- */

    IEnumerator CurarDespuesDeTiempo(float t)
    {
        yield return new WaitForSeconds(t);

        if (playerMovement != null)
        {
            playerMovement.health = playerMovement.maxHealth;
            playerMovement.damageDisplay?.RestaurarVidas();
            if (animator != null && TieneParametro(animator, "Curar"))
                animator.SetTrigger("Curar");
        }
    }

    IEnumerator ActivarEnemigosDespuesDeTiempo(float t)
    {
        yield return new WaitForSeconds(t);

        ActivarEnemigos();      // puedeAvalanzarse = true

        if (spriteActivable != null)
        {
            var sr  = spriteActivable.GetComponent<SpriteRenderer>();
            var an  = spriteActivable.GetComponent<Animator>();
            if (sr != null) sr.enabled = true;
            if (an != null) an.enabled = true;
        }

        yield return new WaitForSeconds(tiempoTransicion);
        SceneManager.LoadScene(indiceEscenaDestino);
    }

    void ActivarEnemigos()
    {
        foreach (var go in enemigosActivables)
        {
            if (go == null) continue;

            EnemyScript e1 = go.GetComponent<EnemyScript>();
            if (e1 != null) { e1.puedeAvalanzarse = true; continue; }

            NPCsTrampa nt = go.GetComponent<NPCsTrampa>();
            if (nt != null) nt.puedeAvalanzarse = true;
        }
    }

    /* ---------- NUEVO: cambiar visión ---------- */
    void CambiarVisionEnemigos(float nuevoRango)
    {
        foreach (var go in enemigosVisionAmpliada)
        {
            if (go == null) continue;

            EnemyScript e1 = go.GetComponent<EnemyScript>();
            if (e1 != null) { e1.visionRange = nuevoRango; continue; }

            NPCsTrampa nt = go.GetComponent<NPCsTrampa>();
            if (nt != null) nt.visionRange = nuevoRango;
        }
    }

    /* ---------- util ---------- */

    bool TieneParametro(Animator anim, string nombre)
    {
        foreach (var p in anim.parameters)
            if (p.name == nombre) return true;
        return false;
    }
}
