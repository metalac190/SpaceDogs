using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetPosition : MonoBehaviour
{
    [SerializeField] Transform _targetLocation;
    [SerializeField] Vector3 _offset = new Vector3(0,0,0);

    private void Update()
    {
        if(_targetLocation != null)
        {
            transform.position = _targetLocation.position + _offset;
        }
    }

    public void SetTarget(Transform targetTransform, Vector3 offset)
    {
        _targetLocation = targetTransform;
    }
}