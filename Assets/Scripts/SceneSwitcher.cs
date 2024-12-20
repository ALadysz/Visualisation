using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    //switch scene
    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //reload scene
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
