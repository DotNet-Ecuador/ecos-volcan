using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que la c�mara seguir�
    public Vector3 offset; // Desplazamiento de la c�mara respecto al objetivo

    void LateUpdate()
    {
        // Verifica que el target no sea null
        if (target == null)
        {
            Debug.LogWarning("Target no asignado. La c�mara no puede seguir a ning�n objeto.");
            return;
        }

        // Calcular la nueva posici�n de la c�mara
        Vector3 desiredPosition = target.position + offset;

        // Actualizar la posici�n de la c�mara de inmediato
        transform.position = desiredPosition;
    }
}
