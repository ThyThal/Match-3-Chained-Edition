using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            var block = GameManager.Instance.LevelController.PoolingSystem.SpawnFromPool("Blocks", node.transform);
            //var block = Instantiate(blockPrefab, node.transform);
            node.CurrentBlock = block.GetComponent<BlockController>();
        }
    }

    [ContextMenu("DASDAS")]
    public void DeleteStartingCombos()
    {
        List<GridNode> exclude = new List<GridNode>();

        foreach (var node in nodes)
        {
            node.DetectChain();
        }
    }

    public void CreateStartingCombos()
    {
        chainedNodes.Clear();

        if (generatedCombos < GameManager.Instance.GameCustomization.startingCustomization.StartingCombos)
        {
            // Generate New List with exclusions.
            List<GridNode> availableNodes = GameManager.Instance.LevelController.NodesArray;
            if (exclude != null) { availableNodes = nodes.Except(exclude).ToList(); }

            // Get Random Node
            GridNode currentNode = availableNodes[Random.Range(0, availableNodes.Count)];
            chainedNodes.Add(currentNode); // Adds first element of chain.
            exclude.Add(currentNode); // Ads node to exclusion.

            // Check for more same types.
            CheckNeighborsType(chainedNodes, 0);
            CreateCombo(chainedNodes);

            // Recursion
            generatedCombos++;
            CreateStartingCombos();
        }
    }

    private void CheckNeighborsType(List<GridNode> newChain, int currentIndex)
    {       
        // Adds neighbors if any is the same, also exclude.
        foreach (var neighbor in newChain[currentIndex].Neighbors)
        {
            if (!chainedNodes.Contains(neighbor))
            {
                if (!exclude.Contains(neighbor))
                {               
                }

                if (neighbor.CurrentBlock.BlockType == newChain[0].CurrentBlock.BlockType)
                {
                    if (!newChain.Contains(neighbor))
                    {
                        newChain.Add(neighbor);
                        exclude.Add(neighbor);
                    }
                }
            }
        }

        if (currentIndex != newChain.Count - 1) // If it's not the last keep checking.
        {
            CheckNeighborsType(newChain, currentIndex+1);
        }        
    }


    private void CreateCombo(List<GridNode> chainedNodes)
    {
        int generateAmount = GameManager.Instance.GameCustomization.playerCustomization.ComboAmount - chainedNodes.Count; // Difference for minimum combo.
        GridNode randomFromChain = chainedNodes[Random.Range(0, chainedNodes.Count)];

        for (int i = 0; i < generateAmount; i++)
        {
            foreach (var neighbor in randomFromChain.Neighbors)
            {
                if (!exclude.Contains(neighbor))
                {
                    exclude.Add(neighbor);
                    neighbor.CurrentBlock.BlockType = chainedNodes[0].CurrentBlock.BlockType;
                    break;
                }                
            }
        }
    }
}
