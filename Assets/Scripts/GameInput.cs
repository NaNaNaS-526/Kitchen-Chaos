using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_OnPerformed;
    }

    private void InteractAlternate_OnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}