using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantCinematicScript : MonoBehaviour
{
    void Start() {
        GetComponent<Animator>().Play("PeasantStandCinematic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
