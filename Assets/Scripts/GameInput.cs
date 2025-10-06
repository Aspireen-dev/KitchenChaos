using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    public PlayerInputActions PlayerInputActions { get; private set; }

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Player.Enable();

        PlayerInputActions.Player.Interact.performed += Interact_Performed;
        PlayerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
    }

    private void Interact_Performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_Performed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() => PlayerInputActions.Player.Move.ReadValue<Vector2>();
}
