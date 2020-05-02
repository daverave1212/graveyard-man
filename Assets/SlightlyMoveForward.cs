using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlightlyMoveForward : MonoBehaviour
{
    [SerializeField] private GameObject forwardTarget;
    private void Update()
    {
        transform.position = transform.position + forwardTarget.transform.forward * 0.8f * Time.deltaTime /2;
    }
}