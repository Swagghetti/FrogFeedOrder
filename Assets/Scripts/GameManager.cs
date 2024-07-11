using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int totalMoves = 10;

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
        UpdateMovesText();
    }

    public void DecreaseMoveCount()
    {
        if (totalMoves > 0)
        {
            totalMoves--;
            UpdateMovesText();
        }

        /*if (totalMoves == 0 || !GridManager.Instance.HasFrogs())
        {
            GameOver();
        }*/
    }

    private void UpdateMovesText()
    {
        //movesText.text = "Moves: " + totalMoves;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        //gameOverUI.SetActive(true);
    }
}

