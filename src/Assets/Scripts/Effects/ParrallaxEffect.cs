using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

using System.Collections.Generic;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier = 0.5f; // Velocidad del parallax
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        // Obtener el ancho del sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWidth = spriteRenderer.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        // Mueve el fondo en función del movimiento de la cámara
        float deltaX = cameraTransform.position.x - previousCameraPosition.x;
        transform.position += new Vector3(deltaX * parallaxMultiplier, 0, 0);
        previousCameraPosition = cameraTransform.position;

        // Calcula el borde izquierdo de la cámara
        float cameraLeftEdge = cameraTransform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

        // Si el fondo se sale completamente por la izquierda, lo movemos a la derecha
        if (transform.position.x + spriteWidth / 2 < cameraLeftEdge)
        {
            float cameraRightEdge = cameraTransform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
            transform.position = new Vector3(cameraRightEdge + spriteWidth / 2, transform.position.y, transform.position.z);
        }
    }
}




