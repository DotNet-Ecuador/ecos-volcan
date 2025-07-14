using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour
{
    [Tooltip("Centro exacto del cuarto (Empty GameObject)")]
    public Transform roomCenter;

    [Tooltip("Duración de la transición en segundos")]
    public float duracionTransicion = 1.2f;

    private CameraFollow cameraFollow;
    private Coroutine movimientoSuave;

    void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                cameraFollow = cam.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                    cameraFollow.enabled = false; // Detenemos seguimiento

                if (movimientoSuave != null)
                    StopCoroutine(movimientoSuave);

                // Iniciar transición suave hacia el centro del cuarto
                movimientoSuave = StartCoroutine(MoverCamaraSuavemente(cam.transform, roomCenter.position, duracionTransicion));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cameraFollow != null)
        {
            if (movimientoSuave != null)
                StopCoroutine(movimientoSuave);

            cameraFollow.enabled = true; // Reactiva seguimiento al jugador
        }
    }

    IEnumerator MoverCamaraSuavemente(Transform camara, Vector3 destino, float duracion)
    {
        Vector3 posicionInicial = camara.position;
        Vector3 destinoFinal = new Vector3(destino.x, destino.y, posicionInicial.z);
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            camara.position = Vector3.Lerp(posicionInicial, destinoFinal, t);
            yield return null;
        }

        camara.position = destinoFinal; // Asegura posición final exacta
    }
}
