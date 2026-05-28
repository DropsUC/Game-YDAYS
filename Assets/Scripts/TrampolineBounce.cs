using UnityEngine;

public class TrampolineBounce : MonoBehaviour
{
    [Header("Bouncing Settings")]
    [Tooltip("Base force applied to the character jumping on this object.")]
    [SerializeField] private float bounceForce = 15f;

    [Tooltip("How much the incoming downward velocity multiplies the bounce force (0 = no scaling).")]
    [SerializeField] private float velocityScaling = 0.5f;

    [Tooltip("Minimum downward velocity required to trigger a bounce.")]
    [SerializeField] private float minImpactVelocity = 0.5f;

    [Header("Detection Settings")]
    [Tooltip("The tag of the character that can use this as a trampoline.")]
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Check if the colliding object is tagged as a Player
        if (collision.gameObject.CompareTag(playerTag))
        {
            // 2. Check if the collision is from above
            // contact.normal points from the other object (Player) towards this object (Trampoline).
            // If the player is on top, the normal will point downwards (0, -1).
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.7f) // Downward normal (Player hitting from top)
                {
                    Rigidbody2D jumperRb = collision.gameObject.GetComponent<Rigidbody2D>();
if (jumperRb != null)
                    {
                        // 3. Check downward movement (relative velocity)
                        // relativeVelocity is (other.velocity - our.velocity).
                        // If player is falling onto us, its relative velocity Y will be negative.
                        float downwardVelocity = -collision.relativeVelocity.y;

                        if (downwardVelocity > minImpactVelocity)
                        {
                            ApplyBounce(jumperRb, downwardVelocity);
                            return; // Trigger once per collision
                        }
                    }
                }
            }
        }
    }

    private void ApplyBounce(Rigidbody2D jumperRb, float impactVelocity)
    {
        // Calculate total force: base + (velocity * scaling)
        float totalForce = bounceForce + (impactVelocity * velocityScaling);

        // Reset vertical velocity to 0 before applying impulse for consistent behavior
        jumperRb.linearVelocity = new Vector2(jumperRb.linearVelocity.x, 0);

        // Apply upward impulse
        jumperRb.AddForce(Vector2.up * totalForce, ForceMode2D.Impulse);
        
        // Debug feedback
        Debug.Log($"[Trampoline] Bounce triggered on {jumperRb.name}! Impact speed: {impactVelocity:F1}, Force applied: {totalForce:F1}");
    }
}
