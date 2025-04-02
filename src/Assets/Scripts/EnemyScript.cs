using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject BulletPrefab;

    private float lastShootTime;
    private float shootCooldown = 1.5f; // Tiempo entre disparos en segundos
    private int Health = 3;

    void Update()
    {
        if (Player == null) return; // Evita errores si el Player no está asignado

        Vector3 direction = Player.transform.position - transform.position;

        // Ajustar la dirección en la escala del enemigo sin cambiar el tamaño
        Vector3 newScale = transform.localScale;
        newScale.x = (direction.x >= 0.0f) ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);

        // Disparar solo si ha pasado el tiempo del cooldown
        if (distance < 5.0f && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time; // Actualiza el último disparo
        }
    }

    private void Shoot()
    {
        // La dirección del disparo ahora depende de la escala del enemigo
        float shootDirection = Mathf.Sign(transform.localScale.x);
        Vector3 direction = new Vector3(shootDirection, 0, 0);

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0) Destroy(gameObject);
    }
}
