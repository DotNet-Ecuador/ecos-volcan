using UnityEngine;

public class PuertaSalida : MonoBehaviour
{
    private bool abierta = false;

    public void AbrirPuerta()
    {
        if (!abierta)
        {
            abierta = true;
            gameObject.SetActive(false); // Simplemente la desactiva (puedes reemplazar con animación)
            Debug.Log("¡Puerta de salida abierta!");
        }
    }
}
