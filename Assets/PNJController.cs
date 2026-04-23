using UnityEngine;
using Azirar.GridSystem; 

public class PNJController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private float _moveSpeed = 5f;

    private Vector3 _targetPosition; 
    private bool _isMoving = false;
    
    private Vector2Int _lastGridPos = new Vector2Int(-1, -1);

    void Start()
    {
        _targetPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
        UpdateGridLocation();
    }

    private void HandleMovement()
    {
        if (!_isMoving) return;

        // Calcul du déplacement fluide indexé sur le temps (Time.deltaTime)
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        // Vérification de l'arrivée avec une marge d'erreur (Epsilon)
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            _isMoving = false;
        }
    }

    private void UpdateGridLocation()
    {
        Vector2Int currentPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );

        // On ne log que si le PNJ a VRAIMENT changé de case
        if (currentPos != _lastGridPos)
        {
            _lastGridPos = currentPos;
            Debug.Log($"PNJ localisé sur la cellule : {currentPos.x}, {currentPos.y}");
        }
    }

    public void MoveToCell(int x, int z)
    {
        if (_gridManager.IsNodeWalkable(x, z))
        {
            _targetPosition = new Vector3(x, 1f, z);
            _isMoving = true;
        }
        else
        {
            Debug.LogWarning($"[Grid] Cellule {x},{z} non praticable (Obstacle).");
        }
    }
}