using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private BlockModel blockModel;
    [SerializeField] private BlockView blockView;

	/*
	 * Properties
	 */
	public ScriptableBlock BlockType
    {
		get { return blockModel.BlockType; }
    }

    /*
	 * MonoBehaviours
	 */
    private void Awake()
    {
		SelectRandomBlock();
	}

	/*
	 * Methods
	 */
	private void SelectRandomBlock()
    {
		// Data
		blockModel.BlockType = GameManager.Instance.BlocksData.Blocks[Random.Range(0, GameManager.Instance.BlocksData.Blocks.Length)];

		// View
		blockView.Artwork.sprite = blockModel.BlockType.artwork;
	}

    public void Destroy()
    {
		Destroy(gameObject);
    }
}
