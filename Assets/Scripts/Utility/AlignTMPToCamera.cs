using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlignTMPToCamera : MonoBehaviour
{
    private Transform _textMeshTransform;
    void Start()
    {
        _textMeshTransform = gameObject.transform;
    }
    
    void Update()
    {
        _textMeshTransform.rotation = Camera.main.transform.rotation;
    }
}
