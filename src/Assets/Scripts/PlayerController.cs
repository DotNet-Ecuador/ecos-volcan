using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform spriteTransform; // arrastra el hijo "Sprite" aquí
    private Vector3 originalScale;

    void Start()
    {
        if (spriteTransform != null)
            originalScale = spriteTransform.localScale;
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            // Mira a la derecha (escala original)
            spriteTransform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < 0)
        {
            // Mira a la izquierda (escala negativa en X)
            spriteTransform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }
}
