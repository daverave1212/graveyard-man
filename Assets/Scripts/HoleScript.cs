using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour
{

    Vector3 originalPosition;
    Slider slider;
    Trembler trembler;
    ParticleSystem particles;

    void Start() {
        slider = GetComponent<Slider>();
        trembler = GetComponent<Trembler>();
        particles = transform.Find("DustParticles").GetComponent<ParticleSystem>();
        if (particles == null) print("Failed to find particles");
        particles.enableEmission = false;
    }

    public void PlayAnimation() {
        originalPosition = transform.position;
        trembler.StartTrembling();
        particles.enableEmission = true;
    }
    
    public void StopAnimation() {
        transform.position = originalPosition;
        trembler.StopTrembling();
        particles.enableEmission = false;
    }

}
