using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game Manager Instance
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameCustomization gameCustomization;
    [SerializeField] private LevelController levelController;
    [SerializeField] private DatabaseBlocks blocksData;
    [SerializeField] public List<GridNode> updatedNodes;
    private bool help = false;
    private int bottomID;
    public DatabaseBlocks BlocksData
    {
        get { return blocksData; }
    }

    public LevelController LevelController
    {
        get { return levelController; }
    }
    public GameCustomization GameCustomization
    {
        get { return gameCustomization; }
    }
    public bool Help
    {
        get { return help; }
        set { help = value; }
    }
    public int BottomID
    {
        get { return bottomID; }
        set { bottomID = value; }
    }

}
