using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>{

    [SerializeField] private Player player;

    private GameState currentState;
    private GameState CurrentState{
        get => currentState;
        set{
            if(currentState != value){
                currentState = value;
                OnGameStateChanged?.Invoke(currentState);
            }
        }
    }

    public event Action<GameState> OnGameStateChanged;

    private void OnEnable(){
        StartCoroutine(ONEnable());
        player.OnLose += Lose;
    }

    private IEnumerator ONEnable(){
        while(UIManager.Instance == null){
            yield return null;
        }
        UIManager.Instance.OnRestart += Restart;
        UIManager.Instance.OnPaused += Pause;
        UIManager.Instance.OnContinue += Continue;
    }

    private void OnDisable(){
        UIManager.Instance.OnRestart -= Restart;
        UIManager.Instance.OnPaused -= Pause;
        UIManager.Instance.OnContinue -= Continue;
        player.OnLose -= Lose;
    }

    private void Lose(){
        UnityEngine.Debug.Log("GameStateManager: Lose");
    }

    private void Restart(){
        UnityEngine.Debug.Log("GameStateManager: Restart");
    }

    private void Pause(){
        UnityEngine.Debug.Log("GameStateManager: Pause");
    }

    private void Continue(){
        UnityEngine.Debug.Log("GameStateManager: Continue");
    }


}
