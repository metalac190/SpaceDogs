using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class ShipAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator = null;

    ShipMovement _shipMovement = null;

    const string IdleState = "Idle";
    const string BoostState = "Boost";
    const string BrakeState = "Brake";
    const string TurnLeftState = "TurnLeft";
    const string TurnRightState = "TurnRight";

    private void Awake()
    {
        if(_animator == null)
        {
            Debug.LogWarning("Not animator assigned to ship! " + gameObject.name);
            this.enabled = false;
        }
        _shipMovement = GetComponent<ShipMovement>();
    }

    private void OnEnable()
    {
        _shipMovement.ChangedDirection += OnChangedDirection;
        _shipMovement.Boosted += OnBoosted;
        _shipMovement.Braked += OnBraked;
    }

    private void OnDisable()
    {
        _shipMovement.ChangedDirection -= OnChangedDirection;
        _shipMovement.Boosted -= OnBoosted;
        _shipMovement.Braked -= OnBraked;
    }

    void OnChangedDirection(int newDirection)
    {
        if(newDirection == -1)
        {
            _animator.CrossFadeInFixedTime(TurnLeftState, .3f);
        }
        else if(newDirection == 0)
        {
            _animator.CrossFadeInFixedTime(IdleState, .3f);
        }
        else if(newDirection == 1)
        {
            _animator.CrossFadeInFixedTime(TurnRightState, .3f);
        }
    }

    void OnBoosted(bool isBoosting)
    {
        Debug.Log("Boost Animation");
    }

    void OnBraked(bool isBraking)
    {
        Debug.Log("Brake Animation");
    }
}
