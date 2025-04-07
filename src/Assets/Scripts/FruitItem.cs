using UnityEngine;

public class FruitItem : MonoBehaviour
{
    public int points = 10; // Cuántos puntos da esta fruta

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.AddScore(points); // Añadir puntos al jugador
            Destroy(gameObject);     // Destruye la fruta
        }
    }
}
