using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D rb;

    [Header("Ground")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Vector2 groundCheckSize = new(0.8f, 0.1f);
    bool isTouchingGround = false;

    [SerializeField] Transform leftWallCheckTransform;
    [SerializeField] Transform rightWallCheckTransform;
    [SerializeField] Vector2 wallCheckSize = new(0.1f, 0.7f);
    bool isTouchingLeftWall = false;
    bool isTouchingRightWall = false;

    [Header("Movement")]
    float xMoveInput = 0f;
    [SerializeField] float moveSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Movement();
    }


    void Movement()
    {
        xMoveInput = Input.GetAxis("Horizontal");
        Vector2 deltaPosition = new(xMoveInput * moveSpeed * Time.deltaTime, 0f);
        deltaPosition += new Vector2(0f, -rb.gravityScale * Time.deltaTime);

        isTouchingGround = Physics2D.BoxCast(groundCheckTransform.position, groundCheckSize, 0f, Vector2.down, 0f, groundLayer);

        isTouchingLeftWall = Physics2D.BoxCast(leftWallCheckTransform.position, wallCheckSize, 0f, Vector2.down, 0f, groundLayer);
        isTouchingRightWall = Physics2D.BoxCast(rightWallCheckTransform.position, wallCheckSize, 0f, Vector2.down, 0f, groundLayer);

        if (isTouchingGround)
        {
            deltaPosition.y = 0f;
        }
        if (isTouchingLeftWall)
        {
            deltaPosition.x = Mathf.Clamp(deltaPosition.x, 0f, float.MaxValue);
        }
        if (isTouchingRightWall)
        {
            deltaPosition.x = Mathf.Clamp(deltaPosition.x, float.MinValue, 0f);
        }

        rb.MovePosition(rb.position + deltaPosition);
    }

}
