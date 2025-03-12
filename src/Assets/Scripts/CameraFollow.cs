using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que la cámara seguirá
    public Vector3 offset; // Desplazamiento de la cámara respecto al objetivo

    void LateUpdate()
    {
        // Verifica que el target no sea null
        if (target == null)
        {
            Debug.LogWarning("Target no asignado. La cámara no puede seguir a ningún objeto.");
            return;
        }

        // Calcular la nueva posición de la cámara
        Vector3 desiredPosition = target.position + offset;

        // Actualizar la posición de la cámara de inmediato
        transform.position = desiredPosition;
    }
}
