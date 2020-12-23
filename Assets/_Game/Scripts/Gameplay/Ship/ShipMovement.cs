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
    public event Action StartedBoost;
    public event Action StoppedBoost;
    public event Action StartedBrake;
    public event Action StoppedBrake;

    [Header("General")]
    [SerializeField] Transform _thrusterLocation;

    [Header("Movement")]
    [SerializeField] float _startSpeed = 0;
    [SerializeField] float _baseSpeed = .2f;
    
    [SerializeField] float _accelerationRateInSec = 1f;
    [SerializeField] float _decelerationRateInSec = 1f;
    public float AccelRatePerSecond => (_baseSpeed / _accelerationRateInSec) * Time.fixedDeltaTime;
    public float DecelRatePerSecond => (_baseSpeed / _decelerationRateInSec) * Time.fixedDeltaTime;

    [Header("Boost")]
    [SerializeField] float _boostSpeedIncrease = .2f;
    [SerializeField] float _boostDuration = .5f;
    [SerializeField] float _boostAccelToMaxInSec = .2f;
    [SerializeField] VisualEffect _boostVFX = null;

    [Header("Brake")]
    [SerializeField] float _brakeSpeedDecrease = .15f;
    [SerializeField] float _brakeDuration = .5f;
    [SerializeField] float _brakeDecelInSec = .2f;
    [SerializeField] VisualEffect _brakeVFX = null;

    [Header("Rotation")]
    [SerializeField] float _turnSpeed = 3;

    //float _maxSpeed;

    float _currentSpeed = 0;
    public float CurrentSpeed
    {
        get => _currentSpeed;   
        private set
        {
            // check if our speed has changed
            if(value != _currentSpeed)
            {
                SpeedChanged?.Invoke(value);
            }
            _currentSpeed = value;
        }
    }
    public float BoostedMaxSpeed => _baseSpeed + _boostSpeedIncrease;

    Rigidbody _rb = null;
    Quaternion _requestedRotation = Quaternion.identity;

    public bool CanBrake { get; private set; } = true;
    public bool IsBraking { get; private set; } = false;
    Coroutine _brakeRoutine = null;

    public bool CanBoost { get; private set; } = true;
    public bool IsBoosting { get; private set; }
    Coroutine _boostRoutine = null;

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
        _rb = GetComponent<Rigidbody>();

        CurrentSpeed = _startSpeed;
    }

    private void FixedUpdate()
    {

        // calculate new forces
        ApplyMomentum();

        Vector3 newMovement = transform.forward * CurrentSpeed;
        Quaternion newTurn = _requestedRotation;

        Debug.Log("CurrentSpeed: " + CurrentSpeed);
        ApplyMove(newMovement);
        ApplyTurn(newTurn);
    }

    private void ApplyMomentum()
    {
        if (!IsBoosting && !IsBraking)
        {
            if (CurrentSpeed > _baseSpeed)
            {
                CurrentSpeed -= DecelRatePerSecond;
                // clamp to avoid small fluctuations
                if (CurrentSpeed < _baseSpeed)
                    CurrentSpeed = _baseSpeed;
            }
            else if (CurrentSpeed < _baseSpeed)
            {
                CurrentSpeed += AccelRatePerSecond;
                // clamp to avoid small fluctuations
                if (CurrentSpeed > _baseSpeed)
                    CurrentSpeed = _baseSpeed;
            }
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



    public void Boost()
    {
        // if we're already booosting, don't allow a new request
        if (IsBoosting)
            return;

        StartBoost();
    }

    private void StartBoost()
    {
        IsBoosting = true;

        _boostVFX?.Play();
        StartedBoost?.Invoke();
        
        // allow ability to cancel brake with a boost
        if (IsBraking)
        {
            StopBrake();
        }

        if (_boostRoutine != null)
        {
            StopCoroutine(_boostRoutine);
        }
        _boostRoutine = StartCoroutine(BoostRoutine());
    }

    IEnumerator BoostRoutine()
    {
        // increase speed to boost max
        float startSpeed = CurrentSpeed;
        
        // increase speed to max over time
        for (float elapsedTime = 0; elapsedTime <= _boostAccelToMaxInSec; elapsedTime += Time.deltaTime)
        {
            CurrentSpeed = Mathf.Lerp(startSpeed, BoostedMaxSpeed, elapsedTime / _boostAccelToMaxInSec);
            yield return null;
        }

        // wait out boost duration
        yield return new WaitForSeconds(_boostDuration);

        StopBoost();
    }

    private void StopBoost()
    {
        IsBoosting = false;

        _boostVFX.Stop();
        StoppedBoost?.Invoke();

        if(_boostRoutine != null)
        {
            StopCoroutine(_boostRoutine);
        }
    }

    public void Brake()
    {
        // if we're already braking, don't allow a new request
        if (IsBraking)
            return;

        StartBrake();
    }

    void StartBrake()
    {
        IsBraking = true;

        _brakeVFX?.Play();
        StartedBrake?.Invoke();
        // allow ability to cancel a boost with a brake
        if (IsBoosting)
        {
            StopBoost();
        }

        if (_brakeRoutine != null)
            StopCoroutine(_brakeRoutine);
        _brakeRoutine = StartCoroutine(BrakeRoutine());
    }

    IEnumerator BrakeRoutine()
    {
        // decrease speed to brake amount
        float startSpeed = CurrentSpeed;
        float targetSpeed = _baseSpeed - _brakeSpeedDecrease;
        // increase speed to max over time
        for (float elapsedTime = 0; elapsedTime <= _brakeDecelInSec; elapsedTime += Time.deltaTime)
        {
            CurrentSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / _brakeDecelInSec);
            yield return null;
        }

        // wait out boost duration
        yield return new WaitForSeconds(_brakeDuration);

        StopBrake();
    }

    void StopBrake()
    {
        IsBraking = false;

        _brakeVFX?.Stop();
        StoppedBrake?.Invoke();

        if (_brakeRoutine != null)
            StopCoroutine(_brakeRoutine);
    }
}
