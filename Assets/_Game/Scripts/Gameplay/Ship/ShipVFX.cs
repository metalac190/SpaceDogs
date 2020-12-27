using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipVFX : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] HealthSystem _health;
    [SerializeField] ShipShooting _shooting;
    [SerializeField] ShipTricks _tricks;

    [Header("Settings")]
    [SerializeField] VisualEffect _healedVFX;
    [SerializeField] VisualEffect _damagedVFX;
    [SerializeField] VisualEffect _deathVFXPrefab;
    [SerializeField] VisualEffect _muzzleFlashVFX;
    [SerializeField] VisualEffect _boostVFX;
    [SerializeField] VisualEffect _brakeVFX;

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
        if (_healedVFX == null)
            return;

        _healedVFX.Play();
    }

    void OnDamaged(int amount)
    {
        if (_damagedVFX == null)
            return;

        _damagedVFX.Play();
    }

    void OnKilled()
    {
        if (_deathVFXPrefab == null)
            return;

        // spawn this one, since the object will likely be destroyed
        VisualEffect newDeathVFX = Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
        _deathVFXPrefab.Play();
    }

    void OnShotWeapon()
    {
        if (_muzzleFlashVFX == null)
            return;

        _muzzleFlashVFX.Play();
    }

    void OnStartedBoost()
    {
        if (_boostVFX == null)
            return;

        _boostVFX.Play();
    }

    void OnStartedBrake()
    {
        if (_brakeVFX == null)
            return;

        _brakeVFX.Play();
    }
}
