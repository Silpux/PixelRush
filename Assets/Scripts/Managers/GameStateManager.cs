using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour{

    [SerializeField] private Player player;
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private GameUISO gameUISO;

    private GameState currentState;
    private GameState CurrentState{
        get => currentState;
        set{
            if(currentState != value){
                currentState = value;
                gameStateSO.InvokeGameStateChange(currentState);
            }
        }
    }

    private void Awake(){
        CurrentState = GameState.WaitingForStart;
    }

    private void OnEnable(){
        gameUISO.OnStart += StartRun;
        gameUISO.OnRestart += Restart;
        gameUISO.OnPaused += Pause;
        gameUISO.OnContinue += Continue;
        player.OnLose += Lose;
        player.OnFinish += Finish;
    }

    private void OnDisable(){
        gameUISO.OnStart -= StartRun;
        gameUISO.OnRestart -= Restart;
        gameUISO.OnPaused -= Pause;
        gameUISO.OnContinue -= Continue;
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
