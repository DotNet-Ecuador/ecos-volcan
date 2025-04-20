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
}
