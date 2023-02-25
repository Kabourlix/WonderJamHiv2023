using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public enum GameState
{
    PlayState,
    WinState,
    EvolveState,
    PauseState,
    GameOverState,
    CaughtState,
}

public class GameManager : MonoBehaviour
{
    #region Singleton declaration

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
        //Instantiate the dictionary
        _linksDict = new Dictionary<GameState, GameState[]>();
        foreach (var link in links)
        {
            _linksDict.Add(link.DestinationState, link.ParentStates);
        }
    }

    #endregion

    #region Serialized properties and their public accessors

    [SerializeField] private GameStateLink[] links;
    private Dictionary<GameState, GameState[]> _linksDict;
    
    [SerializeField] private Transform player;
    public Transform Player => player;

    [SerializeField] private PlayerStats stats;
    
    [SerializeField] private HUDManager hud;

    #endregion
    
    
    public GameState CurrentState { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        ChangeState(GameState.PlayState);
    }

    public void ChangeState(GameState newState)
    {
        if(_linksDict[newState].Contains(CurrentState) == false) //Prevent forbidden transitions
            throw new Exception($"Cannot transition from {CurrentState} to {newState}.");
        
        CurrentState = newState;
        switch (newState)
        {
            case GameState.PlayState:
                HandlePlayState();
                break;
            case GameState.WinState:
                HandleWinState();
                break;
            case GameState.GameOverState:
                HandleGameOverState();
                break;
            case GameState.CaughtState:
                HandleCaughtState();
                break;
            case GameState.EvolveState:
                HandleEvolveState();
                break;
            case GameState.PauseState:
                HandlePauseState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    #region State

    private void HandlePauseState()
    {
        Time.timeScale = 0;
        //Hide Common UI
        //Display the pause menu
        //Switch controls to pause menu
    }

    private void HandleEvolveState()
    {
        //Nothing really
    }

    private void HandleCaughtState()
    {
        var hpPercentage = stats.CaughtSuspicious();
        if(hud is not null) hud.UpdateSuspiciousBar(hpPercentage);
        
    }

    private void HandleGameOverState()
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

    #endregion
}

[Serializable]
public struct GameStateLink
{
    public GameState DestinationState;
    public GameState[] ParentStates;
}

