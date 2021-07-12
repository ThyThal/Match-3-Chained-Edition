using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private BlockModel blockModel;
    [SerializeField] private BlockView blockView;
	[SerializeField] private float destroyTime = 0.25f;
	[SerializeField] private bool destroyed = false;

	/*
	 * Properties
	 */
	public ScriptableBlock BlockType
    {
		get { return blockModel.BlockType; }
		set { blockModel.BlockType = value; UpdateBlockType(); }
    }
	public float DestroyTime
    {
		get { return destroyTime; }
    }
	public bool Destroyed
    {
		get { return destroyed; }
		set { destroyed = value; }
    }

    /*
	 * MonoBehaviours
	 */
    private void Awake()
    {
		SelectRandomBlock();
		var trans = GetComponent<RectTransform>();
		trans.sizeDelta = GameManager.Instance.LevelController.Match3Creator.CellSize;
	}

	/*
	 * Methods
	 */
	public void SelectRandomBlock()
    {
		// Data
		blockModel.BlockType = GameManager.Instance.BlocksData.Blocks[Random.Range(0, GameManager.Instance.BlocksData.Blocks.Length)];

		// View
		blockView.Artwork.sprite = blockModel.BlockType.artwork;
	}

	public void DestroyBlock()
	{
		GameManager.Instance.LevelController.PoolingSystem.AddToPool("Blocks", this.gameObject);
		gameObject.SetActive(false);
	}

	[ContextMenu("Update Me!")]
	public void UpdateBlockType()
    {
		blockView.Artwork.sprite = blockModel.BlockType.artwork;
    }

	public void HandleSelectedBlock()
    {
		blockView.TriggerShaderSelect();
    }

	public void HandleUnselectBlock()
    {
		blockView.TriggerShaderUnselect();
    }
	public void ResetBlock()
    {
		Destroyed = false;
		blockView.ResetShader();
    }
}
