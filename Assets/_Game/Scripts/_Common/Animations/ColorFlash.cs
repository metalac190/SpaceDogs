using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Material _material = null;
    [SerializeField] Color _colorToFlash = Color.red;
    [SerializeField] float _secondsToColor = .2f;
    [SerializeField] float _secondsFromColor = .2f;

    const string EmissionParameterName = "_EmissionColor";
    Color _originalColor;
    Coroutine _flashRoutine = null;

    private void Start()
    {
        _originalColor = _material.GetColor(EmissionParameterName);
    }

    public void Play()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(Flash(_colorToFlash, _secondsToColor, _secondsFromColor));
    }

    public void Stop()
    {
        _material.SetColor(EmissionParameterName, _originalColor);
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
    }

    IEnumerator Flash(Color flashColor, float secondsToColor, float secondsFromColor)
    {
        // flash in
        for (float elapsedTime = 0; elapsedTime <= secondsToColor; elapsedTime += Time.deltaTime)
        {
            Color newColor = Color.Lerp(_originalColor, flashColor, elapsedTime / secondsToColor);
            _material.SetColor(EmissionParameterName, newColor);
            yield return null;
        }

        // flash out
        for (float elapsedTime = 0; elapsedTime <= secondsFromColor; elapsedTime += Time.deltaTime)
        {
            Color newColor = Color.Lerp(flashColor, _originalColor, elapsedTime / secondsToColor);
            _material.SetColor(EmissionParameterName, newColor);
            yield return null;
        }
    }

    private void OnDisable()
    {
        Stop();
    }
}
