using UnityEngine;

public class Sign : MonoBehaviour
{
    [Header("Lien de l'UI")]
    [Tooltip("Glisse ici l'objet Canvas qui est l'enfant de ce panneau")]
    public GameObject bulleUI;

    private void Start()
    {
        // On s'assure que la bulle est cachée au lancement du jeu
        if (bulleUI != null)
        {
            bulleUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le joueur entre = on allume la bulle
        if (collision.CompareTag("Player"))
        {
            bulleUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Le joueur sort = on éteint la bulle
        if (collision.CompareTag("Player"))
        {
            bulleUI.SetActive(false);
        }
    }
}