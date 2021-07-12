using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Gameplay : MonoBehaviour
{
    [SerializeField] private List<GridNode> playerChainlink;
    [SerializeField] private bool startedChain = false;
    [SerializeField] private ScriptableBlock chainType;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool successfulChain = false;

    public List<GridNode> PlayerChainlink
    {
        get { return playerChainlink; }
        set { playerChainlink = value; }
    }
    public bool SuccessfulChain
    {
        get { return successfulChain; }
    }

    public void ClearSuccessfulChain()
    {
		if (playerChainlink.Count >= GameManager.Instance.GameCustomization.playerCustomization.ComboAmount)
		{
			if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GAME_PLAYER_TURN)
			{
                var timer = playerChainlink[0].CurrentBlock.DestroyTime;
				StartCoroutine(DestroyBlocks(timer));
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
            if (successfulChain == true)
            {
                node.ClearChained();
                node.CurrentBlock.DestroyBlock();
                node.CurrentBlock = null;
                GameManager.Instance.LevelController.NodesArray[node.NodeIndex] = node;
            }
        }

        playerChainlink.Clear();
    }

    /*
     * Chainlink
     */
    public void ChainlinkCreate(GridNode currentNode)
    {
        if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GAME_PLAYER_TURN && currentNode.CurrentBlock != null && startedChain == false)
        {
            startedChain = true;
            playerChainlink.Add(currentNode);
            chainType = currentNode.CurrentBlock.BlockType;
            currentNode.CurrentBlock.HandleSelectedBlock();
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 0));
        }
    }
    public void ChainlinkCheck(GridNode currentNode)
    {
        if (startedChain == true && currentNode.CurrentBlock != null && successfulChain == false)
        {
            // Removing
            if (playerChainlink.Contains(currentNode) && playerChainlink.Count > 1)
            {
                var previousNode = playerChainlink[playerChainlink.Count - 2]; // Previous of the last element;
                if (previousNode == currentNode)
                {
                    playerChainlink[playerChainlink.Count - 1].CurrentBlock.HandleUnselectBlock();
                    playerChainlink[playerChainlink.Count - 1].debugImage.color = Color.clear; // Chain Remove Color.
                    playerChainlink.RemoveAt(playerChainlink.Count - 1);

                    if (lineRenderer.positionCount > 0)
                    {
                        lineRenderer.positionCount--;
                    }
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
                        currentNode.CurrentBlock.HandleSelectedBlock();
                        playerChainlink.Add(currentNode);
                        //currentNode.debugImage.color = Color.yellow; // Chain Added Block.

                        if (lineRenderer.positionCount < playerChainlink.Count)
                        {
                            lineRenderer.positionCount++;
                        }

                        // Adding Lines
                        lineRenderer.SetPosition(playerChainlink.Count - 1, new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 0));
                    }
                }
            }
        }
    }
    public void ChainlinkClear()
    {
        if (playerChainlink.Count >= GameManager.Instance.GameCustomization.playerCustomization.ComboAmount)
        {
            successfulChain = true;

            var timer = playerChainlink[0].CurrentBlock.DestroyTime;
            StartCoroutine(DestroyBlocks(timer));          
        }

        else
        {
            successfulChain = false;

            foreach (var node in playerChainlink)
            {
                node.CurrentBlock.HandleUnselectBlock();
                node.debugImage.color = Color.clear;
            }

            playerChainlink.Clear();
            startedChain = false;
        }
        
        lineRenderer.positionCount = 0;
    }

    private IEnumerator DestroyBlocks(float waiting)
    {
        foreach (var node in playerChainlink)
        {
            BlockController block = node.CurrentBlock;
            block.Destroyed = true;
        } // Play Destroy Effect

        yield return new WaitForSeconds(waiting);
        RemoveBlocks();

        if (GameManager.Instance.LevelController.Match3Gameplay.SuccessfulChain == true)
        {
            successfulChain = false;
            startedChain = false;
            GameManager.Instance.LevelController.UpdateGameState(LevelController.GameState.GAME_FALLING_BLOCKS);
        }
    }
}
