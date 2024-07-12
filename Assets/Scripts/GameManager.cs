using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _isGameFinished;
    
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

    private void Start()
    {
        _isGameFinished = false;
    }

    public void GameWin()
    {
        StartCoroutine(GameWinCoroutine());
    }

    IEnumerator GameWinCoroutine()
    {
        _isGameFinished = true;
        yield return new WaitForSeconds(0.4f);
        
        Debug.Log("GameWin");
        EventManager.Instance.TriggerGameWin();
    }

    public void GameLose()
    {
        StartCoroutine(GameLoseCoroutine());
    }
    
    IEnumerator GameLoseCoroutine()
    {
        _isGameFinished = true;
        yield return new WaitForSeconds(0.4f);
        
        Debug.Log("GameLose");
        EventManager.Instance.TriggerGameLose();
    }

    public bool IsGameFinished()
    {
        return _isGameFinished;
    }
    
}

