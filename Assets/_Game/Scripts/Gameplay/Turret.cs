using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Projectile _defaultProjectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;

    [Header("General")]
    [SerializeField] float _shootCooldownInSeconds;

    [Header("Feedback")]
    [SerializeField] VisualEffect _muzzleFlash;
    [SerializeField] AudioClip _shootSound;

    Collider _turretCollider = null;
    Coroutine _shootRoutine = null;

    private void Awake()
    {
        _turretCollider = GetComponent<Collider>();

        StartShootSequence();
    }

    void StartShootSequence()
    {
        if (_shootRoutine != null)
            StopCoroutine(_shootRoutine);
        _shootRoutine = StartCoroutine(ShootSequence(_shootCooldownInSeconds));
    }

    void StopShootSequence()
    {
        if (_shootRoutine != null)
            StopCoroutine(_shootRoutine);
    }

    IEnumerator ShootSequence(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Shoot();
        }
    }

    public void Shoot()
    {
        Projectile projectile = Instantiate(_defaultProjectilePrefab,
            _projectileSpawnPoint.position, transform.rotation);
        // attempt to avoid player and projectile collision from touching

        Collider projectileCollider = projectile.GetComponent<Collider>();
        Physics.IgnoreCollision(_turretCollider, projectileCollider, true);

        //TODO replace with Object Pooling
        if(_shootSound != null)
            AudioHelper.PlayClip2D(_shootSound, 1);
        if(_muzzleFlash != null)
            _muzzleFlash.Play();
    }
}
