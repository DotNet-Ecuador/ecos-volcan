using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D mainCollider;

    void Start()
    {
        mainCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Si el jugador está subiendo, ignorar la colisión
                if (playerRb.velocity.y > 0)
                {
                    Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), mainCollider, true);
                }
                else
                {
                    // Si el jugador está cayendo o quieto, permitir la colisión
                    Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), mainCollider, false);
                }
            }
        }
    }
}
