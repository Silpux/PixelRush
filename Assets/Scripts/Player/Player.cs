using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private PlayerGroundedZone groundedZone;

    private Rigidbody rb;
    private float jumpForce = 8f;

    private float strafeSpeed = 5f;

    private bool strafingRight = false;

    private bool strafingLeft = false;

    private bool isJumping = false;

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

    private void Update(){
        if(strafingLeft){
            rb.linearVelocity = new Vector3(-strafeSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        if(strafingRight){
            rb.linearVelocity = new Vector3(strafeSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        if(isJumping && isGrounded){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            OnJump?.Invoke();
        }
    }

}
