using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] VisualEffect _muzzleFlash;
    [SerializeField] AudioClip _shootSound;

    public void Shoot()
    {
        AudioSource.PlayClipAtPoint(_shootSound, transform.position);
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
