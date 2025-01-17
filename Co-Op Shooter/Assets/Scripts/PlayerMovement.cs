using System;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public static event EventHandler OnAnyPlayerSpawned;

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
    }

    public static PlayerMovement LocalInstance;

    [Header("Components")]
    Rigidbody2D rb;

    [Header("Ground")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Vector2 groundCheckSize = new(0.8f, 0.1f);
    bool isGrounded = false;

    /*
    [SerializeField] Transform leftWallCheckTransform;
    [SerializeField] Transform rightWallCheckTransform;
    [SerializeField] Vector2 wallCheckSize = new(0.1f, 0.7f);
    bool isTouchingLeftWall = false;
    bool isTouchingRightWall = false;
    */

    [Header("Movement")]
    float xMoveInput;
    [SerializeField] float moveSpeed = 10f;

    //Start
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        LocalInstance = this;

        OnAnyPlayerSpawned.Invoke(this, EventArgs.Empty);

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        GetPlayerInput();
        GroundCheck();
    }

    void GetPlayerInput()
    {
        xMoveInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(xMoveInput * moveSpeed * Time.deltaTime, Mathf.Clamp(rb.linearVelocity.y, -rb.gravityScale, float.MaxValue));
        transform.Translate(new Vector2(xMoveInput * moveSpeed * Time.deltaTime, 0));
    }

    void GroundCheck()
    {

        isGrounded = Physics2D.BoxCast(groundCheckTransform.position, groundCheckSize, 0f, Vector2.down, 0f, groundLayer);

        /*
        isTouchingLeftWall = Physics2D.BoxCast(leftWallCheckTransform.position, wallCheckSize, 0f, Vector2.down, 0f, groundLayer);
        isTouchingRightWall = Physics2D.BoxCast(rightWallCheckTransform.position, wallCheckSize, 0f, Vector2.down, 0f, groundLayer);
        */
    }

    
}
