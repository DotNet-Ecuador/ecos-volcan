using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuegoMascara : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(3f); // ← aquí usamos 1f como float

            HUDManager hud = FindObjectOfType<HUDManager>();
            if (hud != null)
            {
                hud.AddProgress(10);
            }
        }
    }
}
