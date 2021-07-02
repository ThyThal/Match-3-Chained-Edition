using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Match3Falling : MonoBehaviour
{
    [SerializeField] private const float tweenDuration = 0.5f;
    private GridNode currentNode;
    private Sequence newSequence;

    public void UpdateFallingBlocks()
    {
        if (GameManager.Instance.GameCustomization.GetUpdatingMode() == GameCustomization.UpdatingMode.UPDATE_USED)
        {
            UpdateUsed();
        }

        if (GameManager.Instance.GameCustomization.GetUpdatingMode() == GameCustomization.UpdatingMode.UPDATE_EVERYTING)
        {
            UpdateEverything();
        }
    }

    private void UpdateUsed()
    {

    }

    private void UpdateEverything()
    {
        List<GridNode> allNodes = new List<GridNode>(GameManager.Instance.LevelController.NodesArray);
        newSequence = DOTween.Sequence();

        for (int i = allNodes.Count - 1; i >= 0; i--)
        {
            currentNode = allNodes[i];
            currentNode.FallLocation = null;
            currentNode.SearchLastFallingLocation();

            if (currentNode.FallLocation != null)
            {
                AddSequence(currentNode);
                //await Fall(currentNode);
            }
        }

        PlayTweenSequence();
    }


    private async Task Fall(GridNode currentNode)
    {
        var sequence = DOTween.Sequence();

        sequence.Join(currentNode.CurrentBlock.transform.DOMove(currentNode.FallLocation.transform.position, tweenDuration));

        await sequence.Play().AsyncWaitForCompletion();

        currentNode.CurrentBlock.transform.parent = currentNode.FallLocation.transform;
        currentNode.FallLocation.CurrentBlock = currentNode.CurrentBlock;
        currentNode.CurrentBlock = null;
    }

    private void AddSequence(GridNode currentNode)
    {
        newSequence.Join(currentNode.CurrentBlock.transform.DOMove(currentNode.FallLocation.transform.position, tweenDuration));
        currentNode.CurrentBlock.transform.SetParent(currentNode.FallLocation.transform);
        currentNode.FallLocation.CurrentBlock = currentNode.CurrentBlock;
        currentNode.CurrentBlock = null;
    }

    private async void PlayTweenSequence()
    {
        await newSequence.Play().AsyncWaitForCompletion();
    }
}
