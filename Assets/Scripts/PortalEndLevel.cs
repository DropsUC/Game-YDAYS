using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PortalEndLevel : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup fadeGroup;
    public TextMeshProUGUI endText;

    [Header("Settings")]
    public float fadeDuration = 2f;
    
    private int playersInPortal = 0;
    private bool isEnding = false;

    private void Start()
    {
        if (fadeGroup != null) fadeGroup.alpha = 0f;
        if (endText != null) 
        {
            Color c = endText.color;
            c.a = 0f;
            endText.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInPortal++;
            CheckEndLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInPortal--;
        }
    }

    private void CheckEndLevel()
    {
        if (playersInPortal >= 2 && !isEnding)
        {
            isEnding = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    private IEnumerator EndLevelSequence()
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            
            if (fadeGroup != null) fadeGroup.alpha = alpha;
            if (endText != null) 
            {
                Color c = endText.color;
                c.a = alpha;
                endText.color = c;
            }
            yield return null;
        }

        if (fadeGroup != null) fadeGroup.alpha = 1f;
        if (endText != null) 
        {
            Color c = endText.color;
            c.a = 1f;
            endText.color = c;
        }

        Debug.Log("Fin du Tutorial !");
    }
}
