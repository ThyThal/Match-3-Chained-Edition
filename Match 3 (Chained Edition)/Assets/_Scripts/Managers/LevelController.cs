using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class LevelController : MonoBehaviour
{
    [Header("Game Main Variables")]
    [SerializeField] public List<GridNode> NodesArray = new List<GridNode>();

    [Header("Game State Variables")]
    [SerializeField] private Text stateText;
    [SerializeField] private GameState state;
    private static event Action<GameState> OnGameStateChanged;

    [Header("Game Components")]
    [SerializeField] private Match3Creator match3Creator;
    [SerializeField] private Match3Generate match3Generate;

    public enum GameState
    {
        CREATE_BOARD,
        GENERATE_GAME,
        PLAYER_TURN,
        REGENERATE_BOARD
    }
    public GameState CurrentState
    {
        get { return state; }
    }

    private void Start()
    {
        UpdateGameState(GameState.CREATE_BOARD);
    }

    private void UpdateGameState(GameState newState)
    {
        state = newState;
        stateText.text = newState.ToString();

        switch (newState)
        {
            case GameState.CREATE_BOARD:
                HandleCreateBoard();
                break;

            case GameState.GENERATE_GAME:
                HandleGenerateGame();
                break;

            case GameState.PLAYER_TURN:
                break;

            case GameState.REGENERATE_BOARD:
                break;

            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleCreateBoard()
    {
        match3Creator.GenerateBoardSize(); // Generates Game Size
        match3Creator.GenerateBoardNodes(); // Creates Nodes.

        Debug.Log("[0] Finished Creating Game!");
        UpdateGameState(GameState.GENERATE_GAME);
    }

    private void HandleGenerateGame()
    {
        match3Generate.GenerateBlocks(); // Generates First Blocks
        match3Generate.DeleteStartingCombos();
        match3Generate.CreateStartingCombos();



        Debug.Log("[1] Finished Creating Blocks!");
        UpdateGameState(GameState.PLAYER_TURN);
    }
}
