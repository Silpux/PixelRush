using System;
using UnityEngine;

public class PlayerGroundedZone : MonoBehaviour{

    // count ground blocks instead of just track trigger enter or exit,
    // because blocks of ground can intersect and it will lead to have flying state on ground
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
