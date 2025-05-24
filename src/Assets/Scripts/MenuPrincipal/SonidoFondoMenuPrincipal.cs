using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSound : MonoBehaviour
{
  public AudioSource audioSource;

  void Start()
  {
    PlaySound();
  }

  public void PlaySound()
  {
    // Play the sound effect
    audioSource.Play();
  }
}
