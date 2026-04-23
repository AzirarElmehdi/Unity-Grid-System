using UnityEngine;

namespace Azirar.GridSystem 
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Size")]
        [SerializeField] private int _width = 10;
        [SerializeField] private int _depth = 10;

        [Header("Visuals")]
        [SerializeField] private GameObject _cellPrefab;

        [Header("Scanning Settings")]
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _checkRadius = 0.4f;

        private Node[,] _grid;

        void Start()
        {
            _grid = new Node[_width, _depth];
            GenerateNavigationGrid(); 
        }

        private void GenerateNavigationGrid()
        {
            for(int x = 0; x < _width; x++)
            {
                for(int z = 0; z < _depth; z++)
                {
                    Vector3 worldPos = new Vector3(x, 0, z);

                    // Détection des obstacles
                    bool isBlocked = Physics.CheckSphere(worldPos, _checkRadius, _obstacleMask);
                    bool walkable = !isBlocked;

                    // Création du Node 
                    _grid[x, z] = new Node(walkable, worldPos, x, z);

                    // Création des cells.
                    CreateDebugCell(worldPos, isBlocked, x, z);
                }
            }
            Debug.Log($"[GridManager] Grille de {_width}x{_depth} générée avec succès.");
        }

        private void CreateDebugCell(Vector3 pos, bool isBlocked, int x, int z)
        {
            GameObject cell = Instantiate(_cellPrefab, pos, Quaternion.identity, transform);
            cell.name = $"Node_{x}_{z}"; 

            if(isBlocked)
            {
                cell.GetComponent<Renderer>().material.color = Color.black;
            }
        }

    

        public bool IsNodeWalkable(int x, int z)
        {
            if (x >= 0 && x < _width && z >= 0 && z < _depth)
            {
                return _grid[x, z].walkable;
            }
            return false;
        }

        public Node GetNodeFromGrid(int x, int z)
        {
            if (x >= 0 && x < _width && z >= 0 && z < _depth) return _grid[x, z];
            return null;
        }
    }
}