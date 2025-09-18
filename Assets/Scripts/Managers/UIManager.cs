using System;
using System.Collections;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

    public event Action OnRestart;
    public event Action OnPaused;
    public event Action OnContinue;

    private void OnEnable(){
        StartCoroutine(ONEnable());
    }

    private IEnumerator ONEnable(){
        while(GameInput.Instance == null){
            yield return null;
        }
        GameInput.Instance.OnCancel += Cancel;
    }

    private void OnDisable(){
        GameInput.Instance.OnCancel -= Cancel;
    }

    public void Restart(){
        OnRestart?.Invoke();
    }

    public void Pause(){
        OnPaused?.Invoke();
    }

    public void Continue(){
        OnContinue?.Invoke();
    }

    private void Cancel(){
        Debug.Log("UIManager: Cancel");
        Pause();
    }

}
