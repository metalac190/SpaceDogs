using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIncreasePickup : Pickup
{
    protected override void OnPickUp(Ship ship)
    {
        Debug.Log("Power up weapons!");
    }
}
