using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Match3Generate : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private List<GridNode> nodes;
    private List<GridNode> exclude;

    public void GenerateBlocks()
    {
        nodes = GameManager.Instance.LevelController.NodesArray;

        foreach (var node in nodes)
        {
            var block = Instantiate(blockPrefab, node.transform);
            node.CurrentBlock = block.GetComponent<BlockController>();
        }
    }

    [ContextMenu("DASDAS")]
    public void DeleteStartingCombos(GameCustomization.ComboMode comboMode)
    {
        List<GridNode> exclude = new List<GridNode>();

        foreach (var node in nodes)
        {
            node.CreateNewChain(GameManager.Instance.GameCustomization.GetComboMode());
        }
    }

    public void CreateStartingCombos()
    {

    }
}
