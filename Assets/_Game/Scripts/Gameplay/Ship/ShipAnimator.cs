using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class ShipAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator = null;

    ShipMovement _shipMovement = null;

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
        if(_animator == null)
        {
            Debug.LogWarning("Not animator assigned to ship! " + gameObject.name);
            this.enabled = false;
        }
        _shipMovement = GetComponent<ShipMovement>();

        _turningLayerIndex = _animator.GetLayerIndex(TurningLayerName);
    }

    private void OnEnable()
    {
        _shipMovement.ChangedDirection += OnChangedDirection;
        _shipMovement.StartedBoost += OnStartedBoost;
        _shipMovement.StartedBrake += OnStartedBrake;
    }

    private void OnDisable()
    {
        _shipMovement.ChangedDirection -= OnChangedDirection;
        _shipMovement.StartedBoost -= OnStartedBoost;
        _shipMovement.StartedBrake -= OnStartedBrake;
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
