using UnityEngine;

public class DropAlMorir : MonoBehaviour
{
    [Header("Prefab que se instanciará al morir")]
    public GameObject objetoADropearse;

    [Header("Posición del drop")]
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);

    void OnDestroy()
    {
        if (objetoADropearse != null)
        {
            Instantiate(objetoADropearse, transform.position + offset, Quaternion.identity);
        }
    }
    
}
