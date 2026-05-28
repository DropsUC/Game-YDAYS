using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GroundButton : MonoBehaviour
{
    [Header("Settings")]
    public float pressDepth = 0.1f;
    public float transitionSpeed = 10f;
    public Color pressedColor = Color.green;
    public Color releasedColor = Color.red;
    
    [Header("Events")]
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private Vector3 unpressedPos;
    private Vector3 pressedPos;
    private bool isPressed = false;
    private List<Collider2D> objectsOnButton = new List<Collider2D>();
    private SpriteRenderer sr;

    void Start()
    {
        unpressedPos = transform.position;
        pressedPos = unpressedPos + Vector3.down * pressDepth;
        sr = GetComponent<SpriteRenderer>();
        if(sr != null) sr.color = releasedColor;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, isPressed ? pressedPos : unpressedPos, Time.deltaTime * transitionSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPresser(other))
        {
            if (!objectsOnButton.Contains(other))
            {
                objectsOnButton.Add(other);
            }
            
            if (!isPressed)
            {
                isPressed = true;
                if(sr != null) sr.color = pressedColor;
                OnPressed.Invoke();
                Debug.Log("Bouton pressé par : " + other.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsOnButton.Contains(other))
        {
            objectsOnButton.Remove(other);
        }

        if (objectsOnButton.Count == 0 && isPressed)
        {
            isPressed = false;
            if(sr != null) sr.color = releasedColor;
            OnReleased.Invoke();
            Debug.Log("Bouton relâché");
        }
    }

    private bool IsPresser(Collider2D other)
    {
        // Détecte le joueur (squelette ou slime) ou la tête
        return other.GetComponent<PlayerMovement>() != null || 
               other.GetComponent<SlimeMovement>() != null || 
               other.GetComponent<HeadProjectile>() != null ||
               other.CompareTag("Player");
    }
}