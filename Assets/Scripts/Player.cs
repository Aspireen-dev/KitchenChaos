using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    #region Singleton

    public static Player Instance { get; private set; }

    #endregion Singleton

    #region Events

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    #endregion Events

    #region SerializeFields

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    #endregion SerializeFields

    #region Properties

    public bool IsWalking { get; private set; }

    #endregion Properties

    #region Private fields

    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    #endregion Private fields

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        float interactionDistance = 2f;
        
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactionDistance, counterLayerMask))
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
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir, try only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0
                      && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X, try only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0
                          && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        IsWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        _selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() => _kitchenObjectHoldPoint;

    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject != null;
}
