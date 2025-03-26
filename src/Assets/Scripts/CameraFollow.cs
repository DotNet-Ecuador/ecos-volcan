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

        float targetX = Mathf.Max(minX, player.position.x); // Asegura que la cámara solo se mueva hacia adelante
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z); // Mueve solo en X

        minX = transform.position.x; // Actualiza el límite mínimo
    }
}
