using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] int _damageAmount = 999;

    private void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.collider.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Rigidbody rb = other.collider.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.MoveRotation(Quaternion.Inverse(rb.rotation));
            }
            damageable.TakeDamage(2);
        }
    }
}
