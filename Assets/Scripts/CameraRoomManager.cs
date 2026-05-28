using UnityEngine;

public class CameraRoomManager : MonoBehaviour
{
    [Header("Personnages")]
    public Transform player1;
    public Transform player2;

    [Header("Configuration Transition")]
    [Tooltip("La ligne X à franchir pour changer de zone")]
    public float transitionX;
    
    [Tooltip("La position de la caméra dans la deuxième zone")]
    public Vector3 cameraTargetPos;

    [Header("Réglages")]
    public float smoothSpeed = 5f;

    private Vector3 initialCameraPos;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // On enregistre la position de départ (Zone 1)
            initialCameraPos = mainCamera.transform.position;
        }
        else
        {
            Debug.LogError("CameraRoomManager : Aucune Main Camera trouvée !");
        }
    }

    void LateUpdate()
    {
        if (mainCamera == null || player1 == null || player2 == null) return;

        // Déterminer la destination
        Vector3 target;
        if (player1.position.x > transitionX && player2.position.x > transitionX)
        {
            // Si les deux sont à droite de la ligne, on vise la Zone 2
            target = new Vector3(cameraTargetPos.x, cameraTargetPos.y, initialCameraPos.z);
        }
        else
        {
            // Sinon, on reste/revient en Zone 1
            target = initialCameraPos;
        }

        // Mouvement fluide
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target, Time.deltaTime * smoothSpeed);
    }

    private void OnDrawGizmos()
    {
        // Dessine la ligne de transition dans l'Editor pour aider au réglage
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transitionX, -100, 0), new Vector3(transitionX, 100, 0));
        
        // Dessine une icône à la destination cible
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(cameraTargetPos, new Vector3(1, 1, 0));
    }
}
