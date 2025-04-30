using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtenerMascara : MonoBehaviour
{
    [Header("Prefab a instanciar")]
    public GameObject prefab;
    public float eje_X = 1f;
    public float eje_Y = 0.5f;

    [Header("Distancia delante del jugador")]
    public float distancia = 1f;

    private bool tieneMascara = false;

    void Update()
    {
        if (tieneMascara && Input.GetKeyDown(KeyCode.Space))
        {
            InstanciarObjetoDelante();
        }
    }

    void InstanciarObjetoDelante()
    {
        // Determina dirección según la escala del jugador
        float direccion = transform.localScale.x > 0 ? 1f : -1f;

        // Cálculo de posición frente al jugador
        Vector3 offset = new Vector3(eje_X * direccion, -eje_Y, 0f);
        Vector3 posicion = transform.position + offset;

        // Instancia el prefab
        GameObject instancia = Instantiate(prefab, posicion, Quaternion.identity);

        // Asegura que el prefab mire en la misma dirección que el jugador
        Vector3 escalaPrefab = instancia.transform.localScale;
        escalaPrefab.x = Mathf.Abs(escalaPrefab.x) * direccion;
        instancia.transform.localScale = escalaPrefab;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mascara"))
        {
            tieneMascara = true;
            Destroy(other.gameObject);
        }
    }
}
