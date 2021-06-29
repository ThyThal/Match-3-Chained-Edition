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
	private List<GridNode> checkedNodes = new List<GridNode>();



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
				return true;
            }

			else
            {
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
						if (GameManager.Instance.GameCustomization.GetComboMode() == GameCustomization.ComboMode.HORIZONTAL)
						{
							if ((x == nodeID.x && y != nodeID.y) || (x != nodeID.x && y == nodeID.y))
							{
								Neighbors.Add(node);
							}
						}

						else
                        {
							Neighbors.Add(node);
                        }
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
				SearchAirBelow();
            }
        }
    }

	[ContextMenu("Debug Create New Chain")]
	public void CreateNewChain()
    {
		if (chainedNodes != null) { ClearChained(); }

		chainedNodes.Add(this); // Adds first element for chain.
		checkedNodes.Add(this);
		//if (debugImage != null) { debugImage.color = Color.red; }

		CheckChainedNodes(chainedNodes, 0);
		CheckChainSize(chainedNodes);
    }

	[ContextMenu("NEW CHECKING")]
	public void CheckChainedNodes(List<GridNode> currentChain, int currentNodeIndex)
    {
		int currentNodeAtIndex = currentNodeIndex;
        foreach (var neighbor in currentChain[currentNodeAtIndex].Neighbors)
        {
			if (checkedNodes.Contains(neighbor)) continue;

			else if (neighbor != null && neighbor.currentBlock != null) // CHECK IF NOT NULL VALUES
			{
				//if (debugImage != null) { neighbor.debugImage.color = Color.yellow; }

				if (neighbor.CurrentBlock.BlockType == chainedNodes[0].CurrentBlock.BlockType) // CHECK FOR SAME TYPE	
				{
					for (int i = 0; i < chainedNodes.Count; i++)
					{
						if (!chainedNodes.Contains(neighbor)) // CHECK FOR NOT REPEAT
						{
							chainedNodes.Add(neighbor);
						}
					}
				}
			}
		}

		if (currentNodeAtIndex != chainedNodes.Count - 1) // If it's not the last keep checking.
		{
			CheckChainedNodes(chainedNodes, currentNodeAtIndex + 1);
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

			if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.PLAYER_TURN)
			{
				if (GameManager.Instance.Help == true) { PaintCombo(); }

				if (GameManager.Instance.Help == false)
				{
					AddUpdatedNodes(chainList);
					ClearSuccessfulChain(chainList);
				}
			}
		}
	}
	private void AddUpdatedNodes(List<GridNode> chainList)
    {
        foreach (var node in chainList)
        {
			GameManager.Instance.updatedNodes.Add(node);
            foreach (var vecino in node.Neighbors)
            {
				GameManager.Instance.updatedNodes.Add(vecino);
            }
        }
    }


	/*
	 * Debug
	 */

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
		CreateNewChain();
    }

	private void PaintCombo()
    {
        foreach (var node in chainedNodes)
        {
			if (node.debugImage != null) { node.debugImage.color = Color.green; }
		}
    }

}
