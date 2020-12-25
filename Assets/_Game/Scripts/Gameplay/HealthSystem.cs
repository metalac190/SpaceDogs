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

    [Header("Feedback")]
    [SerializeField] AudioClip _healSFX;
    [SerializeField] VisualEffect _healVFX;

    [SerializeField] AudioClip _damageSFX;
    [SerializeField] VisualEffect _damageVFX;

    [SerializeField] AudioClip _deathSFX;
    [SerializeField] VisualEffect _deathVFX;

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
        Debug.Log("Heal: " + amount);
        Healed?.Invoke(amount);

        PlayVFX(_healVFX);
        PlaySFX(_healSFX);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Damage: " + amount);
        if(_infiniteHealth == false)
        {
            CurrentHealth -= amount;
            Damaged?.Invoke(amount);
        }

        PlayVFX(_damageVFX);
        PlaySFX(_damageSFX);

        if (CurrentHealth <= 0 && _infiniteHealth == false)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("Killed");
        Killed?.Invoke();

        PlayVFX(_deathVFX);
        PlaySFX(_deathSFX);
    }

    public void PlayVFX(VisualEffect vfxPrefab)
    {
        VisualEffect newPickupVFX = Instantiate
            (vfxPrefab, transform.position, transform.rotation);

        newPickupVFX.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        //TODO replace with Object Pooling
        AudioHelper.PlayClip2D(audioClip, 1);
    }
}
