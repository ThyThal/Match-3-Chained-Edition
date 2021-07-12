using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Falling : MonoBehaviour
{
    [SerializeField] private const float tweenDuration = 0.5f;
    private GridNode currentNode;
    private Sequence newSequence;

    public void UpdateFallingBlocks()
    {
        UpdateEverything();
    }

    private void UpdateEverything()
    {
        List<GridNode> allNodes = new List<GridNode>(GameManager.Instance.LevelController.NodesArray);

        if (newSequence != null)
        {
            newSequence.Kill();
        }

        newSequence = DOTween.Sequence();

        for (int i = allNodes.Count - 1; i >= 0; i--)
        {
            currentNode = allNodes[i];
            currentNode.FallLocation = null;
            currentNode.SearchLastFallingLocation();

            if (currentNode.FallLocation != null)
            {
                AddSequence(currentNode);
            }
        }

        //Task.Run(() => PlayTweenSequence());
        StartCoroutine(PlayTweenSequence());
    }

    private void AddSequence(GridNode currentNode)
    {
        newSequence.Join(currentNode.CurrentBlock.transform.DOMove(currentNode.FallLocation.transform.position, tweenDuration));
        currentNode.CurrentBlock.transform.SetParent(currentNode.FallLocation.transform);
        currentNode.FallLocation.CurrentBlock = currentNode.CurrentBlock;
        currentNode.CurrentBlock = null;
    }

    IEnumerator PlayTweenSequence()
    {
        newSequence.Play();
        yield return new WaitForSeconds(tweenDuration);
        GameManager.Instance.LevelController.UpdateGameState(LevelController.GameState.GAME_REGENERATE_BOARD);
    }
}
