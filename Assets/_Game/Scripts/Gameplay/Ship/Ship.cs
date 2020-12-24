using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class primarily serves as a 'HUB' for the major ship components. Both players and AI will talk
/// to this component, and it will receive this commands and send them out to relevant components.
/// This class also serves as a convenient entry point for detecting if a gameObject is a 'Ship', either Player or bot
/// </summary>
[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(ShipShooting))]
[RequireComponent(typeof(ShipTricks))]
public class Ship : MonoBehaviour
{
    ShipMovement _shipMovement = null;
    ShipShooting _shipShooting = null;
    ShipTricks _shipTricks = null;

    private void Awake()
    {
        _shipMovement = GetComponent<ShipMovement>();
        _shipShooting = GetComponent<ShipShooting>();
        _shipTricks = GetComponent<ShipTricks>();
    }

    public void Shoot()
    {
        _shipShooting.Shoot();
    }

    public void ActivateSpecal()
    {
        Debug.Log("Activate special");
    }

    public void Turn(float turnAmount)
    {
        _shipMovement.Turn(turnAmount);
    }

    public void Boost(bool requestBoost)
    {
        if (requestBoost)
        {
            _shipTricks.Boost();
        }
    }

    public void Brake(bool requestBrake)
    {
        if (requestBrake)
        {
            _shipTricks.Brake();
        }
    }
}
