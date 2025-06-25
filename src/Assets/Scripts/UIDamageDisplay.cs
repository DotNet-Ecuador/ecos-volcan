using System.Collections.Generic;
using UnityEngine;

public class UIDamageDisplay : MonoBehaviour
{
    public GameObject jugador;

    public GameObject vida1;
    public GameObject vida2;
    public GameObject vida3;
    public GameObject vida4;

    private List<GameObject> vidas = new List<GameObject>();

    void Start()
    {
        // Agrega las vidas a la lista en orden
        if (vida1 != null) vidas.Add(vida1);
        if (vida2 != null) vidas.Add(vida2);
        if (vida3 != null) vidas.Add(vida3);
        if (vida4 != null) vidas.Add(vida4);
    }

    public void QuitarVida()
    {
        if (vidas.Count > 0)
        {
            int index = vidas.Count - 1;

            // ⚠️ Aquí es donde realmente se oculta la imagen en pantalla
            vidas[index].SetActive(false);

            vidas.RemoveAt(index);
            Debug.Log("Vida visual eliminada.");
        }

        if (vidas.Count == 0 && jugador != null)
        {
            Debug.Log("Sin vidas visuales, jugador destruido.");
            Destroy(jugador);
        }
    }

    public void RestaurarVidas()
    {
        vidas.Clear();

        if (vida1 != null)
        {
            vida1.SetActive(true);
            vidas.Add(vida1);
        }

        if (vida2 != null)
        {
            vida2.SetActive(true);
            vidas.Add(vida2);
        }

        if (vida3 != null)
        {
            vida3.SetActive(true);
            vidas.Add(vida3);
        }

        if (vida4 != null)
        {
            vida4.SetActive(true);
            vidas.Add(vida4);
        }

        Debug.Log("Vidas visuales restauradas.");
    }
    public void RestaurarUnaVida()
{
    // Si ya hay 4 vidas, no se hace nada
    if (vidas.Count >= 4) return;

    // Determinar qué vida fue desactivada y reactivarla en orden
    if (vida4 != null && !vida4.activeSelf && !vidas.Contains(vida4))
    {
        vida4.SetActive(true);
        vidas.Add(vida4);
        Debug.Log("Vida 4 restaurada.");
    }
    else if (vida3 != null && !vida3.activeSelf && !vidas.Contains(vida3))
    {
        vida3.SetActive(true);
        vidas.Add(vida3);
        Debug.Log("Vida 3 restaurada.");
    }
    else if (vida2 != null && !vida2.activeSelf && !vidas.Contains(vida2))
    {
        vida2.SetActive(true);
        vidas.Add(vida2);
        Debug.Log("Vida 2 restaurada.");
    }
    else if (vida1 != null && !vida1.activeSelf && !vidas.Contains(vida1))
    {
        vida1.SetActive(true);
        vidas.Add(vida1);
        Debug.Log("Vida 1 restaurada.");
    }
}

}
