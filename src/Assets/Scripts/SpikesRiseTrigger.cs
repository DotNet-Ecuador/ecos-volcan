using UnityEngine;
using System.Collections;

public class SpikesRiseTrigger : MonoBehaviour
{
    public GameObject spikes;
    public float riseAmount = 1f;
    public float riseSpeed = 2f;
    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            StartCoroutine(RiseSpikes());
        }
    }

    private IEnumerator RiseSpikes()
    {
        Vector3 startPos = spikes.transform.position;
        Vector3 endPos = startPos + Vector3.up * riseAmount;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            spikes.transform.position = Vector3.Lerp(startPos, endPos, elapsed);
            elapsed += Time.deltaTime * riseSpeed;
            yield return null;
        }

        spikes.transform.position = endPos;
    }
}
