using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    [SerializeField] private GameObject shaderHole;

    Vector3 originalPosition;
    Slider slider;
    Trembler trembler;
    ParticleSystem particles;

    private bool _paused;

    private float _progress;

    [SerializeField] private float timeToDig = 2.0f;

    void Start()
    {
        slider = GetComponent<Slider>();
        trembler = GetComponent<Trembler>();
        particles = transform.Find("DustParticles").GetComponent<ParticleSystem>();
        if (particles == null) print("Failed to find particles");
        particles.enableEmission = false;

        PlayAnimation();
        StartCoroutine(HoleProgress());
    }

    public void PlayAnimation()
    {
        originalPosition = transform.position;
        trembler.StartTrembling();
        particles.enableEmission = true;
    }

    public void StopAnimation()
    {
        transform.position = originalPosition;
        trembler.StopTrembling();
        particles.enableEmission = false;
    }

    IEnumerator HoleProgress()
    {
        float _elapsed = 0.0f;
        while (_elapsed < timeToDig)
        {
            if (!_paused)
            {
                _elapsed += Time.deltaTime;
            }

            yield return null;
        }

        StopAnimation();
        shaderHole.SetActive(true);
    }

    public void PauseDigging()
    {
        _paused = true;
        StopAnimation();
    }

    public void ResumeDigging()
    {
        _paused = false;

        if (_progress <= 1.0f)
        {
            PlayAnimation();
        }
    }
}