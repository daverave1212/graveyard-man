using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDelayed : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Invoke(nameof(RemoveDelay), Random.Range(0.0f, 0.2f));
    }

    private void RemoveDelay()
    {
        _animator.SetBool("Delayed", true);
    }
}