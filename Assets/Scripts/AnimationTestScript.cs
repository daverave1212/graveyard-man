using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
    void Start() {
        GetComponent<Animator>().Play("Running");
    }

    void Update() {
        
    }
}
