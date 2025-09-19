using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private PlayerGroundedZone groundedZone;

    [SerializeField] private float jumpForce = 8f;
    private float runningSpeed = 10f;

    [SerializeField] private float crouchDuration = 0.5f;

    [SerializeField] private float strafeSpeed = 5f;

    [SerializeField] private BoxCollider headBoxCollider;

    private Rigidbody rb;

    private bool isMoving = false;
    private bool isDead = false;
    
    private bool strafingRight = false;
    private bool strafingLeft = false;

    private bool isJumping = false;
    private bool isCrouching = false;
    private bool IsCrouching{
        get => isCrouching;
        set{
            if(isCrouching != value){
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

    private Coroutine currentCrouchRoutine;

    private float crouchGravityBoost = 3f;

    private bool isGrounded;

    public event Action OnIdle;
    public event Action OnRun;
    public event Action OnJump;
    public event Action OnLand;
    public event Action OnCrouch;

    public event Action OnLose;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        OnIdle?.Invoke();
    }

    private void OnEnable(){
        groundedZone.OnGroundStateChanged += SetGroundedState;
        StartCoroutine(ONEnable());
    }

    private IEnumerator ONEnable(){
        while(GameInput.Instance == null){
            yield return null;
        }
        GameInput.Instance.OnMoveStart += MoveStart;
        GameInput.Instance.OnMoveCancel += MoveCancel;
        while(GameStateManager.Instance == null){
            yield return null;
        }
        GameStateManager.Instance.OnGameStateChanged += ChangeState;

    }

    private void OnDisable(){
        groundedZone.OnGroundStateChanged -= SetGroundedState;
        GameInput.Instance.OnMoveStart -= MoveStart;
        GameInput.Instance.OnMoveCancel -= MoveCancel;
    }

    private void ChangeState(GameState state){
        if(state == GameState.Running){
            isMoving = true;
            OnRun?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.TryGetComponent<KillZone>(out _)){
            Debug.Log("Death");
            OnLose?.Invoke();
            isDead = true;
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
                isJumping = true;
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
            case MoveDirection.Up:
                isJumping = false;
                break;
        }
    }

    private IEnumerator CrouchRoutine(){
        IsCrouching = true;
        yield return new WaitForSeconds(crouchDuration);
        IsCrouching = false;
        currentCrouchRoutine = null;
    }

    private void FixedUpdate(){

        if(isDead){
            rb.linearVelocity = new Vector3(0,rb.linearVelocity.y,0);
            return;
        }

        if(!isMoving){
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 newLinearVelocity = rb.linearVelocity;

        if(strafingLeft){
            newLinearVelocity = new Vector3(-strafeSpeed, newLinearVelocity.y, newLinearVelocity.z);
        }
        if(strafingRight){
            newLinearVelocity = new Vector3(strafeSpeed, newLinearVelocity.y, newLinearVelocity.z);
        }

        if(isGrounded){
            if(isJumping){
                newLinearVelocity = new Vector3(newLinearVelocity.x, jumpForce, newLinearVelocity.z);
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
        else{
            if(IsCrouching){
                newLinearVelocity = new Vector3(newLinearVelocity.x, newLinearVelocity.y - crouchGravityBoost, newLinearVelocity.z);
            }
        }

        newLinearVelocity = new Vector3(newLinearVelocity.x, newLinearVelocity.y, runningSpeed);

        rb.linearVelocity = newLinearVelocity;

    }

}
