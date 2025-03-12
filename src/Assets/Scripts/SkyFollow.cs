using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFollow : MonoBehaviour
{
    public Transform target; // Referencia al personaje
    public float parallaxEffectMultiplier = 0.5f; // Efecto de parallax en el cielo

    void Update()
    {
        // Calcular la nueva posici�n del cielo basada en la posici�n del personaje
        float newPosX = target.position.x * parallaxEffectMultiplier;

        // Actualizar la posici�n del cielo
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}
