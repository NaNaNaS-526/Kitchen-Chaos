using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private ClearCounter _selectedCounter;

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
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

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact();
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
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }

            Debug.Log(_selectedCounter);
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        var playerTransform = transform;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y).normalized;
        transform.forward = Vector3.Slerp(playerTransform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.55f;
        float playerHeight = 2.0f;

        var playerPosition = playerTransform.position;
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

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this._selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,
            new OnSelectedCounterChangedEventArgs { SelectedCounter = _selectedCounter });
    }
}