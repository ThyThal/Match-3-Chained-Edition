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
    [SerializeField] private GridGameplay gridGameplay;
    [SerializeField] public List<GridNode> updatedNodes;
    public DatabaseBlocks BlocksData
    {
        get { return blocksData; }
    }

    public GridGameplay GridGameplay
    {
        get { return gridGameplay; }
    }

    public LevelController LevelController
    {
        get { return levelController; }
    }
    public GameCustomization GameCustomization
    {
        get { return gameCustomization; }
    }
}
