using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridNode : MonoBehaviour
{
	[Header("Node Information")]
	[SerializeField] private Vector2 nodeID;
	[SerializeField] private List<GridNode> neighbors;
	[SerializeField] private List<GridNode> chainedNodes;

	[Header("Node Data")]
	[SerializeField] private BlockController currentBlock;
	private Image debugImage;



	/*
	 * Properties
	 */
	public Vector2 NodeID
	{
		get { return nodeID; }
		set { nodeID = value; }
	}
	public List<GridNode> Neighbors
	{
        get { return neighbors; }
    }
	public BlockController CurrentBlock
    {
		get { return currentBlock;}
		set { currentBlock = value; }
	}

    /*
	 * MonoBehaviours
	 */
    private void Start()
    {
		debugImage = GetComponent<Image>();
    }

    /*
	 * Methods Starting
	 */
    public List<GridNode> GetNeighborNodes(List<GridNode> nodesList, List<GridNode> exclude = null)
    {
		List<GridNode> resultList = new List<GridNode>();

		if (exclude == null) // Create Visited Blocks.
		{
			exclude = new List<GridNode> { this, };
		}

		else // Add to Visited.
		{
			exclude.Add(this);
		}

		for (int x = (int)nodeID.x - 1; x < (int)nodeID.x + 2; x++)
		{
			for (int y = (int)nodeID.y - 1; y < (int)nodeID.y + 2; y++)
			{
				if (x == nodeID.x && y == nodeID.y) continue; // Skip own cell.
				
				else
                {
					GridNode node = FindNeighborID(x, y, nodesList);

					if (node != null )
					{						
						Neighbors.Add(node);
					}

                }
			}
		}

		return resultList;
    }
	private GridNode FindNeighborID(int currX, int currY, List<GridNode> nodesList)
    {
		for (int i = 0; i < nodesList.Count; i++)
		{
			GridNode currentNode = nodesList[i];
			if (currentNode.NodeID == new Vector2(currX, currY))
			{
				return currentNode;
			}
		}

		return null;
    }

	/*
	 * Methods Gameplay
	 */
	private void ClearChained()
    {
		chainedNodes.Clear();
    }
	private void ClearSuccessfulChain(List<GridNode> chainList)
    {
		foreach (var node in chainList)
		{
			node.CurrentBlock.Destroy();
			//node.chainedNodes.Clear();
		}

		ClearChained();
	}

	[ContextMenu("Debug Create New Chain")]
	public void CreateNewChain(bool diagonalChain)
    {
		if (chainedNodes != null) { ClearChained(); }
		chainedNodes.Add(this); // Adds first element for chain.
		CheckChainType(diagonalChain, chainedNodes, 0); // Check for chain in first element.
		CheckChainSize(chainedNodes);
    }
	private void CheckChainType(bool diagonalChain, List<GridNode> chainList, int currentNode)
    {
		if (diagonalChain == true)
        {
			CheckDiagonalChains(chainList, currentNode);
        }

		else
        {
			CheckAdyacentChains(chainList, currentNode);
        }
    } // Diagonal or Adyacent.
	private void CheckDiagonalChains(List<GridNode> chainList, int currentNode)
    {
		int currentNodeInChain = currentNode;

		foreach (var vecino in chainList[currentNodeInChain].Neighbors)
		{
			//vecino.debugImage.color = Color.yellow; // IS PART OF CHECKED

			if (vecino != null && vecino.currentBlock != null)
			{
				if (vecino.CurrentBlock.BlockType == chainList[0].CurrentBlock.BlockType)
				{
					for (int i = 0; i < chainList.Count; i++)
					{
						if (!chainList.Contains(vecino))
						{
							chainList.Add(vecino);
						}
					}
				}
			}
		}

		if (currentNodeInChain >= chainList.Count - 1)
		{
            foreach (var item in chainList)
            {
				//item.debugImage.color = Color.blue; // ITEMS IN CHAIN
            }
		}

		else
		{
			CheckDiagonalChains(chainList, currentNodeInChain + 1);
		}
	}
	private void CheckAdyacentChains(List<GridNode> chainList, int currentNode)
    {
		int currentNodeInChain = currentNode;

		foreach (var vecino in chainList[currentNodeInChain].Neighbors)
		{
			if (vecino.nodeID.x == chainList[currentNodeInChain].NodeID.x || vecino.nodeID.y == chainList[currentNodeInChain].NodeID.y) // CHECK FOR ADYACENT.
            {
				vecino.debugImage.color = Color.yellow; // IS PART OF CHECKED

				if (vecino != null && vecino.currentBlock != null) // CHECK IF NOT NULL VALUES
				{
					if (vecino.CurrentBlock.BlockType == chainList[0].CurrentBlock.BlockType) // CHECK FOR SAME TYPE	
					{
						for (int i = 0; i < chainList.Count; i++)
						{
							if (!chainList.Contains(vecino)) // CHECK FOR NOT REPEAT
							{
								chainList.Add(vecino);
							}
						}
					}
				}
			}
		}

		if (currentNodeInChain != chainList.Count - 1) // If it's not the last keep checking.
		{
			CheckAdyacentChains(chainList, currentNodeInChain + 1);
		}
	}

	private void CheckChainSize(List<GridNode> chainList)
	{
		if (chainList.Count >= GameManager.Instance.GridGameplay.MinimumCombo)
		{
			ClearSuccessfulChain(chainList);
		}
	}


	/*
	 * Debug
	 */
	[ContextMenu("Debug Paint Neighbors")]
	private void DebugPaintNeighbors()
    {
		debugImage.color = Color.yellow;

        foreach (var item in Neighbors)
        {
			item.debugImage.color = Color.green;
        }
    }
	[ContextMenu("Debug Clear Neighbors")]
	private void DebugClearNeighbors()
	{
		debugImage.color = Color.clear;

		foreach (var item in Neighbors)
		{
			item.debugImage.color = Color.clear;
		}
	}
}
