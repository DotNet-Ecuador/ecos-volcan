using UnityEngine;

public class AliadoVoladorFlotante : MonoBehaviour
{
    public Transform jugador;           // El jugador a seguir
    public Vector3 offset = new Vector3(1f, 1.5f, 0); // Posición relativa al jugador

    public float radioFlotacion = 0.3f; // Radio del movimiento circular propio
    public float velocidadGiro = 2f;    // Qué tan rápido gira

    private float angulo = 0f;

    void Update()
    {
        if (jugador == null) return;

        // Posición base que sigue al jugador con un offset fijo
        Vector3 posicionBase = jugador.position + offset;

        // Movimiento circular en la posición base
        angulo += velocidadGiro * Time.deltaTime;
        if (angulo >= 360f) angulo -= 360f;

        float x = Mathf.Cos(angulo) * radioFlotacion;
        float y = Mathf.Sin(angulo) * radioFlotacion;

        Vector3 circulo = new Vector3(x, y, 0f);

        // Posición final con movimiento circular
        transform.position = posicionBase + circulo;
    }
}
