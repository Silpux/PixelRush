using UnityEngine;

public class CollectibleManager : MonoBehaviour{
    [SerializeField] private GameCoinsSO gameCoinsSO;

    private void Awake(){
        gameCoinsSO.Reset();
    }
}
