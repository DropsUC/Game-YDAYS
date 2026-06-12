using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 4. Dans cette fonction, vérifie si l'objet qui entre en collision possède le tag 'Player'
        if (other.CompareTag("Player"))
        {
            // 5. Si c'est un joueur, recharge immédiatement la scène actuelle active
            Debug.Log("Player fell into the void! Restarting scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
