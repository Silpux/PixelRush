using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameCoinsSO", menuName = "Scriptable Objects/GameCoinsSO")]
public class GameCoinsSO : ScriptableObject{

    public event Action<int, int> OnCoinsCountUpdate;

    private List<Coin> leftCoins = new();
    private List<Coin> collectedCoins = new();

    public int CollectedCoinsCount => collectedCoins.Count;
    public int TotalCoins => collectedCoins.Count + leftCoins.Count;

    // each coin calls it and passes itself as parameter
    public void RegisterCoin(Coin coin){
        leftCoins.Add(coin);
    }

    public void Reset(){
        leftCoins.Clear();
        collectedCoins.Clear();
    }

    public void CollectCoin(Coin coin){

        if(leftCoins.Contains(coin)){
            leftCoins.Remove(coin);
            collectedCoins.Add(coin);
            OnCoinsCountUpdate?.Invoke(collectedCoins.Count, collectedCoins.Count + leftCoins.Count);
        }

    }

}
