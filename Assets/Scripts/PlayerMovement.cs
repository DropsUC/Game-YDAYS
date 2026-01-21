using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 400f;

    [Header("Saut")]
    public float jumpForce = 500f; // La force du saut
    public Transform groundCheck;  // Un point aux pieds du perso
    public LayerMask groundLayer;  // Ce qui est consid�r� comme du "sol"

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Vector3 velocity = Vector3.zero;
    private float moveDirection;
    private bool isGrounded; // Pour savoir si on touche le sol

    // Fonction appel�e quand on appuie sur Espace / Bouton Sud
    public void OnJump(InputValue value)
    {
        // On saute SEULEMENT si on a appuy� sur le bouton ET qu'on est au sol
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveDirection = inputVector.x;
    }

    void FixedUpdate()
    {
        // Au lieu d'un petit cercle, on cr�e une bo�te rectangulaire
        // Param�tres : (Position, Taille de la boite X/Y, Angle, Calque)
        // new Vector2(0.5f, 0.2f) veut dire : 0.5 unit� de large, 0.2 de haut.
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.2f), 0f, groundLayer);

        // Reste du code de mouvement...
        float horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
        MovePlayer(horizontalMovement);

        Flip(rb.linearVelocity.x);

        float characterVelocity = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.linearVelocity.y);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, .05f);
    }

    // Cette fonction dessine des aides visuelles dans l'�diteur (Gizmos)
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            // Dessine la m�me bo�te que celle utilis�e dans le code
            Gizmos.DrawWireCube(groundCheck.position, new Vector2(0.5f, 0.2f));
        }
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
        
}