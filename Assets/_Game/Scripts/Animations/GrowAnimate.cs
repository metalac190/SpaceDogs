using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// This script will quickly grow or shrink an object when enabled. Useful for faking
/// particle bursts or adding inflections to things
/// </summary>
public class GrowAnimate : MonoBehaviour
{
    [Header("Grow Settings")]


    [SerializeField] bool _disableOnComplete = true;

    [Header("Randomization")]
    [SerializeField] float _scaleInSecondsMin = .05f;
    [SerializeField] float _scaleInSecondsMax = .1f;

    [SerializeField] float _scaleXMin = 1;
    [SerializeField] float _scaleXMax = 1;
    [SerializeField] float _scaleYMin = 1;
    [SerializeField] float _scaleYMax = 1;
    [SerializeField] float _scaleZMin = 1;
    [SerializeField] float _scaleZMax = 1;

    Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        Grow();
    }

    public void Grow()
    {
        transform.localScale = _initialScale;
        Vector3 targetScale = GetRandomTargetScale();

        float targetScaleDuration = UnityEngine.Random.Range(_scaleInSecondsMin, _scaleInSecondsMax);

        transform.DOScale(targetScale, targetScaleDuration).OnComplete(OnScaleComplete);
    }

    void OnScaleComplete()
    {
        if (_disableOnComplete)
        {
            gameObject.SetActive(false);
        }
    }

    Vector3 GetRandomTargetScale()
    {
        float targetXScale = UnityEngine.Random.Range(_scaleXMin, _scaleXMax);
        float targetYScale = UnityEngine.Random.Range(_scaleYMin, _scaleYMax);
        float targetZScale = UnityEngine.Random.Range(_scaleZMin, _scaleZMax);

        Vector3 targetScale = new Vector3(targetXScale, targetYScale, targetZScale);
        return targetScale;
    }
}
