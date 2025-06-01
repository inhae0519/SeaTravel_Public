using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader/Ship")]
public class ShipInputReader : ScriptableObject, Controls.IShipActions
{
    public event Action exitEvent;
    public event Action mapEvent;
    public event Action<Vector2> mouseDeltaEvent; 
    public event Action PauseEvent;

    private Controls _controls;
    private bool isInput;
    
    public float ZInput { get; private set; }
    public float XInput { get; private set; }
    
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Ship.SetCallbacks(this);
        }
    }
    
    public void OnOffInput(bool isTrue)
    {
        if (isTrue)
        {
            _controls.Ship.Enable();

        }
        else
        {
            _controls.Ship.Disable();
        }
    }

    public bool GetEnable() => _controls.Ship.enabled;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        ZInput = context.ReadValue<float>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        XInput = context.ReadValue<float>();
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
            exitEvent?.Invoke();
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        if(context.performed)
            mouseDeltaEvent?.Invoke(context.ReadValue<Vector2>());
    }
}