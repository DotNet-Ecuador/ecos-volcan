using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image healthFill;  // Imagen de la barra de vida
    public Text ammoText;     // Texto de la munición
    public Text scoreText; // Texto del puntaje
    private PlayerMovement player;    // Referencia al jugador

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();  // Buscar al jugador en la escena
    }

    void Update()
    {
        if (player != null)
        {
            healthFill.fillAmount = player.health / player.maxHealth;
            ammoText.text = player.ammo.ToString();
            scoreText.text = "Score: " + player.score;
        }
    }

    public void UpdateHUD()
    {
        if (player != null)
        {
            healthFill.fillAmount = player.health / player.maxHealth;
            ammoText.text = player.ammo.ToString();
            scoreText.text = "Score: " + player.score;
        }
    }
}
