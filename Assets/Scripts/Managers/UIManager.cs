using System;
using System.Collections;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

    public event Action OnStart;
    public event Action OnRestart;
    public event Action OnPaused;
    public event Action OnContinue;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject lostPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject pausePanel;

    private void OnEnable(){
        StartCoroutine(ONEnable());
    }

    private IEnumerator ONEnable(){
        while(GameInput.Instance == null){
            yield return null;
        }
        GameInput.Instance.OnCancel += Cancel;
        while(GameStateManager.Instance == null){
            yield return null;
        }
        GameStateManager.Instance.OnGameStateChanged += ChangeState;
    }

    private void ChangeState(GameState gameState){
        switch(gameState){
            case GameState.Lost:
                StartCoroutine(ShowLostPanel());
                break;
            case GameState.Finished:
                StartCoroutine(ShowWinPanel());
                break;
            case GameState.Paused:
                pausePanel.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator ShowLostPanel(){
        yield return new WaitForSeconds(2f);
        lostPanel.gameObject.SetActive(true);
    }
    private IEnumerator ShowWinPanel(){
        yield return new WaitForSeconds(2f);
        winPanel.gameObject.SetActive(true);
    }

    private void OnDisable(){
        GameInput.Instance.OnCancel -= Cancel;
    }

    public void OnPressStart(){
        startPanel.gameObject.SetActive(false);
        OnStart?.Invoke();
    }

    public void Restart(){
        OnRestart?.Invoke();
    }

    public void Pause(){
        OnPaused?.Invoke();
        pausePanel.gameObject.SetActive(true);
    }

    public void Continue(){
        OnContinue?.Invoke();
        pausePanel.gameObject.SetActive(false);
    }

    public void Quit(){
        Application.Quit();
    }

    private void Cancel(){
        Debug.Log("UIManager: Cancel");
        Pause();
    }

}
