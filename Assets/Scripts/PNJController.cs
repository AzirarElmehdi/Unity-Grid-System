using UnityEngine;
using System.Collections.Generic;
using Azirar.GridSystem; 

public class PNJController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Pathfinding _pathfinding;
    [SerializeField] private float _moveSpeed = 5f;

    private List<Node> _currentPath;
    private int _currentPathIndex;
    private bool _isMoving = false;
    
    private Vector2Int _lastGridPos;
    private bool _hasLoggedPosition = false;

    void Update()
    {
        HandleMovement();
        UpdateGridLocation();
    }

    private void HandleMovement()
    {
        if (!_isMoving || _currentPath == null || _currentPathIndex >= _currentPath.Count) return;

        Node targetNode = _currentPath[_currentPathIndex];
        Vector3 targetPosition = new Vector3(targetNode.gridX, 1f, targetNode.gridZ);

        // Déplacement linéaire simple vers le waypoint actuel
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

        // Seuil d'Epsilon pour valider l'arrivée sur le node et passer au suivant
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            _currentPathIndex++;

            if (_currentPathIndex >= _currentPath.Count)
            {
                _isMoving = false;
                _currentPath = null;
            }
        }
    }

    private void UpdateGridLocation()
    {
        // Conversion du world space en coordonnées de grille via arrondi
        Vector2Int currentPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );

        // Dirty flag pour éviter le spam console si le PNJ n'a pas changé de cellule
        if (!_hasLoggedPosition || currentPos != _lastGridPos)
        {
            _lastGridPos = currentPos;
            _hasLoggedPosition = true;
            Debug.Log($"PNJ localisé sur la cellule : {currentPos.x}, {currentPos.y}");
        }
    }

    /// <summary>
    /// Initialise une requête de chemin A* et lance la séquence de suivi de waypoints.
    /// </summary>
    public void MoveToCell(int x, int z)
    {
        Vector3 targetWorldPos = new Vector3(x, 1f, z);

        _currentPath = _pathfinding.FindPath(transform.position, targetWorldPos);

        // On ne trigger le flag de mouvement que si le pathfinding renvoie une route valide
        if (_currentPath != null && _currentPath.Count > 0)
        {
            _currentPathIndex = 0;
            _isMoving = true;
        }
        else
        {
            Debug.LogWarning($"[PNJ] Impossible de trouver un chemin vers {x},{z} !");
        }
    }
}