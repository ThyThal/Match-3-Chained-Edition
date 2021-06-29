using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match3Creator : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Vector2 gridSize = new Vector2(10, 10);
    [SerializeField] private GameObject nodePrefab;

    [Header("Cells")]
    [SerializeField] private Vector2 cellSize = new Vector2(100, 100);
    [SerializeField] private Vector2 spacing = new Vector2(5, 5);

    [Header("Components")]
    [SerializeField] private RectTransform gridHolder;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform gridBackground;

    public void GenerateBoardSize() // Board Size Visual. 
    {
        // Board Holder
        gridHolder.sizeDelta = (cellSize * gridSize) + ((gridSize - Vector2.one) * spacing);

        // Board Grid Layout Group
        gridLayout.cellSize = cellSize;
        gridLayout.spacing = spacing;

        // Board Background
        gridBackground.offsetMin = -spacing;
        gridBackground.offsetMax = spacing;
    }
    public void GenerateBoardNodes() // Instantiate Nodes & Blocks.
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject nodeObject = Instantiate(nodePrefab, gridLayout.transform);
                GridNode node = nodeObject.GetComponent<GridNode>();
                node.name = $"NodeBlock ({x}, {y})";
                node.NodeID = new Vector2(x, y);
                GameManager.Instance.LevelController.NodesArray.Add(node);
            }
        }

        GenerateNodesNeighbours();
    }
    private void GenerateNodesNeighbours() // Generate Neighbours.
    {
        var nodesArray = GameManager.Instance.LevelController.NodesArray;

        foreach (var node in nodesArray)
        {
            node.GetNeighborNodes(nodesArray);
        }
    }
}
