using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Gameplay : MonoBehaviour
{
    [SerializeField] private List<GridNode> playerChainlink;
    public List<GridNode> PlayerChainlink
    {
        get { return playerChainlink; }
        set { playerChainlink = value; }
    }
    public void CreateChainlink()
    {

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
}
