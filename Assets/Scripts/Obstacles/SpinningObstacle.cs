using UnityEngine;

public class SpinningObstacle : MonoBehaviour{

    [SerializeField] private float spinSpeed = 5f;

    private void Update(){
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

}
