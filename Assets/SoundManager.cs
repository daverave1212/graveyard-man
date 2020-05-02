using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [SerializeField] private GameObject[] audios;
    // Start is called before the first frame update

    private Dictionary<string, AudioSource> _audios;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        _audios = new Dictionary<string, AudioSource>();
        foreach (var audio in audios)
        {
            _audios.Add(audio.name, audio.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(string name)
    {
        _audios[name]?.Play();
    }

    public void StopSound(string name)
    {
        _audios[name]?.Stop();
    }
}