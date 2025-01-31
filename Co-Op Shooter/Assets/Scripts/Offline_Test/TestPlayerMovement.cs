using System;
using UnityEngine;
using static UnityEngine.UI.Image;

public class TestPlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;
    private float xInput;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Vector2 groundCheckSize = Vector2.one;
    bool isGrounded = false;

    [SerializeField] KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField] float jumpForce;
    [SerializeField] float bufferTime;
    float bufferTimeCounter = 0f;

    [SerializeField] float regularGravity;
    [SerializeField] float fallingGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleGrounded();

        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        HandleGravity();
    }

    private void HandleGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(groundCheckTransform.position, groundCheckSize, 0f, Vector2.down, 0f, groundLayer);

        Debug.DrawLine(groundCheckTransform.position - new Vector3(groundCheckSize.x / 2, 0f), groundCheckTransform.position + new Vector3(groundCheckSize.x / 2, -groundCheckSize.y - 0f), Color.green);

        isGrounded = ray.collider != null;
    }

    private void HandleGravity()
    {
        if (rb.linearVelocityY < 0) {
            rb.gravityScale = fallingGravity;
        }
        else {
            rb.gravityScale = regularGravity;
        }

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -rb.gravityScale, float.MaxValue);
    }

    private void HandleJump()
    {
        bufferTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(jumpKeyCode)) {
            bufferTimeCounter = bufferTime;
        }

        if(isGrounded) { 
            if (bufferTimeCounter > 0f) {
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                bufferTimeCounter = 0f;
            }
        }
    }

    private void HandleMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        
        if (xInput == 0) { 
            rb.linearVelocityX = 0;
        }
        else { 
            rb.linearVelocityX = Mathf.Sign(xInput) * moveSpeed;
        }
    }
}

