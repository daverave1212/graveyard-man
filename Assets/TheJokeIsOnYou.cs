using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TheJokeIsOnYou : MonoBehaviour
{
    [SerializeField] private TextMeshPro jokeText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PickJoke());
    }

    IEnumerator PickJoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20.0f, 60.0f));
            if (Random.Range(0.0f, 10.0f) / 5.0f > 1.0f)
            {
                ZiDeAiaCuBerica();
            }
            else
            {
                ZiDeAiaCuMancarea();
            }
        }
    }

    IEnumerator KillJoke()
    {
        yield return new WaitForSeconds(5.0f);
        jokeText.text = "";
    }

    private void ZiDeAiaCuMancarea()
    {
        SoundManager.Instance.PlaySound("Growl");
        jokeText.text = "Mama ce-ar merge una de la Jerry's acuma...";
        StartCoroutine(KillJoke());
    }

    private void ZiDeAiaCuBerica()
    {
        SoundManager.Instance.PlaySound("Burp");
        jokeText.text = "Iar am baut prea mult aseara...";
        StartCoroutine(KillJoke());
    }
}