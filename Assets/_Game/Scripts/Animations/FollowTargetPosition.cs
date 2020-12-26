using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetPosition : MonoBehaviour
{
    [SerializeField] bool _unparentOnActivate = false;  // unparenting avoids inheriting transformations, if needed
    [SerializeField] Transform _targetLocation;
    [SerializeField] Vector3 _offset = new Vector3(0,0,0);

    private void Awake()
    {
        if (_unparentOnActivate)
        {
            gameObject.transform.SetParent(null);
        }

    }

    private void Update()
    {
        if(_targetLocation != null)
        {
            transform.position = _targetLocation.position + _offset;
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        _targetLocation = targetTransform;
    }
}
