using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : Singleton<CollectibleManager>{

    private static List<Coin> coins = new();

    public void RegisterCoin(Coin coin){
        coins.Add(coin);
    }

    public void CollectCoin(Coin coin){

        if(coins.Contains(coin)){
            
        }

    }

}
