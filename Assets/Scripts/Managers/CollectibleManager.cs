using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : Singleton<CollectibleManager>{

    private List<Coin> leftCoins = new();
    private List<Coin> collectedCoins = new();

    public int CollectedCoinsCount => collectedCoins.Count;
    public int TotalCoins => collectedCoins.Count + leftCoins.Count;

    public event Action<int, int> OnCoinsCountUpdate;

    public void RegisterCoin(Coin coin){
        leftCoins.Add(coin);
    }

    private void Start(){
        OnCoinsCountUpdate?.Invoke(0, leftCoins.Count);
    }

    public void CollectCoin(Coin coin){

        if(leftCoins.Contains(coin)){
            leftCoins.Remove(coin);
            collectedCoins.Add(coin);
            OnCoinsCountUpdate?.Invoke(collectedCoins.Count, collectedCoins.Count + leftCoins.Count);
        }

    }

}
