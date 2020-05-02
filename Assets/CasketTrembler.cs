using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CasketTrembler : MonoBehaviour
{
    private Vector3 _originalRotation;

    private float _dur = 0.2f;
    private float _count = 0.0f;

    private Vector3 _targetRot;

    private void Awake()
    {
        _originalRotation = transform.rotation.eulerAngles;
        _targetRot = _originalRotation + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f),
            Random.Range(-2.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_count > _dur)
        {
            _targetRot = _originalRotation + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f),
                Random.Range(-2.0f, 2.0f));
            _count = 0.0f;
        }

        transform.eulerAngles = Vector3.Lerp(_originalRotation, _targetRot, _count / _dur);
        _count += Time.deltaTime;
    }
}