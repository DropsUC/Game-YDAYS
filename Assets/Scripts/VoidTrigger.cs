using UnityEngine;

public class VoidTrigger : MonoBehaviour
{
    // Tag assigned to your player GameObjects (default is "Player")
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is a player
        if (collision.CompareTag(playerTag))
        {
            // Call the GameManager to reset the level
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPlayerDeath();
            }
            else
            {
                Debug.LogWarning("GameManager instance not found in the scene!");
            }
        }
    }
}
