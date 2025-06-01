using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader/Player")]
public class PlayerInputReader : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
{
    public event Action JumpEvent;
    public event Action InteractEvent;
    public event Action<Vector2> MouseMoveEvent;
    public event Action InventoryEvent;
    public event Action<int> WeaponSwapEvent;
    public event Action AttackEvent;
    public event Action MapEvent;
    public event Action PauseEvent;

    private Controls _controls;
    private bool isShip;
    private bool isInput;
    
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.UI.SetCallbacks(this);
        }
        _controls.Player.Enable();
        _controls.UI.Enable();
    }

    public void OnOffInput(bool isTrue)
    {
        if (isTrue)
        {
            _controls.Player.Enable();
        }
        else
        {
            _controls.Player.Disable();
        }
    }

    public void InputCheck()
    {
        if (GameManager.Instance.isShip)
            ShipManager.Instance.ShipInputOnOff(isInput);
        else
            OnOffInput(isInput);
        isInput = !isInput;
    }

    #region PlayerInput
    public Vector2 PlayerMoveDir { get; private set; }
    public bool IsRunPressed { get; private set; }
    public bool IsSwimPressed { get; private set; }
    public bool IsAimPressed { get; private set; }
    public void OnMovement(InputAction.CallbackContext context)
    {
        PlayerMoveDir = context.ReadValue<Vector2>();
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        if(context.performed)
            MouseMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
            JumpEvent?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        IsRunPressed = context.performed;
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.performed)
            InteractEvent?.Invoke();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.performed)
            InventoryEvent?.Invoke();
    }

    public void OnWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int weaponIndex = int.Parse(context.control.name);
            WeaponSwapEvent?.Invoke(weaponIndex);
        }
    }

    public void OnSwim(InputAction.CallbackContext context)
    {
        IsSwimPressed = context.performed;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackEvent?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        IsAimPressed = context.performed;
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (context.performed)
            MapEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            PauseEvent?.Invoke();
    }

    #endregion
}
