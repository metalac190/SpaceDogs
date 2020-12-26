using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] Ship _ship = null;

    [Header("Shooting")]
    [SerializeField] VisualEffect _muzzleFlash;
    [SerializeField] AudioClip _shootSound;

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
        SetIgnoreCollider(_shipCollider, _projectileCollider, true);

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

    // note, you could allow friendly fire/reflect fire with this, but by default it will ignore the firing player
    public void SetIgnoreCollider(Collider collider1, Collider collider2, bool shouldIgnore)
    {
        if(collider1 && collider2 != null)
        {
            Physics.IgnoreCollision(collider1, collider2, shouldIgnore);
        }
        else
        {
            Debug.LogWarning("Cannot ignore colliders on projectile: one of the collisions is null");
        }
    }
}
