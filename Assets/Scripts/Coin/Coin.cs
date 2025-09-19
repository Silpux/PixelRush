using System;
using UnityEngine;

public class Coin : MonoBehaviour{

    public event Action OnCollect;

    private void Start(){
        CollectibleManager.Instance.RegisterCoin(this);
    }

    private void OnTriggerEnter(Collider other){

        if(other.TryGetComponent(out Player player)){

            OnCollect?.Invoke();
            CollectibleManager.Instance.CollectCoin(this);

        }

    }

}
