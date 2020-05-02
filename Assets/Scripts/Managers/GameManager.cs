using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [SerializeField] public float EnemySpawnRate = 15.0f;

    [SerializeField] private TextMeshProUGUI scoreLabel;

    [SerializeField] private TextMeshProUGUI highScoreLabel;

    private WaitForSeconds _waitForSpawn;

    private float _score = 0.0f;

    void Start()
    {
        _waitForSpawn = new WaitForSeconds(EnemySpawnRate);
        StartCoroutine(SpawnEnemy());
        UpdateScoreLabel();
        UpdateHighScoreLabel();
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnemyManager.Instance.SpawnEnemy();
            yield return _waitForSpawn;
        }
    }

    private void UpdateScoreLabel()
    {
        scoreLabel.text = "Score: " + _score.ToString();
    }

    private void UpdateHighScoreLabel()
    {
        highScoreLabel.text = "High Score: " + PlayerPrefs.GetFloat("high_score", 0.0f).ToString();
    }

    public void IncrementScore(float value)
    {
        _score += value;
        UpdateScoreLabel();
    }

    private void SaveHighScoreIfValid()
    {
        if (_score > PlayerPrefs.GetFloat("high_score", 0.0f))
        {
            PlayerPrefs.SetFloat("high_score", _score);
        }

        UpdateHighScoreLabel();
    }

    private void OnApplicationQuit()
    {
        SaveHighScoreIfValid();
    }

    private void OnDestroy()
    {
        SaveHighScoreIfValid();
    }
}