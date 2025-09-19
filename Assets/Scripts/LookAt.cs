using UnityEngine;

public class LookAt : MonoBehaviour{

    [SerializeField] private Transform target;

    private void Start(){
        transform.forward = target.forward;
    }

}
