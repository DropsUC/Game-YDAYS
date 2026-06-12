using UnityEngine;
using TMPro; // Obligatoire pour utiliser TextMeshPro

public class DialogueManager : MonoBehaviour
{
    [Header("Elements de l'UI")]
    public GameObject dialoguePanel; // Le fond (Panel)
    public TextMeshProUGUI dialogueText; // Le composant texte

    // Cette ligne permet d'y accéder de n'importe où (Singleton)
    public static DialogueManager instance;

    private void Awake()
    {
        // On s'assure qu'il n'y a qu'un seul Manager
        if (instance == null)
        {
            instance = this;
        }
    }

    // Fonction pour afficher le texte
    public void ShowDialogue(string message)
    {
        dialoguePanel.SetActive(true); // On active le visuel
        dialogueText.text = message;   // On remplace le texte
    }

    // Fonction pour cacher le texte
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false); // On désactive le visuel
    }
}