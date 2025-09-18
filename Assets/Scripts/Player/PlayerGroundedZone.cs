using System;
using UnityEngine;

public class PlayerGroundedZone : MonoBehaviour{

    public event Action<bool> OnGroundStateChanged;

    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent<Ground>(out _)){
            OnGroundStateChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.TryGetComponent<Ground>(out _)){
            OnGroundStateChanged?.Invoke(false);
        }
    }

}
