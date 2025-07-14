using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public static bool isGrounded;  // Indicador de si el jugador est� tocando el suelo
    public LayerMask groundLayer;   // Capa para identificar el suelo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0 && collision.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("OnTriggerEnter: Jugador toc� el suelo");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0 && collision.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("OnTriggerStay: Jugador en el suelo");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = false;
            Debug.Log("OnTriggerExit: Jugador sali� del suelo");
        }
    }

    private void Update()
    {
        Debug.Log("Estado actual de isGrounded: " + isGrounded);
    }
}
