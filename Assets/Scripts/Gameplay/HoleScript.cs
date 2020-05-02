using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleScript : MonoBehaviour
{
    [SerializeField] private GameObject shaderHole;

    [SerializeField] private GameObject progressBar;

    Vector3 originalPosition;
    Trembler trembler;
    ParticleSystem particles;

    private bool _paused;

    private float _progress;

    private bool _hasCorpse;

    private GameObject _corpse;

    private AudioSource _diggingSound;

    [SerializeField] private float timeToDig = 2.0f;


    private RectTransform _progressBarRectTransform;

    void Start()
    {
        trembler = GetComponentInChildren<Trembler>();
        particles = transform.Find("DustParticles").GetComponent<ParticleSystem>();
        if (particles == null)
        {
            print("Failed to find particles");
        }

        particles.enableEmission = false;

        _progressBarRectTransform = progressBar.GetComponent<RectTransform>();
        _diggingSound = GetComponent<AudioSource>();

        PlayAnimation();
        StartCoroutine(HoleProgress());
    }

    public void PlayAnimation()
    {
        originalPosition = transform.position;
        trembler.StartTrembling();
        particles.enableEmission = true;
        _diggingSound.Play();
    }

    public void StopAnimation()
    {
        transform.position = originalPosition;
        trembler.StopTrembling();
        particles.enableEmission = false;
        _diggingSound.Stop();
    }

    IEnumerator HoleProgress()
    {
        if (!progressBar.activeInHierarchy)
        {
            progressBar.gameObject.transform.parent.gameObject.SetActive(true);
        }

        if (_hasCorpse)
        {
            progressBar.GetComponent<Image>().color = Color.red;
        }

        float _elapsed = 0.0f;
        while (_elapsed < timeToDig)
        {
            if (!_paused)
            {
                _elapsed += Time.deltaTime;
            }

            _progress = _elapsed / timeToDig;
            _progressBarRectTransform.localScale = new Vector3(_progress, 1.0f, 1.0f);
            yield return null;
        }

        progressBar.gameObject.transform.parent.gameObject.SetActive(false);

        StopAnimation();
        shaderHole.SetActive(!_hasCorpse);

        if (_hasCorpse)
        {
            GameManager.Instance.IncrementScore(5.0f);
            GravekeeperController.Instance.StopDigging();
            Destroy(gameObject);
            Destroy(_corpse);
        }
    }

    public void PauseDigging()
    {
        _paused = true;
        StopAnimation();
    }

    public void ResumeDigging()
    {
        _paused = false;

        if (_progress < 1.0f)
        {
            PlayAnimation();
        }
        else if (_hasCorpse)
        {
            PlayAnimation();
            StartCoroutine(HoleProgress());
        }
    }

    public void SetCorpse(GameObject corpse)
    {
        _corpse = corpse;
        _hasCorpse = true;
    }

    public bool HasCorpse()
    {
        return _hasCorpse;
    }
}