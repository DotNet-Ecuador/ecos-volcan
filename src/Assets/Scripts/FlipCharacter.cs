using UnityEngine;

public class FlipCharacter : MonoBehaviour
{
    private float horizontalInput;  // Para almacenar el movimiento horizontal
    private Vector3 originalScale;  // Para almacenar la escala original del padre

    void Start()
    {
        // Al iniciar, guardamos la escala original del objeto
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Obtener el input horizontal (izquierda/derecha)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Si el movimiento es hacia la izquierda (horizontalInput < 0), voltea el padre
        if (horizontalInput < 0)
        {
            // Cambiar la escala del padre a -1 en el eje X
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        // Si el movimiento es hacia la derecha (horizontalInput > 0), vuelve a la escala original
        else if (horizontalInput > 0)
        {
            // Cambiar la escala del padre a 1 en el eje X
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }
}
