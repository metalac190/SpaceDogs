using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

public class ShipTricks : MonoBehaviour
{
    public event Action StartedBoost;
    public event Action StoppedBoost;
    public event Action StartedBrake;
    public event Action StoppedBrake;

    [Header("Dependencies")]
    [SerializeField] Ship _ship = null;

    [Header("General")]
    [SerializeField] int _trickCost = 1;
    public int TrickCost => _trickCost;

    [Header("Boost")]
    [SerializeField] float _boostSpeedIncrease = .2f;
    [SerializeField] float _boostDuration = .5f;
    [SerializeField] float _boostAccelToMaxInSec = .2f;

    [Header("Brake")]
    [SerializeField] float _brakeSpeedDecrease = .15f;
    [SerializeField] float _brakeDuration = .5f;
    [SerializeField] float _brakeDecelInSec = .2f;
 
    public bool CanBrake { get; private set; } = true;
    public bool IsBraking { get; private set; } = false;
    Coroutine _brakeRoutine = null;

    public bool CanBoost { get; private set; } = true;
    public bool IsBoosting { get; private set; }
    Coroutine _boostRoutine = null;

    ShipMovement _shipMovement = null;
    EnergySystem _energy = null;

    private void Awake()
    {
        _shipMovement = _ship.Movement;
        _energy = _ship.Energy;
    }

    public void Boost()
    {
        // if we're already booosting, don't allow a new request
        if (IsBoosting)
            return;
        // check if we have enough energy
        if (_energy.HasEnoughEnergy(TrickCost) == false)
            return;

        StartBoost();
    }

    private void StartBoost()
    {
        // allow ability to cancel brake with a boost
        if (IsBraking)
        {
            StopBrake();
        }

        IsBoosting = true;

        _energy.UseEnergy(TrickCost);
        _energy.PauseFill(_boostDuration);
        StartedBoost?.Invoke();

        if (_boostRoutine != null)
        {
            StopCoroutine(_boostRoutine);
        }
        _boostRoutine = StartCoroutine(BoostRoutine());
    }

    IEnumerator BoostRoutine()
    {
        // increase speed to boost max
        float startSpeed = _shipMovement.CurrentSpeed;
        float boostedMaxSpeed = _shipMovement.BaseSpeed + _boostSpeedIncrease;

        // increase speed to max over time
        for (float elapsedTime = 0; elapsedTime <= _boostAccelToMaxInSec; elapsedTime += Time.deltaTime)
        {
            _shipMovement.CurrentSpeed = Mathf.Lerp(startSpeed, 
                (_shipMovement.BaseSpeed + _boostSpeedIncrease), elapsedTime / _boostAccelToMaxInSec);
            yield return null;
        }

        // wait out boost duration
        _shipMovement.LockSpeed(_boostDuration);
        yield return new WaitForSeconds(_boostDuration);

        StopBoost();
    }

    private void StopBoost()
    {
        IsBoosting = false;

        _energy.ResumeFill();
        StoppedBoost?.Invoke();

        if (_boostRoutine != null)
        {
            StopCoroutine(_boostRoutine);
        }
    }

    public void Brake()
    {
        // if we're already braking, don't allow a new request
        if (IsBraking)
            return;
        if (_energy.HasEnoughEnergy(TrickCost) == false)
            return;

        StartBrake();
    }

    void StartBrake()
    {
        // allow ability to cancel a boost with a brake
        if (IsBoosting)
        {
            StopBoost();
        }

        IsBraking = true;

        _energy.UseEnergy(TrickCost);
        _energy.PauseFill(_brakeDuration);
        StartedBrake?.Invoke();

        if (_brakeRoutine != null)
            StopCoroutine(_brakeRoutine);
        _brakeRoutine = StartCoroutine(BrakeRoutine());
    }

    IEnumerator BrakeRoutine()
    {
        // decrease speed to brake amount
        float startSpeed = _shipMovement.CurrentSpeed;
        float targetSpeed = _shipMovement.BaseSpeed - _brakeSpeedDecrease;
        // increase speed to max over time
        for (float elapsedTime = 0; elapsedTime <= _brakeDecelInSec; elapsedTime += Time.deltaTime)
        {
            _shipMovement.CurrentSpeed = Mathf.Lerp(startSpeed, 
                (_shipMovement.BaseSpeed - _brakeSpeedDecrease), elapsedTime / _brakeDecelInSec);
            yield return null;
        }

        // wait out boost duration
        _shipMovement.LockSpeed(_boostDuration);
        yield return new WaitForSeconds(_brakeDuration);

        StopBrake();
    }

    void StopBrake()
    {
        IsBraking = false;

        _energy.ResumeFill();
        StoppedBrake?.Invoke();

        if (_brakeRoutine != null)
            StopCoroutine(_brakeRoutine);
    }
}
