using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private void Awake()
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
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);


        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
    
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }

    public void LoadMenu()
    {
        LoadScene("MenuScene");
    }

    public void LoadLevel1()
    {
        LoadScene("Level1Scene");
    }
    
    public void LoadLevel2()
    {
        LoadScene("Level2Scene");
    }
    
    public void LoadLevel3()
    {
        LoadScene("Level3Scene");
    }
}
