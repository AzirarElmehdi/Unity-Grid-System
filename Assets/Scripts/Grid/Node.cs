using UnityEngine;

namespace Azirar.GridSystem
{
    /// <summary>
    /// Représente une cellule individuelle de la grille.
    /// Stocke les données de navigation et les coordonnées spatiales.
    /// </summary>
    public class Node
    {
        // Encapsulation pour protéger les données structurelles une fois la grille générée
        public bool walkable { get; private set; } 
        public Vector3 worldPosition { get; private set; }
        public int gridX { get; private set; }
        public int gridZ { get; private set; }

        // Data-holder pour les états itératifs de l'algorithme A*
        public int gCost; 
        public int hCost; 
        public Node parent; 

        // Calcul dynamique du coût total pour l'évaluation de priorité dans la liste ouverte
        public int fCost 
        { 
            get { return gCost + hCost; } 
        }

        /// <summary>
        /// Initialise les paramètres immuables de la cellule lors de la création de la grille.
        /// </summary>
        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridZ)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridZ = gridZ;
        }
    }
}