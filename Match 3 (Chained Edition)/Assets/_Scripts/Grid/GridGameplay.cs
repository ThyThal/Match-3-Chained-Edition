using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGameplay : MonoBehaviour
{
    [SerializeField] private int minimumCombo = 3;

    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private List<GridNode> gridChain;

    public int MinimumCombo
    {
        get { return minimumCombo; }
    }



    [ContextMenu("Debug Find Chain")]
    private void FindChain()
    {
        foreach (var node in gridGenerator.NodesArray)
        {
            node.CreateNewChain(true);
        }
    }
}
