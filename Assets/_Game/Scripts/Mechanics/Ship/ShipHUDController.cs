using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowTargetPosition))]
public class ShipHUDController : MonoBehaviour
{
    [SerializeField] Ship _ship = null;
    [SerializeField] SimpleFillBar _healthBar = null;
    [SerializeField] IconBar _energyBar = null;

    HealthSystem _health = null;
    EnergySystem _energy = null;

    private void Awake()
    {
        _health = _ship.Health;
        _energy = _ship.Energy;

        // initialize HUD display
        _healthBar.SetScale(_health.CurrentHealth, _health.MaxHealth);
        _energyBar.SetMaxIcons(_energy.MaxEnergy);
        _energyBar.FillIcons(_energy.CurrentEnergy);
    }

    // because Awake and OnEnable get called before any other methods, we need to wait until we have
    // passed in the dependencies before subscribing to the listeners. We're just putting it off until
    // Start, even though it looks complicated.
    private void OnEnable()
    {
        _health.ChangedHealth += OnChangedHealth;
        _energy.ChangedEnergy += OnChangedEnergy;
        _energy.ChangedMaxEnergy += OnChangedMaxEnergy;
    }

    private void OnDisable()
    {
        _health.ChangedHealth -= OnChangedHealth;
        _energy.ChangedEnergy -= OnChangedEnergy;
        _energy.ChangedMaxEnergy -= OnChangedMaxEnergy;
    }

    void OnChangedHealth(int health)
    {
        _healthBar.SetScale(health, _health.MaxHealth);
    }

    void OnChangedEnergy(int energy)
    {
        _energyBar.FillIcons(energy);
    }

    void OnChangedMaxEnergy(int newMax)
    {
        if(newMax <= 0)
        {
            _energyBar.SetMaxIcons(0);
        }
        else
        {
            _energyBar.SetMaxIcons(newMax);
        }
    }
}
