using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject[] enemySpawners;

    [SerializeField] private float speed = 2.0f;

    [SerializeField] public static EnemyManager Instance { get; set; }

    private List<GameObject> _enemies = new List<GameObject>();

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        for (int index = 0; index < _enemies.Count; index++)
        {
            _enemies[index].transform.LookAt(target.transform.position);
            _enemies[index].transform.position = Vector3.MoveTowards(_enemies[index].transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, enemySpawners[Random.Range(0, enemySpawners.Length - 1)].transform.position,
            Quaternion.identity);
        newEnemy.transform.LookAt(target.transform.position);
        _enemies.Add(newEnemy);
    }

    public void DestroyEnemy(GameObject referenceObject)
    {
        _enemies.Remove(referenceObject);
        // Destroy(referenceObject);

    }
}