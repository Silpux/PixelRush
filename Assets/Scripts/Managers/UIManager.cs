using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

    public event Action OnStart;
    public event Action OnRestart;
    public event Action OnPaused;
    public event Action OnContinue;

    public event Action<MoveDirection> OnMoveStart;
    public event Action<MoveDirection> OnMoveCancel;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject lostPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TextMeshProUGUI collectedCoinsText;
    [SerializeField] private TextMeshProUGUI collectedCoinsWinPanelText;
    [SerializeField] private TextMeshProUGUI collectedCoinsLosePanelText;

    [SerializeField] private SwipeEventTrigger swipeEventTrigger;

    private void OnEnable(){
        StartCoroutine(ONEnable());
        swipeEventTrigger.OnSwipeUp += MakeJump;
        swipeEventTrigger.OnSwipeDown += MakeCrouch;
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
        while(CollectibleManager.Instance == null){
            yield return null;
        }
        CollectibleManager.Instance.OnCoinsCountUpdate += SetCollectedCoinsText;
    }
    private void OnDisable(){
        GameInput.Instance.OnCancel -= Cancel;
        GameStateManager.Instance.OnGameStateChanged -= ChangeState;
        CollectibleManager.Instance.OnCoinsCountUpdate -= SetCollectedCoinsText;
        swipeEventTrigger.OnSwipeUp -= MakeJump;
        swipeEventTrigger.OnSwipeDown -= MakeCrouch;
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

    public void MakeJump(){
        OnMoveStart?.Invoke(MoveDirection.Up);
    }

    public void MakeCrouch(){
        OnMoveStart?.Invoke(MoveDirection.Down);
    }

    public void MoveRightClick(){
        OnMoveStart?.Invoke(MoveDirection.Right);
    }

    public void MoveLeftClick(){
        OnMoveStart?.Invoke(MoveDirection.Left);
    }
    public void MoveRightRelease(){
        OnMoveCancel?.Invoke(MoveDirection.Right);
    }

    public void MoveLeftRelease(){
        OnMoveCancel?.Invoke(MoveDirection.Left);
    }

    private void SetCollectedCoinsText(int collected, int total){
        collectedCoinsText.text = $"{collected} / {total}";
    }

    private IEnumerator ShowLostPanel(){
        yield return new WaitForSeconds(2f);
        collectedCoinsLosePanelText.text = $"Coins collected: {CollectibleManager.Instance.CollectedCoinsCount} / {CollectibleManager.Instance.TotalCoins}";
        lostPanel.gameObject.SetActive(true);
    }
    private IEnumerator ShowWinPanel(){
        yield return new WaitForSeconds(2f);
        collectedCoinsWinPanelText.text = $"Coins collected: {CollectibleManager.Instance.CollectedCoinsCount} / {CollectibleManager.Instance.TotalCoins}";
        winPanel.gameObject.SetActive(true);
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
