using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    private float moveInput;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isDashing)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Voltear sprite
            if (moveInput > 0) sr.flipX = false;
            else if (moveInput < 0) sr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float direction = sr.flipX ? -1 : 1;
        rb.velocity = new Vector2(direction * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

