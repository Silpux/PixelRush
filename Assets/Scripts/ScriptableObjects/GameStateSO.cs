using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateSO", menuName = "Scriptable Objects/GameStateSO")]
public class GameStateSO : ScriptableObject{

    public event Action<GameState> OnGameStateChanged;

    public void InvokeGameStateChange(GameState gameState){
        OnGameStateChanged?.Invoke(gameState);
    }

}
