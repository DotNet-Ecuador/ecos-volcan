using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ParrallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    private Transform cameraTransForm;
    private Vector3 previousCameraPosition;
    private float spriteWidth, startPosition;

    void Start()
    {
        cameraTransForm = Camera.main.transform;
        previousCameraPosition = cameraTransForm.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void LateUpdate()
    {
        float deltaX = (cameraTransForm.position.x - previousCameraPosition.x) *parallaxMultiplier;
        float moveAmount = cameraTransForm.position.x * (1 - parallaxMultiplier);
        transform.Translate(new Vector3(deltaX, 0, 0));
        previousCameraPosition = cameraTransForm.position;

        if (moveAmount >  startPosition + spriteWidth)
        {
            transform.Translate(new Vector3(spriteWidth, 0, 0));
            startPosition += spriteWidth;
        }
        else if (moveAmount < startPosition - spriteWidth)
        {
            transform.Translate(new Vector3(-spriteWidth, 0, 0));
            startPosition -= spriteWidth;
        }
    }
}
