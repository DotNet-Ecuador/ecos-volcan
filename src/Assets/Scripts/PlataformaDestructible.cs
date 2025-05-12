using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDestructible : MonoBehaviour
{
    public float delayBeforeFall = 0.2f;
    private bool triggered = false;
    private Collider2D col2D;

     void Start()
    {
        col2D = GetComponent<Collider2D>();
    }
     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered && collision.collider.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(StartFall());
        }
    }

    private IEnumerator StartFall()
    {
        yield return new WaitForSeconds(delayBeforeFall);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        if (col2D != null)
            {
                col2D.isTrigger = true;
            }
    }
}
