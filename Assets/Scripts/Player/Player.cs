using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private PlayerGroundedZone groundedZone;
    [SerializeField] private GameInputSO inputEvent;
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private BoxCollider headBoxCollider;

    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float crouchDuration = 0.5f;
    [SerializeField] private float strafeSpeed = 5f;
    [SerializeField] private float crouchGravityBoost = 3f;

    private Rigidbody rb;
    private Coroutine currentCrouchRoutine;

    private bool isGrounded;
    private bool isMoving = false;
    private bool isDead = false;
    
    private bool strafingRight = false;
    private bool strafingLeft = false;

    private bool isCrouching = false;
    private bool IsCrouching{
        get => isCrouching;
        set{
            if(isCrouching != value && isMoving){
                isCrouching = value;
 
                if(isCrouching){
                    headBoxCollider.enabled = false;
                    OnCrouch?.Invoke();
                }
                else{
                    headBoxCollider.enabled = true;
                    OnRun?.Invoke();
                }
            }
        }
    }

    public event Action OnIdle;
    public event Action OnRun;
    public event Action OnJump;
    public event Action OnLand;
    public event Action OnCrouch;

    public event Action OnFinish;
    public event Action OnLose;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        OnIdle?.Invoke();
    }

    private void OnEnable(){
        groundedZone.OnGroundStateChanged += SetGroundedState;
        inputEvent.OnMoveStart += MoveStart;
        inputEvent.OnMoveCancel += MoveCancel;
        gameStateSO.OnGameStateChanged += ChangeState;

    }

    private void OnDisable(){
        groundedZone.OnGroundStateChanged -= SetGroundedState;
        inputEvent.OnMoveStart -= MoveStart;
        inputEvent.OnMoveCancel -= MoveCancel;
        gameStateSO.OnGameStateChanged -= ChangeState;
    }

    private void ChangeState(GameState state){
        if(state == GameState.Running){
            isMoving = true;
            OnRun?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.TryGetComponent<KillZone>(out _)){
            OnLose?.Invoke();
            isDead = true;
        }
        else if(other.gameObject.TryGetComponent<Finish>(out _)){
            OnFinish?.Invoke();
            OnIdle?.Invoke();
            isMoving = false;
        }
    }

    private void SetGroundedState(bool newState){
        if(isMoving && !isDead){
            if(isGrounded != newState){
                if(newState){
                    OnLand?.Invoke();
                }
            }
        }
        isGrounded = newState;
    }

    private void MoveStart(MoveDirection direction){

        if(isDead){
            return;
        }

        switch(direction){
            case MoveDirection.Left:
                strafingRight = false;
                strafingLeft = true;
                break;
            case MoveDirection.Right:
                strafingLeft = false;
                strafingRight = true;
                break;
            case MoveDirection.Up:
                DoJump();
                break;
            case MoveDirection.Down:
                currentCrouchRoutine = StartCoroutine(CrouchRoutine());
                break;
        }

    }

    private void MoveCancel(MoveDirection direction){

        if(isDead){
            return;
        }

        switch(direction){
            case MoveDirection.Left:
                strafingLeft = false;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
                break;
            case MoveDirection.Right:
                strafingRight = false;
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
                break;
        }
    }

    private void DoJump(){
        if(isMoving && isGrounded){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            if(IsCrouching){
                IsCrouching = false;
                if(currentCrouchRoutine != null){
                    StopCoroutine(currentCrouchRoutine);
                }
                currentCrouchRoutine = null;

            }
            OnJump?.Invoke();
        }
    }

    private IEnumerator CrouchRoutine(){
        IsCrouching = true;
        yield return new WaitForSeconds(crouchDuration);
        IsCrouching = false;
        currentCrouchRoutine = null;
    }

    private void FixedUpdate(){

        if(isDead || !isMoving){
            rb.linearVelocity = new Vector3(0,rb.linearVelocity.y,0);
            return;
        }

        Vector3 newLinearVelocity = rb.linearVelocity;

        if(strafingLeft){
            newLinearVelocity = new Vector3(-strafeSpeed, newLinearVelocity.y, newLinearVelocity.z);
        }
        if(strafingRight){
            newLinearVelocity = new Vector3(strafeSpeed, newLinearVelocity.y, newLinearVelocity.z);
        }

        if(!isGrounded){
            if(IsCrouching){
                newLinearVelocity = new Vector3(newLinearVelocity.x, newLinearVelocity.y - crouchGravityBoost, newLinearVelocity.z);
            }
        }

        newLinearVelocity = new Vector3(newLinearVelocity.x, newLinearVelocity.y, runningSpeed);

        rb.linearVelocity = newLinearVelocity;

    }

}
