using UnityEngine;
using Azirar.GridSystem;

/// <summary>
/// Intercepte les entrées utilisateur et transmet les ordres de mouvement au PNJ.
/// Utilise le système de Raycasting pour traduire le clic 2D en coordonnées 3D.
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PNJController _pNJ;
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
        // Création du rayon depuis la position de la souris
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Conversion des coordonnées en entier.
            int gridX = Mathf.RoundToInt(hit.point.x);
            int gridZ = Mathf.RoundToInt(hit.point.z);

            // Transmission de l'ordre
            if (_pNJ != null)
            {
                _pNJ.MoveToCell(gridX, gridZ);
            }
        }
    }
}