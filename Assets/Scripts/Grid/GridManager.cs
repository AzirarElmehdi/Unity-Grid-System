using UnityEngine;
using System.Collections.Generic;

namespace Azirar.GridSystem 
{
    /// <summary>
    /// Gère la génération de la structure de données de navigation et le baking des obstacles.
    /// </summary>
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
            // On itère sur les deux axes pour mapper l'espace monde vers la matrice de données
            for(int x = 0; x < _width; x++)
            {
                for(int z = 0; z < _depth; z++)
                {
                    Vector3 worldPos = new Vector3(x, 0, z);

                    // Utilisation de la physique pour définir l'état de franchissement statique au lancement
                    bool isBlocked = Physics.CheckSphere(worldPos, _checkRadius, _obstacleMask);
                    bool walkable = !isBlocked;

                    _grid[x, z] = new Node(walkable, worldPos, x, z);

                    CreateDebugCell(worldPos, isBlocked, x, z);
                }
            }
            Debug.Log($"[GridManager] Grille de {_width}x{_depth} générée avec succès.");
        }

        private void CreateDebugCell(Vector3 pos, bool isBlocked, int x, int z)
        {
            // Instanciation du retour visuel pour valider les zones de collision dans l'éditeur
            GameObject cell = Instantiate(_cellPrefab, pos, Quaternion.identity, transform);
            cell.name = $"Node_{x}_{z}"; 

            if(isBlocked)
            {
                cell.GetComponent<Renderer>().material.color = Color.black;
            }
        }

        /// <summary>
        /// Vérifie l'accessibilité d'un node avec une sécurité sur les limites de la grille.
        /// </summary>
        public bool IsNodeWalkable(int x, int z)
        {
            if (x >= 0 && x < _width && z >= 0 && z < _depth)
            {
                return _grid[x, z].walkable;
            }
            return false;
        }

        /// <summary>
        /// Récupère la référence du node. Retourne null si les coordonnées sont hors champ.
        /// </summary>
        public Node GetNodeFromGrid(int x, int z)
        {
            if (x >= 0 && x < _width && z >= 0 && z < _depth) return _grid[x, z];
            return null;
        }

        /// <summary>
        /// Retourne les 8 voisins directs et diagonaux pour l'exploration de l'algorithme.
        /// </summary>
        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            // Scan 3x3 autour du node avec exclusion de la cellule centrale
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;

                    int checkX = node.gridX + x;
                    int checkZ = node.gridZ + z;

                    // Validation des index pour éviter les erreurs OutOfBounds sur les bordures
                    if (checkX >= 0 && checkX < _width && checkZ >= 0 && checkZ < _depth)
                    {
                        neighbors.Add(_grid[checkX, checkZ]);
                    }
                }
            }

            return neighbors;
        }
    }
}