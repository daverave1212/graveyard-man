using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light))]
public class LightEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private Light _light;

    private float _initialRadius;

    private float _initialIntensity;

    private float _targetRadius;

    private float _targetIntensity;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(gameObject.GetHashCode());
        _light = GetComponent<Light>();
        _initialRadius = _light.range;
        _initialIntensity = _light.intensity;
        _targetRadius = Random.Range(_initialRadius - 0.15f * _initialRadius, _initialRadius - 0.1f * _initialRadius);
        _targetIntensity = Random.Range(_initialIntensity - 0.15f * _initialIntensity,
            _initialIntensity - 0.1f * _initialIntensity);
    }

    // Update is called once per frame
    void Update()
    {
        _light.range = Mathf.Lerp(_initialRadius, _targetRadius,
            (Mathf.Sin(Time.timeSinceLevelLoad * speed) + 1) * 0.5f);

        _light.intensity = Mathf.Lerp(_initialIntensity, _targetIntensity,
            (Mathf.Sin(Time.timeSinceLevelLoad * speed) + 1) * 0.5f);
    }
}