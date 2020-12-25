using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This system emulates a 'fill resource over time' type system commonly found in games. It should
/// fill to a designated max, and send out all of the appropriate events based on its state. If you
/// want to use energy, you should request it through the Increase or Use methods. This class is meant
/// to be reusable, so that other systems like UI can listen in to the appropriate events, without creating
/// dependencies
/// </summary>
public class EnergySystem : MonoBehaviour
{
    public event Action StartedNewIncrement;    // when energy refill starts a new increment tick
    public event Action<int> ChangedEnergy;     // when energy is changed at all
    public event Action<int> UsedEnergy;        // when energy is used by command
    public event Action<int> IncreasedEnergy;   // when energy is increased by command
    public event Action DepletedEnergy;         // when energy first hits 0
    public event Action FilledEnergy;           // when energy first hits max
    public event Action<int> FailedAttemptedEnergyUse;    // tried to use energy, but not enough

    [Header("Energy")]
    [SerializeField] int _startEnergy = 0;
    [SerializeField] int _maxEnergy = 100;

    [Header("Fill")]
    [SerializeField] int _energyFillPerSecond = 10;
    [SerializeField] float _energyFillTimeIncrement = 2f;
    public int EnergyFillPerIncrement => (int)Math.Floor(_energyFillPerSecond * _energyFillTimeIncrement);
    public bool EnergyEmpty => _currentEnergy == 0;
    public bool EnergyFull => _currentEnergy == _maxEnergy;

    int _currentEnergy;
    public int CurrentEnergy
    {
        get => _currentEnergy;
        private set
        {
            value = Mathf.Clamp(value, 0, _maxEnergy);
            // check if our energy has changed
            if (value != _currentEnergy)
            {
                ChangedEnergy?.Invoke(value);
            }

            // if we were not at max, but new value puts us at max
            if(value == _maxEnergy && value != _currentEnergy)
            {
                FilledEnergy?.Invoke();
            }
            // if we were not depleted, but are just now put at 0
            else if(value == 0 && value != _currentEnergy)
            {
                DepletedEnergy?.Invoke();
            }
            _currentEnergy = value;
        }
    }

    float _timeSinceIncrement = 0;
    bool _isFillPaused = false;  // controls whether or not we should be counting/filling
    Coroutine _pauseFilleRoutine = null;

    private void OnEnable()
    {
        ResetIncrement();
    }

    void Update()
    {
        // check for pause each iteration
        if (_isFillPaused || EnergyFull)
            return;

        // increase elapsed time, check for fill
        _timeSinceIncrement += Time.deltaTime;
        // if we've hit the increment, do it and reset
        if(_timeSinceIncrement >= _energyFillTimeIncrement)
        {
            IncreaseEnergy(EnergyFillPerIncrement);
            ResetIncrement();
        }
    }

    public void IncreaseEnergy(int amount)
    {
        CurrentEnergy += amount;
    }

    public void UseEnergy(int amount)
    {
        // check if there's enough energy
        if(HasEnoughEnergy(amount) == false)
        {
            FailedAttemptedEnergyUse?.Invoke(CurrentEnergy);
            return;
        }
        // if so, use it
        CurrentEnergy -= amount;
    }

    public void PauseFill(float duration)
    {
        if (_pauseFilleRoutine != null)
            StopCoroutine(_pauseFilleRoutine);
        _pauseFilleRoutine = StartCoroutine(PauseFillRoutine(duration));
    }

    public bool HasEnoughEnergy(int amount)
    {
        if (amount < CurrentEnergy)
            return true;
        else
            return false;
    }

    void ResetIncrement()
    {
        _timeSinceIncrement = 0;
        StartedNewIncrement?.Invoke();
    }

    IEnumerator PauseFillRoutine(float duration)
    {
        _isFillPaused = true;
        yield return new WaitForSeconds(duration);
        _isFillPaused = false;
    }
}
