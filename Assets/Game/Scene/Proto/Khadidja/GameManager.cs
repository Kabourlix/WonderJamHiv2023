using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        throw new NotImplementedException();
    }

    private void HandleLoseState()
    {
        throw new NotImplementedException();
    }

    private void HandleWinState()
    {
        throw new NotImplementedException();
    }

    private void HandlePlayState()
    {
        throw new NotImplementedException();
    }
}

