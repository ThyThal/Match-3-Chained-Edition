using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Gameplay : MonoBehaviour
{
    [SerializeField] private List<GridNode> playerChainlink;
    [SerializeField] private bool startedChain = false;
    [SerializeField] private ScriptableBlock chainType;
    [SerializeField] private LineRenderer lineRenderer;


    public List<GridNode> PlayerChainlink
    {
        get { return playerChainlink; }
        set { playerChainlink = value; }
    }


    public void ClearSuccessfulChain()
    {
		if (playerChainlink.Count >= GameManager.Instance.GameCustomization.GetComboAmount)
		{
			if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GAME_PLAYER_TURN)
			{
				RemoveBlocks();
			}
		}

        else
        {
            playerChainlink.Clear();
        }
	}

    private void RemoveBlocks()
    {
        foreach (var node in playerChainlink)
        {
            node.ClearChained();
            node.CurrentBlock.DestroyBlock();
            node.CurrentBlock = null;
            GameManager.Instance.LevelController.NodesArray[node.NodeIndex] = node;
        }

        playerChainlink.Clear();
    }



    /*
     * Chainlink
     */
    public void ChainlinkCreate(GridNode currentNode)
    {
        if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GAME_PLAYER_TURN)
        {
            startedChain = true;
            playerChainlink.Add(currentNode);
            chainType = currentNode.CurrentBlock.BlockType;
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 0));
        }
    }

    public void ChainlinkCheck(GridNode currentNode)
    {
        if (startedChain == true)
        {
            if (playerChainlink.Contains(currentNode) && playerChainlink.Count > 1)
            {
                var previousNode = playerChainlink[playerChainlink.Count - 2]; // Previous of the last element;
                if (previousNode == currentNode)
                {
                    playerChainlink[playerChainlink.Count - 1].debugImage.color = Color.clear; // Chain Remove Color.
                    playerChainlink.RemoveAt(playerChainlink.Count - 1);
                    lineRenderer.positionCount--;
                }
            }

            // Check if it's not in the list and is the first one.
            else if (!playerChainlink.Contains(currentNode) && playerChainlink.Count > 0)
            {
                // Check if previous node is neighbor.
                if (currentNode.Neighbors.Contains(playerChainlink[playerChainlink.Count - 1]))
                {
                    // Check if block is the same type as chain.
                    if (currentNode.CurrentBlock.BlockType == chainType)
                    {
                        playerChainlink.Add(currentNode);
                        //currentNode.debugImage.color = Color.yellow; // Chain Added Block.

                        if (lineRenderer.positionCount < playerChainlink.Count)
                        {
                            lineRenderer.positionCount++;
                        }

                        lineRenderer.SetPosition(playerChainlink.Count - 1, new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 0));
                    }
                }
            }
        }
    }

    public void ChainlinkClear()
    {
        if (playerChainlink.Count >= GameManager.Instance.GameCustomization.GetComboAmount)
        {
            foreach (var node in playerChainlink)
            {
                node.debugImage.color = Color.clear;
                node.CurrentBlock.DestroyBlock();
                node.CurrentBlock = null;
            }            
        }

        else
        {
            foreach (var node in playerChainlink)
            {
                node.debugImage.color = Color.clear;
            }
        }

        lineRenderer.positionCount = 0;
        playerChainlink.Clear();
    }
}
