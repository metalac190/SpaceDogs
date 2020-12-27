using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

public class ShipMovement : MonoBehaviour
{
    public event Action<float> SpeedChanged;
    public event Action<int> ChangedDirection;

    [Header("Dependencies")]
    [SerializeField] Ship _ship = null;

    [Header("General")]
    [SerializeField] Transform _thrusterLocation;

    [Header("Movement")]
    [SerializeField] float _startSpeed = 0;
    [SerializeField] float _baseSpeed = .2f;

    [SerializeField] float _accelerationRateInSec = 1f;
    [SerializeField] float _decelerationRateInSec = 1f;
    public float AccelRatePerSecond => (_baseSpeed / _accelerationRateInSec) * Time.fixedDeltaTime;
    public float DecelRatePerSecond => (_baseSpeed / _decelerationRateInSec) * Time.fixedDeltaTime;

    [Header("Rotation")]
    [SerializeField] float _turnSpeed = 100;

    public float BaseSpeed => _baseSpeed;
    bool _speedLock = false;
    Coroutine _speedLockRoutine = null;

    //float _maxSpeed;

    float _currentSpeed = 0;
    public float CurrentSpeed
    {
        get => _currentSpeed;   
        set
        {
            // check if our speed has changed
            if(value != _currentSpeed)
            {
                SpeedChanged?.Invoke(value);
            }
            _currentSpeed = value;
        }
    }

    Rigidbody _rb;
    Quaternion _requestedRotation = Quaternion.identity;

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

    private void Awake()
    {
        _rb = _ship.GetComponent<Rigidbody>();

        CurrentSpeed = _startSpeed;
    }

    private void FixedUpdate()
    {

        // calculate new forces
        ApplyMomentum();

        Vector3 newMovement = transform.forward * CurrentSpeed;
        Quaternion newTurn = _requestedRotation;

        //Debug.Log("CurrentSpeed: " + CurrentSpeed);
        ApplyMove(newMovement);
        ApplyTurn(newTurn);
    }

    private void ApplyMomentum()
    {
        if (_speedLock)
            return;

        // if we're above speed, decelerate
        if (CurrentSpeed > _baseSpeed)
        {
            CurrentSpeed -= DecelRatePerSecond;
            // clamp to avoid small fluctuations
            if (CurrentSpeed < _baseSpeed)
                CurrentSpeed = _baseSpeed;
        }
        // if we're below speed, accelerate
        else if (CurrentSpeed < _baseSpeed)
        {
            CurrentSpeed += AccelRatePerSecond;
            // clamp to avoid small fluctuations
            if (CurrentSpeed > _baseSpeed)
                CurrentSpeed = _baseSpeed;
        }
    }

    void ApplyMove(Vector3 moveOffset)
    {
        _rb.MovePosition(_rb.position + moveOffset);
    }

    void ApplyTurn(Quaternion turnOffset)
    {
        _rb.MoveRotation(_rb.rotation * turnOffset);
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

    // this method prevents accel/decel from being applied, temporarily
    public void LockSpeed(float lockDuration)
    {
        if (_speedLockRoutine != null)
            StopCoroutine(_speedLockRoutine);
        _speedLockRoutine = StartCoroutine(SpeedLockSequence(lockDuration));
    }

    IEnumerator SpeedLockSequence(float lockDuration)
    {
        _speedLock = true;

        yield return new WaitForSeconds(lockDuration);

        _speedLock = false;
    }
}
