using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SceneChange), 15.0f);
    }

    void SceneChange()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}