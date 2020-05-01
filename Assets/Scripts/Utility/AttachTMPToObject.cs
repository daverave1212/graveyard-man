using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTMPToObject : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private float offsetY = 2.0f;
   
    void Update()
    {
        transform.position = target.transform.position + Vector3.up * offsetY;
    }
}
