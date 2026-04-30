using UnityEngine;
using Azirar.GridSystem;

/// <summary>
/// Traduit les clics de souris en coordonnées monde pour piloter le système de navigation.
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PNJController _pNJ;
    
    // On utilise une référence directe pour éviter l'appel coûteux à Camera.main à chaque frame
    [SerializeField] private Camera _mainCamera; 

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ExecuteClickToMove();
        }
    }

    private void ExecuteClickToMove()
    {
        // Projection du clic 2D dans l'espace 3D
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // Le Raycast nécessite un Collider sur la grille ou le sol pour intercepter le laser
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Conversion des flottants en index entiers [x, z] pour la matrice de la grille
            int gridX = Mathf.RoundToInt(hit.point.x);
            int gridZ = Mathf.RoundToInt(hit.point.z);

            // Protection contre les erreurs de référence si le controller n'est pas lié
            if (_pNJ != null)
            {
                _pNJ.MoveToCell(gridX, gridZ);
            }
        }
    }
}