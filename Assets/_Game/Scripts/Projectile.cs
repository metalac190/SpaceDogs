 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
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
        //_rb.velocity = transform.forward * _moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // avoid detecting the player
        //TODO this won't work in multiplayer, but will work for testing
        if (other.isTrigger)
        {
            return;
        }

        VisualEffect impactParticle = Instantiate
            (_impactParticle, transform.position, transform.rotation);
        impactParticle.Play();

        Destroy(gameObject);
    }
}
