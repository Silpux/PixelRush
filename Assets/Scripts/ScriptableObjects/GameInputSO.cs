using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInputEventSO", menuName = "Scriptable Objects/GameInputEventSO")]
public class GameInputSO : ScriptableObject{

    public event Action<MoveDirection> OnMoveStart;
    public event Action<MoveDirection> OnMoveCancel;
    public event Action OnCancel;

    public void InvokeMoveStart(MoveDirection direction){
        OnMoveStart?.Invoke(direction);
    }

    public void InvokeMoveCancel(MoveDirection direction){
        OnMoveCancel?.Invoke(direction);
    }

    public void InvokeCancel(){
        OnCancel?.Invoke();
    }

}
