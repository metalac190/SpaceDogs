using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class primarily serves as a 'HUB' for the major ship components. Both players and AI will talk
/// to this component, and it will receive this commands and send them out to relevant components.
/// This class also serves as a convenient entry point for detecting if a gameObject is a 'Ship', either Player or bot.
/// Basically... if it's specific to the ship, child objects with reference are fine. If it's generic, keep the component
/// on the Player root for GetComponent() searches, and optionally give it a field if you wish (for convenience).
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(EnergySystem))]
public class Ship : MonoBehaviour
{
    [SerializeField] ShipMovement _movement = null;
    public ShipMovement Movement => _movement;

    [SerializeField] ShipTricks _tricks = null;
    public ShipTricks Tricks => _tricks;

    [SerializeField] ShipAnimator _shipAnimator = null;
    public ShipAnimator ShipAnimator => _shipAnimator;

    [SerializeField] ShipShooting _shooting = null;
    public ShipShooting Shooting => _shooting;

    [SerializeField] Animator _animator = null;
    public Animator Animator => _animator;

    public void Shoot()
    {
        _shooting.Shoot();
    }

    public void ActivateSpecal()
    {
        Debug.Log("Activate special");
    }

    public void Turn(float turnAmount)
    {
        _movement.Turn(turnAmount);
    }

    public void Boost(bool requestBoost)
    {
        if (requestBoost)
        {
            _tricks.Boost();
        }
    }

    public void Brake(bool requestBrake)
    {
        if (requestBrake)
        {
            _tricks.Brake();
        }
    }
}
