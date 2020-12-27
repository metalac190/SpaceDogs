using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimator : MonoBehaviour
{
    [SerializeField] Ship _ship = null;

    Animator _animator = null;
    ShipMovement _shipMovement = null;
    ShipTricks _shipTricks = null;

    // anim state names
    const string BoostStateName = "Boost";
    const string BrakeStateName = "Brake";

    // turning layer
    const string TurningLayerName = "Turning";
    const string IdleStateName = "Idle";
    const string TurnLeftStateName = "TurnLeft";
    const string TurnRightStateName = "TurnRight";
    int _turningLayerIndex;

    private void Awake()
    {
        _animator = _ship.Animator;
        _shipMovement = _ship.Movement;
        _shipTricks = _ship.Tricks;

        _turningLayerIndex = _animator.GetLayerIndex(TurningLayerName);
    }

    private void OnEnable()
    {
        _shipMovement.ChangedDirection += OnChangedDirection;
        _shipTricks.StartedBoost += OnStartedBoost;
        _shipTricks.StartedBrake += OnStartedBrake;
    }

    private void OnDisable()
    {
        _shipMovement.ChangedDirection -= OnChangedDirection;
        _shipTricks.StartedBoost -= OnStartedBoost;
        _shipTricks.StartedBrake -= OnStartedBrake;
    }

    void OnChangedDirection(int newDirection)
    {
        if(newDirection == -1)
        {
            _animator.CrossFadeInFixedTime(TurnLeftStateName, .3f, _turningLayerIndex);
        }
        else if(newDirection == 0)
        {
            _animator.CrossFadeInFixedTime(IdleStateName, .3f, _turningLayerIndex);
        }
        else if(newDirection == 1)
        {
            _animator.CrossFadeInFixedTime(TurnRightStateName, .3f, _turningLayerIndex);
        }
    }

    void OnStartedBoost()
    {
        _animator.CrossFadeInFixedTime(BoostStateName, .1f);
    }

    void OnStartedBrake()
    {

    }
}
