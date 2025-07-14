using UnityEngine;

public class Pinchos : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
            if (pr != null)
            {
                pr.TakeDamageFromSpikes(); // ✅ LLAMADA CORRECTA
            } 
               // Llamada a Hit del script PlayerMovement
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.Hit(); // ✅ LLAMADA A Hit()
            }
        }
    }
}
