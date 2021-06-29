using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Match3Generate : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private List<GridNode> nodes;
    private List<GridNode> exclude = new List<GridNode>();
    private List<GridNode> chainedNodes = new List<GridNode>();
    private int generatedCombos = 0;

    public void GenerateBlocks()
    {
        nodes = GameManager.Instance.LevelController.NodesArray;

        foreach (var node in nodes)
        {
            var block = Instantiate(blockPrefab, node.transform);
            node.CurrentBlock = block.GetComponent<BlockController>();
        }
    }

    [ContextMenu("DASDAS")]
    public void DeleteStartingCombos()
    {
        List<GridNode> exclude = new List<GridNode>();

        foreach (var node in nodes)
        {
            node.CreateNewChain();
        }
    }

    public void CreateStartingCombos()
    {
        if (generatedCombos < GameManager.Instance.GameCustomization.GetStartingCombos)
        {
            // Generate New List.
            List<GridNode> availableNodes = GameManager.Instance.LevelController.NodesArray;
            if (exclude != null)
            {
                availableNodes = nodes.Except(exclude).ToList();
            }

            // Get Random Node & Exclude
            GridNode currentNode = nodes[Random.Range(0, availableNodes.Count)];
            chainedNodes.Add(currentNode);
            exclude.Add(currentNode);

            // Check for more same types.
            //CheckNeighborsType(currentNode);
            //CreateRandomComboAmount(randomNode);

            // Recursion
            generatedCombos++;
            CreateStartingCombos();
        }
    }

    private void CheckNeighborsType(GridNode currentNode)
    {
        foreach (var node in chainedNodes)
        {
            foreach (var neighbor in node.Neighbors)
            {
                if (neighbor.CurrentBlock.BlockType == chainedNodes[0].CurrentBlock.BlockType)
                {
                    chainedNodes.Add(neighbor);
                }
            }
        }
    }


    private void CreateRandomComboAmount(GridNode currentNode)
    {
        if (chainedNodes.Count <= GameManager.Instance.GameCustomization.GetComboAmount)
        {


            /*
            int randomVecino = Random.Range(0, currentNode.Neighbors.Count);
            GridNode vecino = currentNode.Neighbors[randomVecino];*/
        }
    }
}
