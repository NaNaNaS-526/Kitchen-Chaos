using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_OnPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_OnPerformed;
        _playerInputActions.Player.Pause.performed += Pause_OnPerformed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= Interact_OnPerformed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_OnPerformed;
        _playerInputActions.Player.Pause.performed -= Pause_OnPerformed;
        _playerInputActions.Dispose();
    }

    private void Pause_OnPerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_OnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_OnPerformed(InputAction.CallbackContext ctx)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}