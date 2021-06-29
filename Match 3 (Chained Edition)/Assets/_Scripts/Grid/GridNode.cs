using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridNode : MonoBehaviour
{
	[Header("Node Information")]
	[SerializeField] private Vector2 nodeID;
	[SerializeField] private List<GridNode> neighbors;
	[SerializeField] private GridNode nodeBelow;
	[SerializeField] private List<GridNode> chainedNodes;

	[Header("Node Data")]
	[SerializeField] private BlockController currentBlock;
	[SerializeField]  private GridNode lastFallLocation;
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
	public GridNode NodeBelow
    {
		get { return nodeBelow; }
    }
	public bool HasBlock
    {
        get
        {
			if (CurrentBlock != null)
            {
				debugImage.color = Color.blue;
				return true;
            }

			else
            {
				debugImage.color = Color.gray;
				return false;
            }
        }
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

					if (x == nodeID.x + 1 && y == nodeID.y)
                    {
						nodeBelow = node;
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
			GameManager.Instance.updatedNodes.Add(node);
			//node.chainedNodes.Clear();
		}

		ClearChained();
	}

	[ContextMenu("Last Fall Location")]
	public void SearchAirBelow()
    {
		if (nodeBelow.HasBlock == false && lastFallLocation == null)
        {
			lastFallLocation = nodeBelow; // Set First Fall Location.
        }

		if (lastFallLocation != null)
        {
			if (lastFallLocation.nodeBelow.HasBlock == false)
            {
				lastFallLocation = lastFallLocation.NodeBelow;
				lastFallLocation.debugImage.color = Color.yellow;
				SearchAirBelow();
            }
        }
    }

	[ContextMenu("Debug Create New Chain")]
	public void CreateNewChain(GameCustomization.ComboMode comboMode)
    {
		if (chainedNodes != null) { ClearChained(); }
		chainedNodes.Add(this); // Adds first element for chain.
		CheckChainType(comboMode, chainedNodes, 0); // Check for chain in first element.
		CheckChainSize(chainedNodes);
    }
	private void CheckChainType(GameCustomization.ComboMode comboMode, List<GridNode> chainList, int currentNode)
    {
		if (comboMode == GameCustomization.ComboMode.DIAGONAL)
        {
			CheckDiagonalChains(chainList, currentNode);
        }

		else if (comboMode == GameCustomization.ComboMode.HORIZONTAL)
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
			if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GENERATE_GAME)
            {
				RandomizeType();
            }

			else
            {
				AddUpdatedNodes(chainList);
				ClearSuccessfulChain(chainList);
            }

		}
	}
	private void AddUpdatedNodes(List<GridNode> chainList)
    {
        foreach (var node in chainList)
        {
			GameManager.Instance.updatedNodes.Add(node);
			node.debugImage.color = Color.green;

            foreach (var vecino in node.Neighbors)
            {
				GameManager.Instance.updatedNodes.Add(vecino);
				vecino.debugImage.color = Color.white;
            }
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
	[ContextMenu("NEW BLOCK")]
	private void RandomizeType()
    {
		List<ScriptableBlock> blockTypes = GameManager.Instance.BlocksData.Blocks.ToList();
		List<ScriptableBlock> usedTypes = new List<ScriptableBlock>();

		foreach (var vecino in Neighbors)
		{
			usedTypes.Add(vecino.CurrentBlock.BlockType);
		}

		List<ScriptableBlock> freeTypes = blockTypes.Except(usedTypes).ToList();
		CurrentBlock.BlockType = freeTypes[Random.Range(0, freeTypes.Count)];
	}

	public void CHAIN()
    {
		CreateNewChain(GameManager.Instance.GameCustomization.GetComboMode());
    }

}
