# Unity Grid System — A* Pathfinding

Implémentation manuelle de l'algorithme A* sur une grille générée procéduralement sous Unity 6 (LTS). Le projet couvre la génération de la grille, la détection d'obstacles par physique, le calcul de chemin optimal et le déplacement autonome d'un agent.

---

## Fonctionnement

Au lancement, le `GridManager` génère une grille de N×N cases. Chaque case est scannée via `Physics.CheckSphere` pour détecter les obstacles tagués sur le `LayerMask` dédié. Les cases bloquées sont colorées en noir visuellement.

Un clic gauche sur une case accessible déclenche le calcul A* depuis la position actuelle du PNJ jusqu'à la cible. Si la cible est une case noire (obstacle), un message d'avertissement est loggé en console et le déplacement est annulé. Sinon, la capsule se déplace case par case en suivant le chemin calculé.

---

## Algorithme A*

L'algorithme est implémenté from scratch sans librairie externe.

**Formule :** `F = G + H`

| Variable | Rôle |
|----------|------|
| G | Coût cumulé depuis le point de départ |
| H | Heuristique — estimation jusqu'à l'arrivée |
| F | Score total, A* choisit toujours le F le plus bas |

**Heuristique utilisée : Octile Distance**

Gère correctement les déplacements diagonaux avec les coefficients `14` (diagonal ≈ √2 × 10) et `10` (cardinal), ce qui évite tout cast en float dans la boucle principale.

```csharp
if (dstX > dstZ)
    return 14 * dstZ + 10 * (dstX - dstZ);
return 14 * dstX + 10 * (dstZ - dstX);
```

---

## Architecture

```
Assets/Scripts/
├── Grid/
│   ├── Node.cs          # Données d'une cellule (walkable, gCost, hCost, parent)
│   └── GridManager.cs   # Génération de la grille + détection obstacles + accès aux voisins
├── Pathfinding.cs        # Algorithme A* isolé
├── PNJController.cs      # Déplacement de l'agent case par case
└── InputManager.cs       # Raycast souris → coordonnées grille → ordre au PNJ
```

Chaque script a une responsabilité unique. Le namespace `Azirar.GridSystem` isole les classes de navigation du reste du projet.

---

## Stack

- **Unity 6 (LTS)**
- **C# / .NET**

---

## Limitations connues

`openSet` utilise une `List<Node>` avec recherche linéaire O(n). Une priority queue (Min-Heap) donnerait O(log n) et serait plus performante sur des grilles larges. Acceptable pour le scope actuel du projet.

---

## Améliorations prévues

- Implémentation d'une Min-Heap pour optimiser la recherche dans l'openSet
- Support des terrains à coût variable (zones lentes, zones rapides)
- Visualisation du chemin calculé directement sur la grille via Gizmos
- Gestion multi-agents avec files de priorité indépendantes