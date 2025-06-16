using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaRompible : MonoBehaviour
{
    public GameObject efectoRompible; // Partículas o efecto visual al romper

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("PlayerMelee"))
        {
            if (efectoRompible != null)
                Instantiate(efectoRompible, transform.position, Quaternion.identity);

            Destroy(gameObject); // Destruye la puerta de entrada
        }
    }
}
