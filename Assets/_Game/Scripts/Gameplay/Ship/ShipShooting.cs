using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] VisualEffect _muzzleFlash;
    [SerializeField] AudioClip _shootSound;

    [SerializeField] Projectile _defaultProjectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;

    public void Shoot()
    {
        Projectile projectile = Instantiate(_defaultProjectilePrefab,
            _projectileSpawnPoint.position, transform.rotation);
        //TODO replace with Object Pooling
        AudioHelper.PlayClip2D(_shootSound, 1);
        PlayVFX();
    }

    public void Charge()
    {

    }

    void PlayVFX()
    {
        _muzzleFlash.Play();
    }
}
