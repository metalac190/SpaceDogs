using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public event Action<float> SpeedChanged;
    public event Action<int> ChangedDirection;
    public event Action<bool> Boosted;
    public event Action<bool> Braked;

    [Header("Movement")]
    [SerializeField] float _startSpeed = 6;
    [SerializeField] float _maxSpeed = 6;
    [SerializeField] Animator _animator = null;

    [Header("Boost")]
    [SerializeField] float _boostSpeedIncrease = 6;
    [SerializeField] float _boostAccelToMaxInSec = 1;
    [SerializeField] float _boostDecelToNormalInSec = 1;
    [SerializeField] VisualEffect _boostVFX = null;

    [Header("Brake")]
    [SerializeField] float _brakeSpeedDecrease = 6;
    [SerializeField] float _brakeDecelToZeroInSec = 1;
    [SerializeField] float _brakeAccelToNormalInSec = 1;
    [SerializeField] VisualEffect _brakeVFX = null;

    [Header("Rotation")]
    [SerializeField] float _turnSpeed = 3;

    [Header("Abilities")]
    [SerializeField] float _brakeToZeroInSec = .5f;
    [SerializeField] float _boostSpeed = 3;

    [SerializeField] Transform _thrusterLocation;

    float _currentSpeed = 0;
    public float CurrentSpeed
    {
        get => _currentSpeed;   
        private set
        {
            // ensure we don't EVER exceed max speed
            value = Mathf.Clamp(value, 0, _maxSpeed);
            // check if our speed has changed
            if(value != _currentSpeed)
            {
                SpeedChanged?.Invoke(value);
            }
            _currentSpeed = value;
        }
    }

    Rigidbody _rb = null;
    Vector3 _requestedMovement;
    Quaternion _requestedRotation = Quaternion.identity;

    bool _isBraking = false;
    public bool IsBraking
    {
        get => _isBraking;
        private set
        {
            if(value != _isBraking)
            {
                Debug.Log("Braked: " + value);
                Braked?.Invoke(value);
            }
            _isBraking = value;
        }
    }

    bool _isBoosting = false;
    public bool IsBoosting
    {
        get => _isBoosting;
        private set
        {
            if(value != _isBoosting)
            {
                Debug.Log("Boosted: " + value);
                Boosted?.Invoke(value);
            }
            _isBoosting = value;
        }
    }

    int _turnDirection = 0; // -1 for left, 0 for neutral, 1 for right
    public int TurnDirection 
    {
        get => _turnDirection;
        private set
        {
            value = Mathf.Clamp(value, -1, 1);
            if(value != _turnDirection)
            {
                ChangedDirection?.Invoke(value);
            }
            _turnDirection = value;
        }
    } 

    // returns speed as a fraction of the max
    float CurrentMomentumRatio => (1 / _maxSpeed) * _currentSpeed;

    float BoostAccelRatePerSecond => (_maxSpeed / _boostAccelToMaxInSec) * Time.fixedDeltaTime;
    float BoostDecelRatePerSecond => (_maxSpeed / _boostDecelToNormalInSec) * Time.fixedDeltaTime;

    float BrakeDecelRatePerSecond => (_maxSpeed / _brakeToZeroInSec) * Time.fixedDeltaTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // calculate new forces
        Vector3 newMovement = transform.forward * _maxSpeed * Time.fixedDeltaTime;
        Quaternion newTurn = _requestedRotation;

        newMovement = ApplyAccelDecel(newMovement);

        ApplyMove(newMovement);
        ApplyTurn(newTurn);
    }

    public void Turn(float turnAmount)
    {
        DetermineTurnDirection(turnAmount);

        float turnOffset = turnAmount * _turnSpeed * Time.fixedDeltaTime;
        _requestedRotation = Quaternion.Euler(0, turnOffset, 0);
    }

    private void DetermineTurnDirection(float turnAmount)
    {
        if (turnAmount == 0)
        {
            TurnDirection = 0;
        }
        else if(turnAmount < 0)
        {
            TurnDirection = -1;
        }
        else if(turnAmount > 0)
        {
            TurnDirection = 1;
        }
    }

    public void Brake(bool requestBrake)
    {
        // ensure we don't brake again if we're already braking
        if (requestBrake == IsBraking)
            return;

        IsBraking = requestBrake;

        if (IsBraking)
        {
            PlayBrakeVFX();
            _maxSpeed -= _brakeSpeedDecrease;
        }
        else
        {
            _maxSpeed += _brakeSpeedDecrease;
        }
    }

    public void Boost(bool requestBoost)
    {
        if (requestBoost == IsBoosting)
            return;

        IsBoosting = requestBoost;

        if (IsBoosting)
        {
            PlayBoostVFX();
            _maxSpeed += _boostSpeedIncrease;
        }
        else
        {
            _maxSpeed -= _boostSpeedIncrease;
        }
    }

    Vector3 ApplyAccelDecel(Vector3 newMovement)
    {
        // if we're trying to move, accelerate
        if (_currentSpeed != _maxSpeed && _isBoosting)
        {
            _currentSpeed += BoostAccelRatePerSecond;
        }
        else if(_currentSpeed != _maxSpeed && _isBraking)
        {
            //_currentSpeed 
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);

        return newMovement;
    }

    void ApplyMove(Vector3 moveOffset)
    {
         _rb.MovePosition(_rb.position + moveOffset);
    }

    void ApplyTurn(Quaternion turnOffset)
    {
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    void PlayBrakeVFX()
    {
        _brakeVFX.Play();
    }

    void PlayBoostVFX()
    {
        _boostVFX.Play();
    }
}
