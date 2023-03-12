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
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }

    private const string PlayerPrefsBindings = "InputBindings";

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PlayerPrefsBindings))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindings));
        }

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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            _playerInputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PlayerPrefsBindings, _playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}