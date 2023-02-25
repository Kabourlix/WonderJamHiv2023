using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Inputs;
using Game.Scripts;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public enum GameState
{
    PlayState,
    EndLevel,
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
        foreach (var link in transitions.Transitions)
        {
            _linksDict.Add(link.DestinationState, link.ParentStates);
        }
    }

    #endregion

    #region Serialized properties and their public accessors

    [SerializeField] private StateTransitions transitions ;
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
        var old = CurrentState;
        CurrentState = newState;
        switch (newState)
        {
            case GameState.PlayState:
                HandlePlayState(old);
                break;
            case GameState.WinState:
                HandleWinState(old);
                break;
            case GameState.GameOverState:
                HandleGameOverState(old);
                break;
            case GameState.CaughtState:
                HandleCaughtState(old);
                break;
            case GameState.EvolveState:
                HandleEvolveState(old);
                break;
            case GameState.PauseState:
                HandlePauseState(old);
                break;
            case GameState.EndLevel:
                HandleEndLevelState(old);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleEndLevelState(GameState old)
    {
        //Check if the player has fully evolved or not
        //If not, go to game over state
        //If yes, go to win state
        ChangeState(player.GetComponent<XPManager>().IsFullyEvolved 
            ? GameState.WinState 
            : GameState.GameOverState);
    }

    #region State

    private void HandlePauseState(GameState oldState)
    {
        Time.timeScale = 0;
        hud.ShowPause();
        InputManager.Instance.EnableControls(false);
    }

    private void HandleEvolveState(GameState oldState)
    {
        //Nothing really
    }

    private void HandleCaughtState(GameState oldState)
    {
        var hpPercentage = stats.CaughtSuspicious();
        if(hud is not null) hud.UpdateSuspiciousBar(hpPercentage);
        if(hpPercentage <= 0)
            ChangeState(GameState.GameOverState);
        
    }

    private void HandleGameOverState(GameState oldState)
    {
        if (hud is not null) hud.ShowGameOver();
        //Disable controls or switch to ui controls 
        InputManager.Instance.EnableControls(false);
    }

    private void HandleWinState(GameState oldState)
    {
        if(hud is not null) hud.ShowWin();
        //Disable controls or switch to ui controls
        InputManager.Instance.EnableControls(false);
    }

    private void HandlePlayState(GameState oldState)
    {
        if(oldState == GameState.CaughtState) //Respawn
            player.position = stats.SpawnPosition.position;
        
        Time.timeScale = 1;
        //Enable controls
        InputManager.Instance.EnableControls(true);
        
    }

    #endregion
}

[Serializable]
public struct StateTransitionsStruct
{
    public GameState DestinationState;
    public GameState[] ParentStates;
}

