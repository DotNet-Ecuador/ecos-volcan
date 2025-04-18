using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarArma : MonoBehaviour
{
    public GameObject Capsule;
    public Sprite armaNueva;  // El sprite de la nueva arma
    private SpriteRenderer spriteRenderer;

    private PlayerMovement playerMovement;  // Referencia al script PlayerMovement

    void Start()
    {
        // Obtén el componente PlayerMovement de Capsule
        playerMovement = Capsule.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            // Si PlayerMovement se encuentra, se puede acceder a ammo
            int valor = playerMovement.score;
            Debug.Log("Ammo al inicio: " + valor);
        }

        // Obtener el SpriteRenderer del objeto que tiene el arma
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Verifica si los puntos han llegado a 60
        if (playerMovement != null && playerMovement.score >= 40)
        {
            CambiarImagenArma();
        }
    }

    void CambiarImagenArma()
    {
        spriteRenderer.sprite = armaNueva;  // Cambia el sprite del arma
    }
}
