using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockModel : MonoBehaviour
{
    [SerializeField] private ScriptableBlock blockType;
    public ScriptableBlock BlockType
    {
        get { return blockType; }
        set { blockType = value; }
    }
}
