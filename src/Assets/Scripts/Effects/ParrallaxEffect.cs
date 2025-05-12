using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier = 0.5f;
    [SerializeField] private bool canRepeat = true;
    [SerializeField] private bool useCustomFirstReposition = false;
    [SerializeField] private float customRepositionX = 0f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            spriteWidth = sr.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        float deltaX = cameraTransform.position.x - previousCameraPosition.x;
        transform.position += new Vector3(deltaX * parallaxMultiplier, 0, 0);
        previousCameraPosition = cameraTransform.position;

        if (!canRepeat) return;

        float cameraLeftEdge = cameraTransform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

        if (transform.position.x + spriteWidth / 2 < cameraLeftEdge)
        {
            if (useCustomFirstReposition)
            {
                // Posiciona en el eje X especificado solo una vez
                transform.position = new Vector3(customRepositionX, transform.position.y, transform.position.z);

                // Desactiva el booleano para que la siguiente reposición sea normal
                useCustomFirstReposition = false;

                // Actualiza la posición previa de la cámara
                previousCameraPosition = cameraTransform.position;
            }
            else
            {
                // Posición estándar al borde derecho
                float cameraRightEdge = cameraTransform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
                transform.position = new Vector3(cameraRightEdge + spriteWidth / 2, transform.position.y, transform.position.z);
            }
        }
    }
}
