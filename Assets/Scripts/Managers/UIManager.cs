using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour{

    [SerializeField] private GameInputSO gameInputSO;
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private GameUISO gameUISO;
    [SerializeField] private GameCoinsSO gameCoinsSO;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject lostPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TextMeshProUGUI collectedCoinsText;
    [SerializeField] private TextMeshProUGUI collectedCoinsWinPanelText;
    [SerializeField] private TextMeshProUGUI collectedCoinsLosePanelText;

    [SerializeField] private SwipeEventTrigger swipeEventTrigger;

    private void OnEnable(){
        swipeEventTrigger.OnSwipeUp += MakeJump;
        swipeEventTrigger.OnSwipeDown += MakeCrouch;
        gameInputSO.OnCancel += Cancel;
        gameStateSO.OnGameStateChanged += ChangeState;
        gameCoinsSO.OnCoinsCountUpdate += SetCollectedCoinsText;
    }

    private void OnDisable(){
        gameInputSO.OnCancel -= Cancel;
        gameStateSO.OnGameStateChanged -= ChangeState;
        gameCoinsSO.OnCoinsCountUpdate -= SetCollectedCoinsText;
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
        gameInputSO.InvokeMoveStart(MoveDirection.Up);
    }

    public void MakeCrouch(){
        gameInputSO.InvokeMoveStart(MoveDirection.Down);
    }

    public void MoveRightClick(){
        gameInputSO.InvokeMoveStart(MoveDirection.Right);
    }

    public void MoveLeftClick(){
        gameInputSO.InvokeMoveStart(MoveDirection.Left);
    }

    public void MoveRightRelease(){
        gameInputSO.InvokeMoveCancel(MoveDirection.Right);
    }

    public void MoveLeftRelease(){
        gameInputSO.InvokeMoveCancel(MoveDirection.Left);
    }

    private void SetCollectedCoinsText(int collected, int total){
        collectedCoinsText.text = $"{collected}";
    }

    private IEnumerator ShowLostPanel(){
        yield return new WaitForSeconds(2f);
        collectedCoinsLosePanelText.text = $"Coins collected:\n{gameCoinsSO.CollectedCoinsCount} / {gameCoinsSO.TotalCoins}";
        lostPanel.gameObject.SetActive(true);
    }
    private IEnumerator ShowWinPanel(){
        yield return new WaitForSeconds(2f);
        collectedCoinsWinPanelText.text = $"Coins collected:\n{gameCoinsSO.CollectedCoinsCount} / {gameCoinsSO.TotalCoins}";
        winPanel.gameObject.SetActive(true);
    }

    public void OnPressStart(){
        startPanel.gameObject.SetActive(false);
        gameUISO.InvokeStart();
    }

    public void Restart(){
        gameUISO.InvokeRestart();
    }

    public void Pause(){
        gameUISO.InvokePaused();
        pausePanel.gameObject.SetActive(true);
    }

    public void Continue(){
        gameUISO.InvokeContinue();
        pausePanel.gameObject.SetActive(false);
    }

    public void Quit(){
        Application.Quit();
    }

    private void Cancel(){
        Pause();
    }

}
