using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaCambioEscena : MonoBehaviour
{
    public int indiceDeLaEscena = 1; // Puedes asignarlo desde el Inspector
    private bool jugadorDentro = false;
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.puedeSaltar = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            if (playerMovement != null)
                playerMovement.puedeSaltar = true;
        }
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(indiceDeLaEscena);
        }
    }
}
