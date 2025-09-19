using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    protected override void Awake(){
        base.Awake();
        CurrentState = GameState.WaitingForStart;
    }

    private void OnEnable(){
        StartCoroutine(ONEnable());
        player.OnLose += Lose;
        player.OnFinish += Finish;
    }

    private IEnumerator ONEnable(){
        while(UIManager.Instance == null){
            yield return null;
        }
        UIManager.Instance.OnStart += StartRun;
        UIManager.Instance.OnRestart += Restart;
        UIManager.Instance.OnPaused += Pause;
        UIManager.Instance.OnContinue += Continue;
    }

    private void OnDisable(){
        UIManager.Instance.OnStart -= StartRun;
        UIManager.Instance.OnRestart -= Restart;
        UIManager.Instance.OnPaused -= Pause;
        UIManager.Instance.OnContinue -= Continue;
        player.OnLose -= Lose;
        player.OnFinish -= Finish;
    }

    private void StartRun(){
        CurrentState = GameState.Running;
    }
    private void Lose(){
        CurrentState = GameState.Lost;
    }

    private void Finish(){
        CurrentState = GameState.Finished;
    }

    private void Restart(){
        SceneManager.LoadScene(0);
    }

    private void Pause(){
        Time.timeScale = 0f;
    }

    private void Continue(){
        Time.timeScale = 1f;
    }


}
