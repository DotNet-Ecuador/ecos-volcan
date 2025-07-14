using UnityEngine;

public class RecogerVida : MonoBehaviour
{
    [Header("Referencia manual al objeto con UIDamageDisplay")]
    public UIDamageDisplay damageDisplay; // ← arrastra esto en el Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageDisplay != null)
        {
            damageDisplay.RestaurarUnaVida(); // método que ya añadimos antes
            Destroy(gameObject);
        }
    }
}
