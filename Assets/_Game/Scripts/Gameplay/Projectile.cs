 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] int _damageAmount = 5;
    [SerializeField] VisualEffect _impactParticle = null;

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveOffset = transform.forward * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + moveOffset);
    }

    private void OnCollisionEnter(Collision otherCollision)
    {
        //TODO projectiles colliding with other projectiles. Consider if this is intended
        // may need to increase projectile movespeed while shot from a boosted player ship
        IDamageable damageable = otherCollision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damageAmount);
        }
        Impact();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(_damageAmount);
            Impact();
        }
        
    }
    */

    private void Impact()
    {
        VisualEffect impactParticle = Instantiate
                    (_impactParticle, transform.position, transform.rotation);
        impactParticle.Play();

        Destroy(gameObject);
    }
}
