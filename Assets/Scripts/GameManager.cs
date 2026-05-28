using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // Optional: Keep the GameManager across scene loads if you have global data
        // For this specific request, reloading the scene will reset the state anyway.
        // DontDestroyOnLoad(gameObject); 
    }

    /// <summary>
    /// Triggered when a player dies or falls into the void.
    /// Reloads the current scene to reset the level for everyone.
    /// </summary>
    public void OnPlayerDeath()
    {
        ReloadLevel();
    }

    private void ReloadLevel()
    {
        // Get the index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Reload it
        SceneManager.LoadScene(currentSceneIndex);
    }
}
