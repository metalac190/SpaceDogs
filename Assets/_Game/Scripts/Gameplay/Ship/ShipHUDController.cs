using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowTargetPosition))]
public class ShipHUDController : MonoBehaviour
{
    [SerializeField] SimpleFillBar _healthBar = null;
    [SerializeField] SimpleFillBar _energyBar = null;
    [SerializeField] FollowTargetPosition _followTargetPosition = null;

    Transform _followTarget = null;
    HealthSystem _health = null;
    EnergySystem _energy = null;

    bool _started = false;

    public void Initialize(HealthSystem health, EnergySystem energy, Transform followTarget)
    {
        _health = health;
        _energy = energy;
        _followTarget = followTarget;

        if(_followTargetPosition != null)
            _followTargetPosition.SetTarget(_followTarget);
    }

    // because Awake and OnEnable get called before any other methods, we need to wait until we have
    // passed in the dependencies before subscribing to the listeners. We're just putting it off until
    // Start, even though it looks complicated.
    private void OnEnable()
    {
        if(_started)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Start()
    {
        if (!_started)
        {
            _started = true;
            Subscribe();
        }
    }

    void Subscribe()
    {
        _health.ChangedHealth += OnChangedHealth;
        _energy.ChangedEnergy += OnChangedEnergy;
    }

    void Unsubscribe()
    {
        _health.ChangedHealth -= OnChangedHealth;
        _energy.ChangedEnergy -= OnChangedEnergy;
    }

    void OnChangedHealth(int health)
    {
        _healthBar.SetScale(health, _health.MaxHealth);
    }

    void OnChangedEnergy(int energy)
    {
        _energyBar.SetScale(energy, _energy.MaxEnergy);
    }
}
