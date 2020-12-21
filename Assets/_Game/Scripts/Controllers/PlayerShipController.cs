using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is designed to interpret the player's input and send the relevant actions
/// to the functional components of the ship. This extra layer is to allow for BotAI to interpret
/// actions in place of player input in other ships.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(ShipShooting))]
public class PlayerShipController : MonoBehaviour
{
    ShipMovement _shipMovement = null;
    ShipShooting _shipShooting = null;

    private void Awake()
    {
        _shipMovement = GetComponent<ShipMovement>();
        _shipShooting = GetComponent<ShipShooting>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            _shipShooting.Shoot();
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

        _shipMovement.Turn(turnAmount);
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Start special");
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
            _shipMovement.Boost(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _shipMovement.Boost(false);
        }
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _shipMovement.Brake(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _shipMovement.Brake(false);
        }
    }
}
