using System;
using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour{

    public event Action OnCollect;

    [SerializeField] private GameObject collectParticles;

    private void Start(){
        CollectibleManager.Instance.RegisterCoin(this);
    }

    private void OnTriggerEnter(Collider other){

        if(other.TryGetComponent(out Player player)){

            OnCollect?.Invoke();
            CollectibleManager.Instance.CollectCoin(this);
            Destroy(Instantiate(collectParticles, transform.position, Quaternion.identity), 1f);

        }

    }

}
