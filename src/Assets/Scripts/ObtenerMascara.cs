using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtenerMascara : MonoBehaviour
{
    [Header("Prefab a instanciar")]
    public GameObject prefab;
    public float eje_X = 1f;
    public float eje_Y = 0.5f;

    [Header("Distancia delante del jugador")]
    public float distancia = 1f;

    private bool tieneMascara = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (tieneMascara && Input.GetKeyDown(KeyCode.Space) && playerMovement != null && CheckGround.isGrounded)
        {
            InstanciarObjetoDelante();
        }
    }

    void InstanciarObjetoDelante()
    {
        // ✅ Usa la escala del hijo spriteTransform
        float direccion = playerMovement.spriteTransform.localScale.x > 0 ? 1f : -1f;

        // Posición basada en dirección y offset
        Vector3 offset = new Vector3(eje_X * direccion, -eje_Y, 0f);
        Vector3 posicion = transform.position + offset;

        // Instancia el prefab
        GameObject instancia = Instantiate(prefab, posicion, Quaternion.identity);

        // Ajusta escala del prefab para que mire hacia donde mira el sprite
        Vector3 escalaPrefab = instancia.transform.localScale;
        escalaPrefab.x = Mathf.Abs(escalaPrefab.x) * direccion;
        instancia.transform.localScale = escalaPrefab;

        Destroy(instancia, 0.5f);

        // Aplica el stun al jugador
        if (playerMovement != null)
        {
            StartCoroutine(StunJugador(playerMovement.stunTime));
        }
    }

    IEnumerator StunJugador(float tiempo)
    {
        playerMovement.isStunned = true;
        yield return new WaitForSeconds(tiempo);
        playerMovement.isStunned = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mascara"))
        {
            tieneMascara = true;
            Destroy(other.gameObject);
        }
    }
}
