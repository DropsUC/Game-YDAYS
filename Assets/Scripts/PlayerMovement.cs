using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // --- NOUVEAU : SYSTÈME D'ÉTATS ---
    public enum HeadState { Attachee, EnMain, EnVol }

    [Header("Mouvement")]
    public float moveSpeed = 8f; // Ajusté suite à tes tests de vitesse

    [Header("Saut")]
    public float jumpForce = 500f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Lancer de tête")]
    public GameObject teteProjectilePrefab;
    public Transform throwPoint;
    public float throwForce = 15f;
    [Header("État de la tête")]
    public HeadState etatTete = HeadState.Attachee; // État de départ

    private GameObject teteActuelle;

    [Header("Composants")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;
    private float moveDirection;
    private bool isGrounded;

    // --- INPUTS ---

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveDirection = inputVector.x;
    }

    public void OnThrow(InputValue value)
    {
        if (value.isPressed)
        {
            switch (etatTete)
            {
                case HeadState.Attachee:
                    // ÉTAPE 1 : On passe de "Sur les épaules" à "Dans la main"
                    etatTete = HeadState.EnMain;
                    if (animator != null) animator.SetTrigger("PrendreTete");
                    break;

                case HeadState.EnMain:
                    // ÉTAPE 2 : On lance l'animation de lancer. 
                    // C'est l'Animation Event "ExecuteThrow" qui fera le travail physique.
                    if (animator != null) animator.SetTrigger("LancerTete");
                    break;

                case HeadState.EnVol:
                    // ÉTAPE 3 : La tête vole déjà, on la détruit pour qu'elle repousse
                    if (teteActuelle != null)
                    {
                        Destroy(teteActuelle);
                        teteActuelle = null;
                    }
                    etatTete = HeadState.Attachee;
                    if (animator != null) animator.SetTrigger("TeteRepousse");
                    break;
            }
        }
    }

    // --- LOGIQUE PHYSIQUE ---

    // Cette fonction sera appelée par ton Animation Event sur la frame du lancer
    public void ExecuteThrow()
    {
        // On change l'état logiquement
        etatTete = HeadState.EnVol;

        // Création du projectile
        teteActuelle = Instantiate(teteProjectilePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rbTete = teteActuelle.GetComponent<Rigidbody2D>();

        if (rbTete != null)
        {
            Vector2 aimDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            // Calcul de la direction vers la souris si elle existe
            if (Mouse.current != null)
            {
                Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
                aimDirection = ((Vector2)mouseWorldPosition - (Vector2)throwPoint.position).normalized;
            }

            // Propulsion
            rbTete.linearVelocity = aimDirection * throwForce;
        }
}

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.2f), 0f, groundLayer);

        float horizontalMovement = moveDirection * moveSpeed;
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

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f) spriteRenderer.flipX = false;
        else if (_velocity < -0.1f) spriteRenderer.flipX = true;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, new Vector2(0.3f, 0.2f));
        }
    }
}