using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class BotAI : MonoBehaviour
{
    Ship _ship = null;

    private void Awake()
    {
        _ship = GetComponent<Ship>();
    }

    //TODO add AI behaviors to call commands on ship
}
