using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeMovement : MonoBehaviour
{
    [Header("Mouvement Escargot")]
    public float moveSpeed = 6f;
    public float climbSpeed = 4f;
    public float rotationSpeed = 15f;

    [Header("Saut")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Composants")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Transform visuelTransform;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isOnWall;
    private Vector2 wallNormal;
    private bool isClimbing;

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded)
            {
                // Saut normal depuis le sol
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (isClimbing)
            {
                // SAUT MURAL : Propulse vers le haut et s'écarte du mur
                rb.linearVelocity = new Vector2(wallNormal.x * jumpForce * 0.8f, jumpForce);
                isClimbing = false;
            }
        }
    }

    void FixedUpdate()
    {
        // 1. Détections
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.3f, groundLayer);
        isGrounded = groundHit.collider != null;
        CheckWalls();

        // 2. Logique de grimpe
        if (isOnWall && !isGrounded)
        {
            isClimbing = true;
            rb.gravityScale = 0;

            float vMove = 0;
            
            // Mur à GAUCHE (normale vers la droite)
            if (wallNormal.x > 0.1f)
            {
                if (moveInput.x < -0.1f) vMove = climbSpeed;      // Flèche Gauche -> Monter
                else if (moveInput.x > 0.1f) vMove = -climbSpeed; // Flèche Droite -> Descendre
            }
            // Mur à DROITE (normale vers la gauche)
            else if (wallNormal.x < -0.1f)
            {
                if (moveInput.x > 0.1f) vMove = climbSpeed;       // Flèche Droite -> Monter
                else if (moveInput.x < -0.1f) vMove = -climbSpeed; // Flèche Gauche -> Descendre
            }

            // Maintien au mur + mouvement vertical calculé
            rb.linearVelocity = new Vector2(-wallNormal.x * 2f, vMove);
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = 3f;
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }

        HandleVisualRotation();
    }

    void CheckWalls()
    {
        isOnWall = false;
        RaycastHit2D hitR = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);

        if (hitR.collider != null) { isOnWall = true; wallNormal = hitR.normal; }
        else if (hitL.collider != null) { isOnWall = true; wallNormal = hitL.normal; }
    }

    void HandleVisualRotation()
    {
        if (visuelTransform == null) return;

        Quaternion targetRot = Quaternion.identity;
        if (isClimbing)
        {
            float angle = Mathf.Atan2(wallNormal.y, wallNormal.x) * Mathf.Rad2Deg - 90f;
            targetRot = Quaternion.Euler(0, 0, angle);
        }

        visuelTransform.rotation = Quaternion.Lerp(visuelTransform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);

        if (!isClimbing)
        {
            if (rb.linearVelocity.x > 0.1f) spriteRenderer.flipX = false;
            else if (rb.linearVelocity.x < -0.1f) spriteRenderer.flipX = true;
        }
    }
}