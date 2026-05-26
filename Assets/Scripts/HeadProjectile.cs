using UnityEngine;

public class HeadProjectile : MonoBehaviour
{
    [Header("Paramètres")]
    public float rotationSpeed = 500f;
    public float lifetime = 5f;
    
    private void Start()
    {
        // La tête se détruit automatiquement après un certain temps
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Fait tourner la tête sur elle-même pendant le vol
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // On pourrait ajouter ici des dégâts aux ennemis
        // Debug.Log("La tête a touché : " + collision.gameObject.name);
    }
}
