using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float speed = 2.0f; // Velocidad con la que la cámara se mueve
    private float minX; // Límite mínimo de la cámara (para evitar que retroceda)

    void Start()
    {
        if (player != null)
        {
            minX = transform.position.x; // La posición inicial de la cámara es el límite mínimo
        }
    }

    void LateUpdate()
    {
        if (player == null) return; // Si no hay jugador, salir

        // Calcular la posición objetivo en X
        float targetX = Mathf.Max(minX, player.position.x);

        // Llamar a un Vector3 con la posición deseada, pero redondeando los valores a números enteros
        Vector3 targetPosition = new Vector3(Mathf.Round(targetX), transform.position.y, transform.position.z);

        // Mover la cámara de manera suave con la velocidad definida, sin decimales
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

        // Actualizar el límite mínimo
        minX = transform.position.x;
    }
}

