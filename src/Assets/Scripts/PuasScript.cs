using UnityEngine;

public class MovimientoDePinchos : MonoBehaviour
{
    public float moveDistance = 1.5f;       // Cuánto suben/bajan
    public float moveSpeed = 2f;            // Velocidad del movimiento
    public float waitTime = 1f;             // Tiempo que esperan antes de moverse

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool goingUp = true;
    private float waitTimer = 0f;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(0, moveDistance, 0);
    }

    void Update()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        Vector3 target = goingUp ? targetPosition : initialPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            goingUp = !goingUp;
            waitTimer = waitTime;
        }
    }
}
