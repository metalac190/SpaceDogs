using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{
    protected abstract void OnPickUp(Ship ship);

    [Header("Base Settings")]
    [SerializeField] AudioClip _pickupSFX = null;
    [SerializeField] VisualEffect _pickupVFXPrefab = null;

    Collider _collider = null;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Reset()
    {
        // set isTrigger in Inspector, in case Designer forgets to
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // guard clause
        Ship ship = other.GetComponent<Ship>();
        if (ship == null)
            return;

        // otherwise, we found a ship
        OnPickUp(ship);

        //TODO replace with object pooling
        if(_pickupSFX != null)
        {
            SpawnAudio(_pickupSFX);
        }
        //TODO replace with object pooling
        if(_pickupVFXPrefab != null)
        {
            SpawnVisuals(_pickupVFXPrefab);
        }

        Destroy(gameObject);
    }

    void SpawnAudio(AudioClip pickupSFX)
    {
        AudioHelper.PlayClip2D(pickupSFX, 1);
    }

    void SpawnVisuals(VisualEffect pickupVFXPrefab)
    {
        VisualEffect newPickupVFX = Instantiate
            (pickupVFXPrefab, transform.position, transform.rotation);

        newPickupVFX.Play();
    }
}
