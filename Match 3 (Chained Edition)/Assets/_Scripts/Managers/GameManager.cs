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

    [SerializeField] private DatabaseBlocks blocksData;
    [SerializeField] private GridGameplay gridGameplay;
    public DatabaseBlocks BlocksData
    {
        get { return blocksData; }
    }

    public GridGameplay GridGameplay
    {
        get { return gridGameplay; }
    }
}
