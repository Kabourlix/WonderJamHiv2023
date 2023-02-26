using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inputs;
using Game.Scripts;
using Game.Scripts.UI;
using UnityEngine;


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
        
        _playerController = player.GetComponent<PlayerController>();
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
    private PlayerController _playerController;
    private void Start()
    {
        PlayerController.OnEvolveEnd += () => ChangeState(player.GetComponent<XPManager>().IsFullyEvolved ? GameState.WinState : GameState.PlayState);
        ChangeState(GameState.PlayState);
        
    }

    private void Update()
    {

        #region Win/Lose Debug
        //pause
        if(Input.GetKeyDown(KeyCode.P))
            ChangeState(GameState.EndLevel);
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("pause");
            hud.ShowPause();
        }
        //win good
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("win good");
            if(hud is not null) hud.ShowWinGood();
        }
        //win evil
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("win evil");
            if(hud is not null) hud.ShowWinEvil();
        }
        //lose
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("lose");
            if(hud is not null) hud.ShowGameOver();
        }

        #endregion    
        
        //TODO - change state conditions
        //if()
    }

    public void ChangeState(GameState newState)
    {
        if(_linksDict[newState].Contains(CurrentState) == false) //Prevent forbidden transitions
            throw new Exception($"Cannot transition from {CurrentState} to {newState}.");
        ExitState(CurrentState); 
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
    }
    
    private void ExitState(GameState old)
    {
        switch (old)
        {
            case GameState.PlayState:
                
                break;
            case GameState.WinState:
                
                break;
            case GameState.GameOverState:
                
                break;
            case GameState.CaughtState:
                
                break;
            case GameState.EvolveState:
                //Unfreeze enemies
                //Input already dealt with
                break;
            case GameState.PauseState:
                
                break;
            case GameState.EndLevel:
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(old), old, null);
        }
    }

    private void HandleEndLevelState(GameState old)
    {
        //Check if the player has fully evolved or not
        //If not, go to game over state
        //If yes, go to win state
        ChangeState(player.GetComponent<XPManager>().IsFullyEvolved 
            ? GameState.EvolveState 
            : GameState.GameOverState);
    }

    #region Handlers

    private void HandlePauseState(GameState oldState)
    {
        Time.timeScale = 0;
        hud.ShowPause();
        InputManager.Instance.EnableControls(false);
    }

    private void HandleEvolveState(GameState oldState)
    {
        InputManager.Instance.EnableControls(false);
        Player.GetComponent<PlayerController>().EvolveBegin();
        //Nothing really
        //Pause all IA Logic.
    }

    private void HandleCaughtState(GameState oldState)
    {
        var hpPercentage = stats.CaughtSuspicious();
        if(hud is not null) hud.UpdateSuspiciousBar(hpPercentage);
        if(hpPercentage <= 0)
            ChangeState(GameState.GameOverState);
        player.GetComponent<PlayerController>().IsSuspect = false;
        ChangeState(GameState.PlayState); // Respawn

    }

    private void HandleGameOverState(GameState oldState)
    {
        //Disable controls or switch to ui controls 
        InputManager.Instance.EnableControls(false);
        if (hud is not null) hud.ShowGameOver();
    }

    private void HandleWinState(GameState oldState)
    {
        Player.GetComponent<PlayerController>().EvolveBegin();
        //Disable controls or switch to ui controls
        InputManager.Instance.EnableControls(false);
        //TODO : Determiner si c'est une victoire good ou evil
        if (hud is null) return;
        if(player.GetComponent<XPManager>().IsFullyEvolved)
            hud.ShowWinEvil();
        else
            hud.ShowWinGood();
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

