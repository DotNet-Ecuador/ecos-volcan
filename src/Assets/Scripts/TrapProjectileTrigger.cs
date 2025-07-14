using UnityEngine;

public class TrapProjectileTrigger : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public Vector2 shootDirection = Vector2.left;
    public float projectileSpeed = 6f;

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;

            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection.normalized * projectileSpeed;
            }
        }
    }
}
