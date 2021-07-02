﻿using System;
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
    [SerializeField] private Match3Falling match3Falling;
    [SerializeField] private Match3Gameplay match3Gameplay;

    public enum GameState
    {
        GAME_CREATE,
        GAME_GENERATE,
        GAME_PLAYER_TURN,
        GAME_FALLING_BLOCKS,
        REGENERATE_BOARD
    }
    public GameState CurrentState
    {
        get { return state; }
    }
    public Match3Gameplay Match3Gameplay
    {
        get { return match3Gameplay; }
    }

    private void Start()
    {
        UpdateGameState(GameState.GAME_CREATE);
    }

    private void UpdateGameState(GameState newState)
    {
        state = newState;
        stateText.text = newState.ToString();

        switch (newState)
        {
            case GameState.GAME_CREATE:
                //Debug.Log("[0] Creating Game!");
                HandleCreateBoard();
                break;

            case GameState.GAME_GENERATE:
                //Debug.Log("[1] Creating Blocks!");
                HandleGenerateGame();
                break;

            case GameState.GAME_PLAYER_TURN:
                //Debug.Log("[2] Player Turn!");
                break;

            case GameState.GAME_FALLING_BLOCKS:
                //Debug.Log("[3] Falling Blocks!");
                HandleFallingBlocks();
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
        UpdateGameState(GameState.GAME_GENERATE);
    }

    private void HandleGenerateGame()
    {
        match3Generate.GenerateBlocks(); // Generates First Blocks
        match3Generate.DeleteStartingCombos();
        match3Generate.CreateStartingCombos();        
        UpdateGameState(GameState.GAME_PLAYER_TURN);
    }

    [ContextMenu("AYUDA")]
    public void HandlePlayerTurn(GridNode currentNode)
    {
        match3Gameplay.ChainlinkClear();
        UpdateGameState(GameState.GAME_FALLING_BLOCKS);
    }

    private void HandleFallingBlocks()
    {        
        match3Falling.UpdateFallingBlocks();
        UpdateGameState(GameState.GAME_PLAYER_TURN);
    }
}
