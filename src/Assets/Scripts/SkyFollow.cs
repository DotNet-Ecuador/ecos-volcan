using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFollow : MonoBehaviour
{
    public Transform target; // Referencia al personaje
    public float parallaxEffectMultiplier = 0.5f; // Efecto de parallax en el cielo

    void Update()
    {
        // Calcular la nueva posición del cielo basada en la posición del personaje
        float newPosX = target.position.x * parallaxEffectMultiplier;

        // Actualizar la posición del cielo
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}
