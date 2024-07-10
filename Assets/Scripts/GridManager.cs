using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodesActivelyOnVisit = new List<Node>();
    [SerializeField] private LineRenderer lineRenderer;

    private void OnEnable()
    {
        EventManager.Instance.Subscribe(HandleFrogClicked);
        EventManager.Instance.SubscribeRemoveTopCells(HandleRemoveTopCells);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(HandleFrogClicked);
        EventManager.Instance.UnsubscribeRemoveTopCells(HandleRemoveTopCells);
    }

    private void HandleFrogClicked(Node startNode)
    {
        CalculateVisitNodes(startNode);
        GenerateTongue();
    }

    private void HandleRemoveTopCells(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            node.RemoveTopCell();
        }
    }

    private void GenerateTongue()
    {
        if (nodesActivelyOnVisit == null || nodesActivelyOnVisit.Count == 1)
        {
            Debug.Log("returning");
            return;
        }

        Vector3[] positions = new Vector3[nodesActivelyOnVisit.Count];

        lineRenderer.positionCount = nodesActivelyOnVisit.Count;
        for (int i = 0; i < nodesActivelyOnVisit.Count; i++)
        {
            positions[i] = nodesActivelyOnVisit[i].GetEntityObject().transform.position;
            lineRenderer.SetPosition(i, positions[i]);
        }

        StartTongueMovement(positions);
    }

    private void StartTongueMovement(Vector3[] positions)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = positions.Length - 1; i > 0; i--)
        {
            int currentIndex = i;

            if (nodesActivelyOnVisit[currentIndex]?.GetEntityObject() != null)
            {
                Transform entityTransform = nodesActivelyOnVisit[currentIndex].GetEntityObject().transform;
                sequence.Append(entityTransform.DOMove(positions[currentIndex - 1], 0.3f).OnUpdate(() =>
                {
                    if (entityTransform != null)
                    {
                        UpdateLineRendererPosition(currentIndex);
                    }
                }));
            }
        }

        Vector3 startPosition = positions[0];
        sequence.AppendCallback(() =>
        {
            for (int i = 0; i < nodesActivelyOnVisit.Count; i++)
            {
                if (nodesActivelyOnVisit[i]?.GetEntityObject() != null)
                {
                    Transform entityTransform = nodesActivelyOnVisit[i].GetEntityObject().transform;
                    entityTransform.DOMove(startPosition, 0.5f);
                }
            }
        });

        sequence.AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                EventManager.Instance.RemoveTopCells(nodesActivelyOnVisit);
            })
            .AppendCallback(() =>
            {
                nodesActivelyOnVisit.Clear();
            });

        sequence.Play();
    }



    private void UpdateLineRendererPosition(int currentIndex)
    {
        Vector3[] updatedPositions = new Vector3[currentIndex + 1];
        for (int i = 0; i <= currentIndex; i++)
        {
            updatedPositions[i] = nodesActivelyOnVisit[i].GetEntityObject().transform.position;
        }

        lineRenderer.positionCount = currentIndex + 1;
        lineRenderer.SetPositions(updatedPositions);
    }

    private void CalculateVisitNodes(Node startNode)
    {
        Cell topCell = startNode.GetTopCell();
        Cell.CellColor targetColor = topCell.GetCellColor();
        PointDirection direction = topCell.GetPointDirection();

        Node currentNode = startNode;

        while (true)
        {
            Node nextNode = GetNextNode(currentNode, direction);

            Debug.Log(currentNode.gameObject.name + " visited");

            nodesActivelyOnVisit.Add(currentNode);

            if (nextNode == null || nextNode.GetTopCell().GetCellColor() != targetColor)
            {
                break;
            }

            currentNode = nextNode;
        }
    }

    private Node GetNextNode(Node currentNode, PointDirection direction)
    {
        return currentNode.GetNeighboringNode(direction);
    }
}