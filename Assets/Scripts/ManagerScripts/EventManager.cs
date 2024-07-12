using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    private event Action<Node> OnFrogClicked;
    private event Action<List<Node>> OnRemoveTopCells;
    private event Action OnGameWin;
    private event Action OnGameLose;
    
    private event Action<int> onMoveCountChanged;
    

    public void Subscribe(Action<Node> frogClickedListener)
    {
        OnFrogClicked += frogClickedListener;
    }

    public void Unsubscribe(Action<Node> frogClickedListener)
    {
        OnFrogClicked -= frogClickedListener;
    }
    
    public void SubscribeOnMoveCountChanged(Action<int> listener)
    {
        onMoveCountChanged += listener;
    }

    public void UnsubscribeOnMoveCountChanged(Action<int> listener)
    {
        onMoveCountChanged -= listener;
    }

    public void TriggerOnMoveCountChanged(int count)
    {
        onMoveCountChanged?.Invoke(count);
    }

    public void FrogClicked(Node node)
    {
        OnFrogClicked?.Invoke(node);
    }

    public void SubscribeRemoveTopCells(Action<List<Node>> removeTopCellsListener)
    {
        OnRemoveTopCells += removeTopCellsListener;
    }

    public void UnsubscribeRemoveTopCells(Action<List<Node>> removeTopCellsListener)
    {
        OnRemoveTopCells -= removeTopCellsListener;
    }

    public void RemoveTopCells(List<Node> nodes)
    {
        OnRemoveTopCells?.Invoke(nodes);
    }
    
    public void TriggerGameWin()
    {
        OnGameWin?.Invoke();
    }

    public void TriggerGameLose()
    {
        OnGameLose?.Invoke();
    }

    public void SubscribeGameWin(Action subscriber)
    {
        OnGameWin += subscriber;
    }

    public void UnsubscribeGameWin(Action subscriber)
    {
        OnGameWin -= subscriber;
    }

    public void SubscribeGameLose(Action subscriber)
    {
        OnGameLose += subscriber;
    }

    public void UnsubscribeGameLose(Action subscriber)
    {
        OnGameLose -= subscriber;
    }
}
