using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [Header("Main Things")]
    [SerializeField] private List<GridNode> nodesArray;
    [SerializeField] private List<GameObject> pooledBlocks;

    [Header("Grid")]
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private GameObject gridNode;

    [Header("Cells")]
    [SerializeField] private Vector2 cellSize = new Vector2(90, 90);
    [SerializeField] private Vector2 spacing = new Vector2(10, 10);

    [Header("Components")]
    [SerializeField] private RectTransform gridHolder;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform gridBackground;

    private void Start()
    {
        // Generate Game Nodes.
        GenerateBoardSize();
        GenerateBoardNodes();

        // Restart Game (First Generation)
    }

    /*
     * Properties
     */
    public List<GridNode> NodesArray
    {
        get { return nodesArray; }
    }

    /*
     * First Game Methods.
     */
    [ContextMenu("(Update) Board Size View")]
    private void GenerateBoardSize() // Board Size Visual. 
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
    private void GenerateBoardNodes() // Instantiate Nodes & Blocks.
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject nodeObject = Instantiate(gridNode, gridLayout.transform);
                GridNode node = nodeObject.GetComponent<GridNode>();
                node.name = $"NodeBlock ({x}, {y})";
                node.NodeID = new Vector2(x, y);
                nodesArray.Add(node);
            }
        }

        GenerateNodesNeighbours();
    }
    private void GenerateNodesNeighbours() // Generate Neighbours.
    {
        foreach (var node in nodesArray)
        {
            node.GetNeighborNodes(nodesArray);
        }
    }

    /*
     * Restart Game Methods.
     */
}
