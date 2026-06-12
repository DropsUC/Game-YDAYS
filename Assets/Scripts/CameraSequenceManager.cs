using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CameraZone
{
    public string name;
    public float transitionX; // La ligne à franchir pour CETTE zone
    public Vector3 targetPos; // Où la caméra doit aller
}

public class CameraSequenceManager : MonoBehaviour
{
    [Header("Personnages")]
    public Transform player1;
    public Transform player2;

    [Header("Réglages")]
    public float smoothSpeed = 5f;

    [Header("Zones de Caméra")]
    public List<CameraZone> zones = new List<CameraZone>();

    private Vector3 initialCameraPos;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            initialCameraPos = mainCamera.transform.position;
        }
        else
        {
            Debug.LogError("CameraSequenceManager : Aucune Main Camera trouvée !");
        }
    }

    void LateUpdate()
    {
        if (mainCamera == null || player1 == null || player2 == null) return;

        // On part de la position initiale (Zone 0)
        Vector3 target = initialCameraPos;

        // On parcourt les zones. 
        // Si les deux joueurs ont franchi la transition d'une zone, cette zone devient la cible.
        // On suppose que les zones sont rangées par ordre croissant de transitionX.
        foreach (var zone in zones)
        {
            if (player1.position.x > zone.transitionX && player2.position.x > zone.transitionX)
            {
                target = new Vector3(zone.targetPos.x, zone.targetPos.y, initialCameraPos.z);
            }
        }

        // Mouvement fluide
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target, Time.deltaTime * smoothSpeed);
    }

    private void OnDrawGizmos()
    {
        if (zones == null) return;

        foreach (var zone in zones)
        {
            // Dessine la ligne de transition
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(zone.transitionX, -10, 0), new Vector3(zone.transitionX, 10, 0));
            
            // Dessine une icône à la destination cible
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(zone.targetPos, new Vector3(1, 1, 0));
        }
    }
}
