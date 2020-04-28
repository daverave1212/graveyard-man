using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefCinematic : MonoBehaviour
{
    
    Vector3 debugPos = new Vector3(47.26f, 0, 39.39f);
    Vector3 startPos = new Vector3(31.15f, 0, 33.72f);
    Vector3 endPos = new Vector3(51.85f, 0, 49.92f);

    void Start() {
        transform.GetChild(0).GetComponent<Animator>().Play("Run");
        transform.position = startPos;
        Invoke("StartMoving", 1);
    }

    void StartMoving() {
        gameObject.transform.LookAt(endPos);
        GetComponent<Slider>().SlideTo(endPos, null, 2000.0f);
        print("I am at");
        print(transform.position);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
