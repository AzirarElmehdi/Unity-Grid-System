using UnityEngine;

namespace Azirar.GridSystem
{
    /// <summary>
    /// Représente une cellule individuelle de la grille.
    /// Stocke les données de navigation et les coordonnées spatiales.
    /// </summary>
    public class Node
    {
        // Utilisation des propriétés (getters) pour les données qui ne changent pas.
        public bool walkable { get; private set; } 
        public Vector3 worldPosition { get; private set; }
        public int gridX { get; private set; }
        public int gridZ { get; private set; }



        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridZ)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridZ = gridZ;
        }
    }
}