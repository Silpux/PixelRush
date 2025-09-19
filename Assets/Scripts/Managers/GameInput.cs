using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{

    private InputSystem_Actions actions;
    private InputSystem_Actions Actions => actions ??= new();

    [SerializeField] private GameInputSO gameInputSO;

    private void Awake(){
        EnablePlayerMap();
    }

    private void OnEnable(){
        Actions.Player.StrafeLeft.performed += InvokeStrafeLeft;
        Actions.Player.StrafeRight.performed += InvokeStrafeRight;
        Actions.Player.Jump.performed += InvokeJump;
        Actions.Player.Crouch.performed += InvokeCrouch;
        Actions.Player.StrafeLeft.canceled += InvokeStrafeLeft;
        Actions.Player.StrafeRight.canceled += InvokeStrafeRight;
        Actions.Player.Jump.canceled += InvokeJump;
        Actions.Player.Crouch.canceled += InvokeCrouch;
        Actions.Player.Cancel.performed += InvokeCancelEvent;
        Actions.UI.Cancel.performed += InvokeCancelEvent;
    }

    private void OnDisable(){
        Actions.Player.StrafeLeft.performed -= InvokeStrafeLeft;
        Actions.Player.StrafeRight.performed -= InvokeStrafeRight;
        Actions.Player.Jump.performed -= InvokeJump;
        Actions.Player.Crouch.performed -= InvokeCrouch;
        Actions.Player.StrafeLeft.canceled -= InvokeStrafeLeft;
        Actions.Player.StrafeRight.canceled -= InvokeStrafeRight;
        Actions.Player.Jump.canceled -= InvokeJump;
        Actions.Player.Crouch.canceled -= InvokeCrouch;
        Actions.Player.Cancel.performed -= InvokeCancelEvent;
        Actions.UI.Cancel.performed -= InvokeCancelEvent;
        DisableActions();
    }

    public void EnablePlayerMap(){
        Actions.UI.Disable();
        Actions.Player.Enable();
    }

    public void EnableUIMap(){
        Actions.Player.Disable();
        Actions.UI.Enable();
    }

    private void DisableActions(){
        Actions.Player.Disable();
        Actions.UI.Disable();
    }

    private void InvokeCancelEvent(InputAction.CallbackContext ctx){
        gameInputSO.InvokeCancel();
    }

    private void InvokeJump(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            gameInputSO.InvokeMoveStart(MoveDirection.Up);
        }
        else{
            gameInputSO.InvokeMoveCancel(MoveDirection.Up);
        }
    }

    private void InvokeCrouch(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            gameInputSO.InvokeMoveStart(MoveDirection.Down);
        }
        else{
            gameInputSO.InvokeMoveCancel(MoveDirection.Down);
        }
    }

    private void InvokeStrafeRight(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            gameInputSO.InvokeMoveStart(MoveDirection.Right);
        }
        else{
            gameInputSO.InvokeMoveCancel(MoveDirection.Right);
        }
    }

    private void InvokeStrafeLeft(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Performed){
            gameInputSO.InvokeMoveStart(MoveDirection.Left);
        }
        else{
            gameInputSO.InvokeMoveCancel(MoveDirection.Left);
        }
    }
}
