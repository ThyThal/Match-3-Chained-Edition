using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Automatic : MonoBehaviour
{
    [SerializeField] private List<GridNode> chainedNodes;
    [SerializeField] private List<GridNode> usedNodes;
    [SerializeField] private List<GridNode> chainedNodesClear;

    [ContextMenu("Test Automatic")]
    public void FindAutomaticChains()
    {
        List<GridNode> nodesList = GameManager.Instance.LevelController.NodesArray;
        usedNodes.Clear();

        foreach (var node in nodesList)
        {
            chainedNodes.Clear();

            if (!usedNodes.Contains(node))
            {
                usedNodes.Add(node);
                chainedNodes.Add(node);
                FindAutomaticChained(node);
                CheckChainSize(chainedNodes);
            }
        }

        if (chainedNodesClear.Count > 0)
        {
            ClearAutomaticChains();
        }

        if (chainedNodesClear.Count <= 0)
        {
            GameManager.Instance.LevelController.UpdateGameState(LevelController.GameState.GAME_PLAYER_TURN);
        }
    }

    private void FindAutomaticChained(GridNode node)
    {
        switch (GameManager.Instance.GameCustomization.automaticChain.AutomaticChainMode)
        {
            case GameCustomization.AutomaticChainMode.DIAGONAL:
                FindDiagonalCombos(chainedNodes, 0);
                break;

            case GameCustomization.AutomaticChainMode.HORIZONTAL:
                FindHorizontalCombos(chainedNodes, 0);
                break;

            default:
                break;
        }
    }

    private void FindHorizontalCombos(List<GridNode> chainedNodes, int index)
    {
        GridNode currentNode = chainedNodes[index];

        foreach (var vecino in currentNode.Neighbors)
        {
            // Check Node Horizontal
            if ((vecino.NodeID.x == currentNode.NodeID.x && vecino.NodeID.y != currentNode.NodeID.y) || (vecino.NodeID.x != currentNode.NodeID.x && vecino.NodeID.y == currentNode.NodeID.y))
            {
                if (vecino.CurrentBlock.BlockType == chainedNodes[0].CurrentBlock.BlockType)
                {
                    if (!chainedNodes.Contains(vecino))
                    {
                        chainedNodes.Add(vecino);
                        usedNodes.Add(vecino);
                    }
                }
            }
        }

        if (index != chainedNodes.Count - 1) // If it's not the last keep checking.
        {
            FindHorizontalCombos(chainedNodes, index + 1);
        }
    }
    private void FindDiagonalCombos(List<GridNode> chainedNodes, int index)
    {
        GridNode currentNode = chainedNodes[index];

        foreach (var vecino in currentNode.Neighbors)
        {
            if (vecino.CurrentBlock.BlockType == chainedNodes[0].CurrentBlock.BlockType)
            {
                if (!chainedNodes.Contains(vecino))
                {
                    chainedNodes.Add(vecino);
                    usedNodes.Add(vecino);
                }
            }
        }

        if (index != chainedNodes.Count - 1) // If it's not the last keep checking.
        {
            FindDiagonalCombos(chainedNodes, index + 1);
        }
    }
    private void CheckChainSize(List<GridNode> currentList)
    {
        if (currentList.Count >= GameManager.Instance.GameCustomization.automaticChain.ComboAmount)
        {
            foreach (var node in currentList)
            {
                chainedNodesClear.Add(node);
            }

            chainedNodes.Clear();
        }

        else
        {
            chainedNodes.Clear();
        }
    }

    private void ClearAutomaticChains()
    {
        var timer = chainedNodesClear[0].CurrentBlock.DestroyTime;
        StartCoroutine(DestroyBlocks(timer));
    }
    private IEnumerator DestroyBlocks(float waiting)
    {
        foreach (var node in chainedNodesClear)
        {
            BlockController block = node.CurrentBlock;
            block.Destroyed = true;
        } // Play Destroy Effect

        yield return new WaitForSeconds(waiting);
        RemoveBlocks();

        GameManager.Instance.LevelController.UpdateGameState(LevelController.GameState.GAME_FALLING_BLOCKS);
    }

    private void RemoveBlocks()
    {
        foreach (var node in chainedNodesClear)
        {
            node.ClearChained();
            node.CurrentBlock.DestroyBlock();
            node.CurrentBlock = null;
            GameManager.Instance.LevelController.NodesArray[node.NodeIndex] = node;
        }

        chainedNodesClear.Clear();
    }
}
