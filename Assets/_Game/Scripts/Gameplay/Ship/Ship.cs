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
public class Ship : MonoBehaviour
{
    ShipMovement _shipMovement = null;
    ShipShooting _shipShooting = null;

    private void Awake()
    {
        _shipMovement = GetComponent<ShipMovement>();
        _shipShooting = GetComponent<ShipShooting>();
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

    public void Boost(bool shouldBoost)
    {
        _shipMovement.Boost(shouldBoost);
    }

    public void Brake(bool shouldBrake)
    {
        _shipMovement.Brake(shouldBrake);
    }
}
