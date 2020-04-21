using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    
    public static float lengthStop = 1.0f;

    public Vector3 fromPoint;
    public Vector3 toPoint;

    public float speed = 100.0f;
    public float startTime;

    public float journeyLength;

    public event System.Action<int> callback;

    bool isSliding = false;

    public void PauseSliding() { isSliding = false; }
    public void ContinueSliding() { isSliding = true; }

    public void SlideTo(Vector3 to, System.Action<int> callback) {
        fromPoint = gameObject.transform.position;
        toPoint = to;
        startTime = Time.time;
        //journeyLength = Vector3.Distance(fromPoint, toPoint);
        journeyLength = 1f;
        isSliding = true;
        this.callback = callback;
    }

    void Update() {
        if (!isSliding) return;
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        print("Fraction of journey:" + fractionOfJourney);
        transform.position = Vector3.Lerp(fromPoint, toPoint, fractionOfJourney);
        if (fractionOfJourney >= 1) {
            isSliding = false;
            callback(0);
        }
    }
}
