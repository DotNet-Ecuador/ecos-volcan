using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject BulletPrefab;
    public float speed = 2.0f; // Velocidad de movimiento del enemigo
    public float followDistance = 1.5f; // Distancia mínima para detenerse

    private float lastShootTime;
    private float shootCooldown = 1.5f; // Tiempo entre disparos en segundos
    private int Health = 3;

    void Update()
    {
        if (Player == null) return;

        Vector3 direction = Player.transform.position - transform.position;

        // Ajusta la escala para mirar hacia el jugador
        Vector3 newScale = transform.localScale;
        newScale.x = (direction.x >= 0.0f) ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);

        // Moverse hacia el jugador si está lejos
        if (distance > followDistance)
        {
            transform.position += new Vector3(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0, 0);
        }

        // Disparar si está cerca y ha pasado el cooldown
        if (distance < 5.0f && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Shoot()
    {
        float shootDirection = Mathf.Sign(transform.localScale.x);
        Vector3 direction = new Vector3(shootDirection, 0, 0);

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);

        // Establece la dirección y marca como bala enemiga
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.SetDirection(direction);
        bulletScript.isEnemyBullet = true; // ← Aquí lo marcamos como bala enemiga
    }

    public void Hit()
    {
        Health -= 1;
        if (Health <= 0) Destroy(gameObject);
    }
}
