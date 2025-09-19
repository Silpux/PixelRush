using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameUISO", menuName = "Scriptable Objects/GameUISO")]
public class GameUISO : ScriptableObject{

    public event Action OnStart;
    public event Action OnRestart;
    public event Action OnPaused;
    public event Action OnContinue;

    public void InvokeStart(){
        OnStart?.Invoke();
    }

    public void InvokeRestart(){
        OnRestart?.Invoke();
    }

    public void InvokePaused(){
        OnPaused?.Invoke();
    }

    public void InvokeContinue(){
        OnContinue?.Invoke();
    }
}
