using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Match3Helper : MonoBehaviour
{
    [SerializeField] private List<GridNode> available = new List<GridNode>();
    [SerializeField] private List<GridNode> exclude = new List<GridNode>();

    private void Start()
    {
        available = GameManager.Instance.LevelController.NodesArray;
    }
    [ContextMenu("Help Me!")]
    public void FindCombos()
    {
        GameManager.Instance.Help = true;

        foreach (var node in available)
        {
            node.DetectChain();
        }

        GameManager.Instance.Help = false;
    }
}
