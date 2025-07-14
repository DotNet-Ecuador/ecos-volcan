using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    private bool isInvulnerable = false;
    public float invulnerableTime = 1.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        respawnPoint = transform.position; // Punto inicial
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateCheckpoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }

    public void TakeDamageFromSpikes()
    {
        if (isInvulnerable) return;

        StartCoroutine(RespawnAfterDamage());
    }

    private IEnumerator RespawnAfterDamage()
    {
        // Puedes agregar animación de daño aquí
        yield return new WaitForSeconds(0.1f); // Pequeño retardo

        transform.position = respawnPoint;

        // Flash para indicar invulnerabilidad
        float flashDuration = 0.1f;
        for (float i = 0; i < invulnerableTime; i += flashDuration * 2)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }

        isInvulnerable = false;
    }
}
