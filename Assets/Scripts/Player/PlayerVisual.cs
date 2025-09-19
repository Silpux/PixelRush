using UnityEngine;

public class PlayerVisual : MonoBehaviour{

    private const string DEATH_TRIGGER = "DeathTrigger";
    private const string DEATH_ANIMATION = "Death";
    private const string RUN_TRIGGER = "RunTrigger";
    private const string CROUCH_TRIGGER = "CrouchTrigger";
    private const string IDLE_TRIGGER = "IdleTrigger";

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
        animator.Play(DEATH_ANIMATION);
    }
    private void PlayCrouch(){
        animator.ResetTrigger(RUN_TRIGGER);
        animator.SetTrigger(CROUCH_TRIGGER);
    }
    private void PlayRun(){
        animator.ResetTrigger(IDLE_TRIGGER);
        animator.SetTrigger(RUN_TRIGGER);
    }

    private void PlayIdle(){
        animator.ResetTrigger(RUN_TRIGGER);
        animator.SetTrigger(IDLE_TRIGGER);
    }

    private void PlayJump(){
        animator.Play(JUMP_ANIMATION);
    }
    private void PlayLanding(){
        animator.Play(LAND_ANIMATION);
    }

}
