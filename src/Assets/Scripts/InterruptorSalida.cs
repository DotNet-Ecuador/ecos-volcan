using UnityEngine;

public class InterruptorSalida : MonoBehaviour
{
    public PuertaSalida puertaSalida;

    private bool activado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activado && other.CompareTag("Player"))
        {
            activado = true;
            puertaSalida.AbrirPuerta();
            Debug.Log("Interruptor activado");

            // Aplastar el botón visualmente reduciendo solo su escala Y
            Vector3 nuevaEscala = transform.localScale;
            nuevaEscala.y = 0.297701f;
            transform.localScale = nuevaEscala;
        }
    }
}
