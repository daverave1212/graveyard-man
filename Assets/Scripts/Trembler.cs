using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trembler : MonoBehaviour
{

    private bool startedTrembling = false;
    private bool isTrembling = false;
    private Vector3 originalRotation;

    public void StartTrembling() {
        originalRotation = transform.eulerAngles;
        if (!startedTrembling) {
            startedTrembling = true;
            isTrembling = true;
            InvokeRepeating("TryTrembleOnce", 0.1f, 0.1f);
        } else if (!isTrembling) {
            isTrembling = true;
        }
    }

    public void StopTrembling() {
        transform.eulerAngles = originalRotation;
        isTrembling = false;
    }

    void TryTrembleOnce() {
        if (isTrembling) {
            TrembleOnce();
        }
    }

    void TrembleOnce() {
        var rotationVariation = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        transform.eulerAngles = originalRotation + rotationVariation;
    }
}
