using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridNode : MonoBehaviour
{
	[Header("Node Information")]
	[SerializeField] private Vector2 nodeID;
	[SerializeField] private int nodeIndex;
	[SerializeField] private List<GridNode> neighbors;
	[SerializeField] private GridNode nodeBelow;
	[SerializeField] private List<GridNode> chainedNodes;

	[Header("Node Data")]
	[SerializeField] private BlockController currentBlock;
	[SerializeField]  private GridNode lastFallLocation;
	public Image debugImage;
	private List<GridNode> checkedNodes = new List<GridNode>();
	public event EventHandler FinishedChainlink;



	/*
	 * Properties
	 */
	public Vector2 NodeID
	{
		get { return nodeID; }
		set { nodeID = value; }
	}
	public int NodeIndex
    {
        get { return nodeIndex; }
		set { nodeIndex = value; }
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
	public GridNode FallLocation
    {
        get { return lastFallLocation; }
		set { lastFallLocation = value; }
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
		FinishedChainlink += TriggerEndTurn;
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
						if (GameManager.Instance.GameCustomization.playerCustomization.PlayerChainMode == GameCustomization.PlayerChainMode.HORIZONTAL)
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
	public void ClearChained()
	{
		chainedNodes.Clear();
	}

	[ContextMenu("Last Fall Location")]
	public void SearchLastFallingLocation()
    {
		if (NodeBelow != null && HasBlock == true) // Current Block Check
        {
			if (lastFallLocation == null && NodeBelow.HasBlock == false) // Sets first fall location.
            {
				lastFallLocation = NodeBelow;
				//lastFallLocation.debugImage.color = Color.magenta;
			}

			if (lastFallLocation != null) // If has fall location.
            {
				if (lastFallLocation.NodeBelow != null) // Not last row.
                {
					if (lastFallLocation.nodeBelow.HasBlock == false)
                    {
						lastFallLocation = lastFallLocation.nodeBelow;
						//lastFallLocation.debugImage.color = Color.red; // Updated Location
						SearchLastFallingLocation();
                    }
                }

				//lastFallLocation.debugImage.color = Color.green;

			}
        }
	}

	[ContextMenu("Debug Create New Chain")]
	public void DetectChain()
    {
		if (chainedNodes != null) { chainedNodes.Clear(); }

		chainedNodes.Add(this); // Adds first element for chain.
		checkedNodes.Add(this);

		CheckChainedNodes(chainedNodes, 0);
		//CheckChainSize(chainedNodes);
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
		CurrentBlock.BlockType = freeTypes[UnityEngine.Random.Range(0, freeTypes.Count)];
	}

	public void CHAIN()
    {
		if (GameManager.Instance.LevelController.CurrentState == LevelController.GameState.GAME_PLAYER_TURN)
        {
			DetectChain();
			GameManager.Instance.LevelController.Match3Gameplay.PlayerChainlink = new List<GridNode>(chainedNodes);
			chainedNodes.Clear();
			FinishedChainlink?.Invoke(this, EventArgs.Empty);
		}
	}

	private void HelperPaintCombo()
    {
        foreach (var node in chainedNodes)
        {
			if (node.debugImage != null) { node.debugImage.color = Color.green; }
		}
    }

	private void TriggerEndTurn(object sender, EventArgs e)
    {
		GameManager.Instance.LevelController.HandlePlayerTurn(this);
	}



	/*
	 * Chainlink
	 */
	public void HandleCreateChainlink()
    {
		GameManager.Instance.LevelController.Match3Gameplay.ChainlinkCreate(this);
		//FinishedChainlink?.Invoke(this, EventArgs.Empty);
	}
	public void HandleAddChainlink()
    {
		GameManager.Instance.LevelController.Match3Gameplay.ChainlinkCheck(this);
	}

	public void HandleStopChain()
    {
		FinishedChainlink?.Invoke(this, EventArgs.Empty);
	}
}
