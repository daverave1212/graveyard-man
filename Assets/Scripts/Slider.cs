using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    
    public static float lengthStop = 1.0f;

    public Vector3 fromPoint;
    public Vector3 toPoint;

    public float speed = 1.0f;
    public float startTime;

    public float journeyLength;

    public event System.Action<int> callback;

    bool isSliding = false;

    public void SlideTo(Vector3 to, System.Action<int> callback = null, float overTime = 1.0f) {
        fromPoint = gameObject.transform.position;
        toPoint = to;
        print("Starting to slide from:");
        print(fromPoint);
        print("To");
        print(toPoint);
        startTime = Time.time;
        journeyLength = overTime;
        isSliding = true;
        this.callback = callback;
    }
    int _updateCount = 1;
    void Update() {
        if (!isSliding) return;
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        print(_updateCount + " - Fraction of journey:" + fractionOfJourney);
        transform.position = Vector3.Lerp(fromPoint, toPoint, fractionOfJourney);
        if (fractionOfJourney >= 1) {
            isSliding = false;
            if(callback != null) callback(0);
        }
    }
}
