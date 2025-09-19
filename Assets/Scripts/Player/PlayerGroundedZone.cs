using System;
using UnityEngine;

public class PlayerGroundedZone : MonoBehaviour{

    private int groundCounter = 0;

    public event Action<bool> OnGroundStateChanged;

    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent<Ground>(out _)){
            groundCounter++;
            OnGroundStateChanged?.Invoke(groundCounter > 0);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.TryGetComponent<Ground>(out _)){
            groundCounter--;
            OnGroundStateChanged?.Invoke(groundCounter > 0);
        }
    }

}
