using UnityEngine;

public class PlayerVisual : MonoBehaviour{

    private const string DEATH_TRIGGER = "DeathTrigger";
    private const string RUN_TRIGGER = "RunTrigger";
    private const string CROUCH_TRIGGER = "CrouchTrigger";
    private const string IS_IDLE = "IsIdle";

    private const string JUMP_ANIMATION = "Jump";
    private const string LAND_ANIMATION = "Landing";

    [SerializeField] private Player player; 

    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        player.OnLose += PlayLose;
        player.OnCrouch += PlayCrouch;
        player.OnRun += PlayRun; 
        player.OnIdle += PlayIdle;
        player.OnJump += PlayJump;
        player.OnLand += PlayLanding;
    }

    private void OnDisable(){
        player.OnLose -= PlayLose;
        player.OnCrouch -= PlayCrouch;
        player.OnRun -= PlayRun;
        player.OnIdle -= PlayIdle;
        player.OnJump -= PlayJump;
        player.OnLand -= PlayLanding;
    }

    private void PlayLose(){
        animator.SetTrigger(DEATH_TRIGGER);
    }
    private void PlayCrouch(){
        animator.SetTrigger(CROUCH_TRIGGER);
    }
    private void PlayRun(){
        animator.SetBool(IS_IDLE, false);
        animator.SetTrigger(RUN_TRIGGER);
    }

    private void PlayIdle(){
        animator.SetBool(IS_IDLE, true);
    }

    private void PlayJump(){
        animator.Play(JUMP_ANIMATION);
    }
    private void PlayLanding(){
        animator.Play(LAND_ANIMATION);
    }

}
