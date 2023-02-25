using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public enum GameState
{
    PlayState,
    WinState,
    LoseState,
    CoteState,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.PlayState);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PlayState:
                HandlePlayState();
                break;
            case GameState.WinState:
                HandleWinState();
                break;
            case GameState.LoseState:
                HandleLoseState();
                break;
            case GameState.CoteState:
                HandleCoteState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleCoteState()
    {
        
    }

    private void HandleLoseState()
    {
        Debug.Log("You lose");
    }

    private void HandleWinState()
    {
        Debug.Log("You win");
    }

    private void HandlePlayState()
    {
        if (/*suspicion max*/)
        {
            UpdateGameState(GameState.LoseState);
        }

        if (/*main quest complete*/)
        {
            UpdateGameState(GameState.WinState);
        }
    }
}

