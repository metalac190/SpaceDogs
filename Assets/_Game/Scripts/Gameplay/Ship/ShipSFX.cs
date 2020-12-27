using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSFX : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] HealthSystem _health;
    [SerializeField] ShipShooting _shooting;
    [SerializeField] ShipTricks _tricks;

    [Header("Settings")]
    [SerializeField] AudioClip _healedSFX;
    [SerializeField] AudioClip _damagedSFX;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] AudioClip _weaponShotSFX;
    [SerializeField] AudioClip _boostSFX;
    [SerializeField] AudioClip _brakeSFX;

    private void OnEnable()
    {
        _health.Healed += OnHealed;
        _health.Damaged += OnDamaged;
        _health.Killed += OnKilled;
        _shooting.ShotWeapon += OnShotWeapon;
        _tricks.StartedBoost += OnStartedBoost;
        _tricks.StartedBrake += OnStartedBrake;
    }

    private void OnDisable()
    {
        _health.Healed -= OnHealed;
        _health.Damaged -= OnDamaged;
        _health.Killed -= OnKilled;
        _shooting.ShotWeapon -= OnShotWeapon;
        _tricks.StartedBoost -= OnStartedBoost;
        _tricks.StartedBrake -= OnStartedBrake;
    }

    void OnHealed(int amount)
    {
        if (_healedSFX == null)
            return;

        AudioHelper.PlayClip2D(_healedSFX, 1);
    }

    void OnDamaged(int amount)
    {
        if (_damagedSFX == null)
            return;

        AudioHelper.PlayClip2D(_damagedSFX, 1);
    }

    void OnKilled()
    {
        if (_deathSFX == null)
            return;

        AudioHelper.PlayClip2D(_deathSFX, 1);
    }

    void OnShotWeapon()
    {
        if (_weaponShotSFX == null)
            return;

        AudioHelper.PlayClip2D(_weaponShotSFX, 1);
    }

    void OnStartedBoost()
    {
        if (_boostSFX == null)
            return;

        AudioHelper.PlayClip2D(_boostSFX, 1);
    }

    void OnStartedBrake()
    {
        if (_brakeSFX == null)
            return;

        AudioHelper.PlayClip2D(_brakeSFX, 1);
    }
}
