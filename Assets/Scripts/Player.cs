using System;
using Counters;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y).normalized;
        if (moveDirection != Vector3.zero) _lastInteractDirection = moveDirection;

        float interactDistance = 2.0f;
        if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance,
                countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        var playerTransform = transform;
        var playerPosition = playerTransform.position;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y).normalized;
        transform.forward = Vector3.Slerp(playerTransform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.55f;
        float playerHeight = 2.0f;

        bool canMove = !Physics.CapsuleCast(playerPosition, playerPosition + Vector3.up * playerHeight,
            playerRadius, moveDirection, moveDistance);
        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0.0f, 0.0f).normalized;
            canMove = !Physics.CapsuleCast(playerPosition, playerPosition + Vector3.up * playerHeight,
                playerRadius, moveDirectionX, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0.0f, 0.0f, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(playerPosition, playerPosition + Vector3.up * playerHeight,
                    playerRadius, moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        _isWalking = moveDirection != Vector3.zero;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,
            new OnSelectedCounterChangedEventArgs { SelectedCounter = _selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
        if (kitchenObject)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject;
    }
}