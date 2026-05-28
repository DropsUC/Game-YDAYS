using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeMovement : MonoBehaviour
{
    public float baseGravityScale = 3f;

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
    public Animator animator;

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
        isGrounded = CheckRaycast(groundCheck.position, Vector2.down, 0.3f);
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
            rb.gravityScale = baseGravityScale;
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            }

            // --- MISE À JOUR DE L'ANIMATION (Chaque frame) ---
            if (animator != null)
            {
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
            float verticalSpeed = Mathf.Abs(rb.linearVelocity.y);
            
            // Si on grimpe, on utilise la vitesse verticale ou l'input
            float currentMoveSpeed = isClimbing ? Mathf.Max(horizontalSpeed, verticalSpeed) : horizontalSpeed;
            
            animator.SetFloat("Speed", currentMoveSpeed);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsClimbing", isClimbing);
            }

            HandleVisualRotation();
            }

    void CheckWalls()
    {
        isOnWall = false;
        
        RaycastHit2D hitR = CheckRaycastHit(transform.position, Vector2.right, 0.6f);
        if (hitR.collider != null) 
        { 
            isOnWall = true; 
            wallNormal = hitR.normal; 
        }
        else 
        {
            RaycastHit2D hitL = CheckRaycastHit(transform.position, Vector2.left, 0.6f);
            if (hitL.collider != null) 
            { 
                isOnWall = true; 
                wallNormal = hitL.normal; 
            }
        }
    }

    // Fonction utilitaire pour ignorer le slime lui-même
    private bool CheckRaycast(Vector2 origin, Vector2 direction, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, groundLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject) return true;
        }
        return false;
    }

    private RaycastHit2D CheckRaycastHit(Vector2 origin, Vector2 direction, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, groundLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject) return hit;
        }
        return new RaycastHit2D();
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