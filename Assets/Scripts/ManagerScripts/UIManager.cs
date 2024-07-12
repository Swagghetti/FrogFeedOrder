using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfMovesText;
    [SerializeField] private TextMeshProUGUI finishMessageText;
    [SerializeField] private GameObject finishPanel;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeOnMoveCountChanged(SetMoveCountText);
        EventManager.Instance.SubscribeGameWin(HandleGameWin);
        EventManager.Instance.SubscribeGameLose(HandleGameLose);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeOnMoveCountChanged(SetMoveCountText);
        EventManager.Instance.UnsubscribeGameWin(HandleGameWin);
        EventManager.Instance.UnsubscribeGameLose(HandleGameLose);
    }

    private void HandleGameWin()
    {
        finishPanel.SetActive(true);
        SetFinishMessageText("You Win!");
    }
    
    private void HandleGameLose()
    {
        finishPanel.SetActive(true);
        SetFinishMessageText("You Lose");
    }

    private void SetMoveCountText(int count)
    {
        numberOfMovesText.text = count.ToString();
    }

    private void SetFinishMessageText(string text)
    {
        finishMessageText.text = text;
    }
}
