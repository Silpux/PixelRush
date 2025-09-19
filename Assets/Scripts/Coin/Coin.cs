using System;
using UnityEngine;

public class Coin : MonoBehaviour{

    public event Action OnCollect;

    [SerializeField] private GameCoinsSO gameCoinsSO;
    [SerializeField] private GameObject collectParticles;

    private void Start(){
        gameCoinsSO.RegisterCoin(this);
    }

    private void OnTriggerEnter(Collider other){

        if(other.TryGetComponent(out Player player)){

            OnCollect?.Invoke();
            gameCoinsSO.CollectCoin(this);
            Destroy(Instantiate(collectParticles, transform.position, Quaternion.identity), 1f);

        }

    }

}
