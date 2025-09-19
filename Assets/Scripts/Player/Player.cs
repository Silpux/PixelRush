using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private PlayerGroundedZone groundedZone;

    [SerializeField] private float jumpForce = 8f;
    private float runningSpeed = 10f;

    [SerializeField] private float crouchDuration = 0.5f;

    [SerializeField] private float strafeSpeed = 5f;

    private Rigidbody rb;
    
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
                    OnCrouch?.Invoke();
                }
                else{
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
    }

    private void OnDisable(){
        groundedZone.OnGroundStateChanged -= SetGroundedState;
        GameInput.Instance.OnMoveStart -= MoveStart;
        GameInput.Instance.OnMoveCancel -= MoveCancel;
    }

    private void SetGroundedState(bool newState){
        if(isGrounded != newState){
            isGrounded = newState;
            if(newState){
                OnLand?.Invoke();
            }
        }
    }

    private void MoveStart(MoveDirection direction){
        Debug.Log($"Player: Move {direction}");

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
        Debug.LogWarning($"Move cancel: {direction}");
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
