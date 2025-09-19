using UnityEngine;

public class Follow : MonoBehaviour{

    [SerializeField] private Transform target;

    private Vector3 difference;

    private void Start(){
        difference = target.position - transform.position;
    }

    private void LateUpdate(){
        transform.position = target.position - difference;
    }

}
