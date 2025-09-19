using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : Singleton<GameInput>{

    private static InputSystem_Actions actions;
    public static InputSystem_Actions Actions => actions ??= new();

    public event Action<MoveDirection> OnMoveStart;
    public event Action<MoveDirection> OnMoveCancel;
    public event Action OnCancel;

    protected override void Awake(){
        base.Awake();
        EnablePlayerMap();
    }

    private void OnEnable(){
        Actions.Player.StrafeLeft.performed += RaiseStrafeLeft;
        Actions.Player.StrafeRight.performed += RaiseStrafeRight;
        Actions.Player.Jump.performed += RaiseJump;
        Actions.Player.Crouch.performed += RaiseCrouch;
        Actions.Player.StrafeLeft.canceled += RaiseStrafeLeft;
        Actions.Player.StrafeRight.canceled += RaiseStrafeRight;
        Actions.Player.Jump.canceled += RaiseJump;
        Actions.Player.Crouch.canceled += RaiseCrouch;

        Actions.Player.Cancel.performed += RaiseCancelEvent;
        Actions.UI.Cancel.performed += RaiseCancelEvent;
        // because other singleton's Awake may not be called yet
        StartCoroutine(ONEnable());
    }

    private IEnumerator ONEnable(){
        while(GameStateManager.Instance == null){
            yield return null;
        }
        GameStateManager.Instance.OnGameStateChanged += ChangeState;
        while(UIManager.Instance == null){
            yield return null;
        }
        UIManager.Instance.OnMoveStart += UIMoveStart;
        UIManager.Instance.OnMoveCancel += UIMoveCancel;
    }

    private void OnDisable(){
        Actions.Player.StrafeLeft.performed -= RaiseStrafeLeft;
        Actions.Player.StrafeRight.performed -= RaiseStrafeRight;
        Actions.Player.Jump.performed -= RaiseJump;
        Actions.Player.Crouch.performed -= RaiseCrouch;
        Actions.Player.StrafeLeft.canceled -= RaiseStrafeLeft;
        Actions.Player.StrafeRight.canceled -= RaiseStrafeRight;
        Actions.Player.Jump.canceled -= RaiseJump;
        Actions.Player.Crouch.canceled -= RaiseCrouch;

        Actions.Player.Cancel.performed -= RaiseCancelEvent;
        Actions.UI.Cancel.performed -= RaiseCancelEvent;

        GameStateManager.Instance.OnGameStateChanged -= ChangeState;
        UIManager.Instance.OnMoveCancel -= UIMoveCancel;

    }

    private void UIMoveStart(MoveDirection direction){
        OnMoveStart?.Invoke(direction);
    }

    private void UIMoveCancel(MoveDirection direction){
        OnMoveCancel?.Invoke(direction);
    }

    private void ChangeState(GameState state){
        Debug.Log($"GameInput: Change state: {state}");
    }

    public void EnablePlayerMap(){
        Actions.UI.Disable();
        Actions.Player.Enable();
    }

    public void EnableUIMap(){
        Actions.Player.Disable();
        Actions.UI.Enable();
    }

    private void RaiseCancelEvent(InputAction.CallbackContext ctx){
        OnCancel?.Invoke();
    }

    private void RaiseJump(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            OnMoveStart?.Invoke(MoveDirection.Up);
        }
        else{
            OnMoveCancel?.Invoke(MoveDirection.Up);
        }
    }

    private void RaiseCrouch(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            OnMoveStart?.Invoke(MoveDirection.Down);
        }
        else{
            OnMoveCancel?.Invoke(MoveDirection.Down);
        }
    }

    private void RaiseStrafeRight(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            OnMoveStart?.Invoke(MoveDirection.Right);
        }
        else{
            OnMoveCancel?.Invoke(MoveDirection.Right);
        }
    }

    private void RaiseStrafeLeft(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            OnMoveStart?.Invoke(MoveDirection.Left);
        }
        else{
            OnMoveCancel?.Invoke(MoveDirection.Left);
        }
    }
}
