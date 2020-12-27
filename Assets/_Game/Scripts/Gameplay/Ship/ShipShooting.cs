using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

public class ShipShooting : MonoBehaviour
{
    public event Action ShotWeapon;

    [SerializeField] Ship _ship = null;

    [Header("Shooting")]
    [SerializeField] Projectile _defaultProjectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;

    Collider _shipCollider;
    Collider _projectileCollider;

    private void Awake()
    {
        // fill dependencies here
        _shipCollider = _ship.GetComponent<Collider>();
    }

    public void Shoot()
    {
        Projectile projectile = Instantiate(_defaultProjectilePrefab,
            _projectileSpawnPoint.position, transform.rotation);

        // attempt to avoid player and projectile collision from touching
        _projectileCollider = projectile.GetComponent<Collider>();
        if(_shipCollider != null && _projectileCollider != null)
            Physics.IgnoreCollision(_shipCollider, _projectileCollider, true);

        ShotWeapon?.Invoke();
    }

    public void Charge()
    {

    }
}
