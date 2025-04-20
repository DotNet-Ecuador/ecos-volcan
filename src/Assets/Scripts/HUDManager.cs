using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text ammoText;       // Texto de la munición
    public Text scoreText;      // Texto del puntaje
    private PlayerMovement player;  // Referencia al jugador
    public Image progressBar;   // Image tipo Filled (Left to Right)
    private float maxProgress = 100f;
    private float currentProgress = 0f;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();  // Buscar al jugador en la escena
    }

    void Update()
    {
        if (player != null)
        {
            ammoText.text = player.ammo.ToString();
            scoreText.text = "Score: " + player.score;
        }
    }

    public void AddProgress(float damageAmount)
    {
        currentProgress += damageAmount;
        currentProgress = Mathf.Clamp(currentProgress, 0, maxProgress);
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = currentProgress / maxProgress;
        }
    }

    public void UpdateHUD()
    {
        if (player != null)
        {
            ammoText.text = player.ammo.ToString();
            scoreText.text = "Score: " + player.score;
        }
    }
}
