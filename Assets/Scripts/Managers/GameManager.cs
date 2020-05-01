﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [SerializeField] public float EnemySpawnRate = 15.0f;

    private WaitForSeconds _waitForSpawn;

    void Start()
    {
        _waitForSpawn = new WaitForSeconds(EnemySpawnRate);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnemyManager.Instance.SpawnEnemy();
            yield return _waitForSpawn;
        }
    }
}