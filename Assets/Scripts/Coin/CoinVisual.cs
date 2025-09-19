using UnityEngine;

public class CoinVisual : MonoBehaviour{

    private const string COLLECT_ANIMATION = "Collect";

    [SerializeField] private Coin coin;

    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        coin.OnCollect += Collect;
    }

    private void OnDisable(){
        coin.OnCollect -= Collect;
    }

    private void Collect(){
        animator.Play(COLLECT_ANIMATION);
    }

    public void OnAnimationFinish(){
        Destroy(coin.gameObject);
    }

}
