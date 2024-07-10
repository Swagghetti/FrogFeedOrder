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

    public void Subscribe(Action<Node> frogClickedListener)
    {
        OnFrogClicked += frogClickedListener;
    }

    public void Unsubscribe(Action<Node> frogClickedListener)
    {
        OnFrogClicked -= frogClickedListener;
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
}
