using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Regenerate : MonoBehaviour
{
    private GameCustomization.RegenerateMode regenerateMode;

    private void Start()
    {
        regenerateMode = GameManager.Instance.GameCustomization.regenerateCustomization.RegenerateMode;
    }

    public void RegenerateBoard()
    {
        switch (regenerateMode)
        {
            case GameCustomization.RegenerateMode.EVERYTHING:
                RegenerateEverything();
                break;

            case GameCustomization.RegenerateMode.FALLING:
                //RegenerateFalling();
                break;

            default:
                break;
        }
    }

    [ContextMenu("Regenerate Board")]
    private void RegenerateEverything()
    {        
        foreach (var node in GameManager.Instance.LevelController.NodesArray)
        {
            if (node.HasBlock == false)
            {
                GameObject newBlock = GameManager.Instance.LevelController.PoolingSystem.SpawnFromPool("Blocks", node.transform);
                BlockController blockController = newBlock.GetComponent<BlockController>();

                blockController.SelectRandomBlock();
                blockController.ResetBlock();
                node.CurrentBlock = blockController;
            }
        }
    }
}
