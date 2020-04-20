using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShovelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetArea;

    [SerializeField] private Vector2 minBounds, maxBounds;

    [SerializeField] private GameObject shovel;

    // Start is called before the first frame update
    private void Start()
    {
        Transform areaTransform = targetArea.transform;
        Vector2 halfOffset = 5f * (Vector2) areaTransform.localScale;
        StartCoroutine(PeriodicSpawn());
    }

    public void SpawnShovel()
    {
        Instantiate(shovel,
            new Vector3(Random.Range(minBounds.x, maxBounds.x), targetArea.transform.position.y,
                Random.Range(minBounds.y, maxBounds.y)), Quaternion.identity);
    }

    private IEnumerator PeriodicSpawn()
    {
        SpawnShovel();
        // while (true)
        // {
        //     SpawnShovel();
        //     yield return new WaitForSeconds(5);
        // }
        yield break;
    }
}