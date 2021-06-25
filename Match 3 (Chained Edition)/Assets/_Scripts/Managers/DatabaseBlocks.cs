using UnityEngine;

public class DatabaseBlocks : MonoBehaviour
{
    [SerializeField] public ScriptableBlock[] Blocks { get; private set; }
    private void Awake()
    {
        Blocks = Resources.LoadAll<ScriptableBlock>("Blocks/");
    }
}
