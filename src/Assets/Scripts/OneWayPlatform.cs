using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D mainCollider; // El collider sólido de la plataforma

    void Start()
    {
        mainCollider = GetComponent<Collider2D>(); // Obtiene el collider principal
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ignorar colisión si el jugador toca la plataforma desde abajo
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), mainCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reactivar la colisión cuando el jugador deja la plataforma
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), mainCollider, false);
        }
    }
}
