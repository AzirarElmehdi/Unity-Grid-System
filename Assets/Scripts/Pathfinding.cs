using UnityEngine;
using System.Collections.Generic;
using Azirar.GridSystem;

public class Pathfinding : MonoBehaviour
{
    private GridManager _gridManager;

    void Awake()
    {
        _gridManager = GetComponent<GridManager>();
    }

    /// <summary>
    /// Calcule le chemin le plus court via A*. Retourne null si la cible est inatteignable.
    /// </summary>
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Mapping des coordonnées mondiales vers l'indexation matricielle de la grille
        int startX = Mathf.RoundToInt(startPos.x);
        int startZ = Mathf.RoundToInt(startPos.z);
        int targetX = Mathf.RoundToInt(targetPos.x);
        int targetZ = Mathf.RoundToInt(targetPos.z);

        Node startNode = _gridManager.GetNodeFromGrid(startX, startZ);
        Node targetNode = _gridManager.GetNodeFromGrid(targetX, targetZ);

        // Early exit : évite de processer si la destination est hors bounds ou bloquée
        if (startNode == null || targetNode == null || !targetNode.walkable) 
        {
            return null;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Cible atteinte, on déclenche le backtracking
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            // Évaluation du coût G pour chaque voisin praticable
            foreach (Node neighbor in _gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // Mise à jour si le nouveau chemin est plus court ou si le voisin est non découvert
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Backtracking depuis la cible pour reconstruire le tracé complet.
    /// </summary>
    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        // La liste est générée de l'arrivée vers le départ, on la flip pour la navigation du PNJ
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Calcul heuristique (Octile distance) pour l'évaluation des coûts.
    /// </summary>
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        // 14 représente la diagonale (sqrt(2) * 10) et 10 les axes cardinaux pour éviter le cast en float
        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }
}