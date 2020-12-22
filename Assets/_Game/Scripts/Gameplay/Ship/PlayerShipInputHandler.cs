using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is designed to interpret the player's input and send the relevant actions
/// to the ship. This extra layer is to allow for BotAI to interpret
/// actions in place of player input in other ships, while keeping a consisent 'Ship' for behaviors
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Ship))]
public class PlayerShipInputHandler : MonoBehaviour
{
    Ship _ship = null;

    private void Awake()
    {
        _ship = GetComponent<Ship>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            _ship.Shoot();
            Debug.Log("Start weapon");
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Release weapon");
        }
    }
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        movementInput.Normalize();

        float turnAmount = movementInput.x;

        _ship.Turn(turnAmount);
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _ship.ActivateSpecal();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Release special");
        }
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _ship.Boost(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _ship.Boost(false);
        }
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _ship.Brake(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _ship.Brake(false);
        }
    }
}
