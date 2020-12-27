using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

public class HealthSystem : MonoBehaviour, IDamageable, IHealable
{
    public event Action<int> ChangedHealth;
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action Killed;

    [SerializeField] bool _infiniteHealth = false;
    [SerializeField] int _startingHealth = 10;
    [SerializeField] int _maxHealth = 10;

    int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            value = Mathf.Clamp(value, 0, _maxHealth);
            if (value != _currentHealth)
            {
                ChangedHealth?.Invoke(value);
            }
            _currentHealth = value;
        }
    }
    public int MaxHealth => _maxHealth;

    void Awake()
    {
        CurrentHealth = _startingHealth;
    }

    public void Heal(int amount)
    {
        Healed?.Invoke(amount);
    }

    public void TakeDamage(int amount)
    {
        if(_infiniteHealth == false)
        {
            CurrentHealth -= amount;
            Damaged?.Invoke(amount);
        }

        if (CurrentHealth <= 0 && _infiniteHealth == false)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Killed?.Invoke();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        if (audioClip == null)
            return;
        //TODO replace with Object Pooling
        AudioHelper.PlayClip2D(audioClip, 1);
    }
}
