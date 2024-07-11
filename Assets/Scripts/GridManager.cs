using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodesActivelyOnVisit = new List<Node>();
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool isGridAvailable = true;

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
        if (!isGridAvailable)
            return;
        
        CalculateVisitNodes(startNode);

        if (nodesActivelyOnVisit.Count < 1 || !PathContainsGrape())
        {
            Debug.Log("Path doesnt contain grape");
            return;
        }
            
        isGridAvailable = false;
        GenerateTongue();
    }

    private bool PathContainsGrape()
    {
        foreach (var node in nodesActivelyOnVisit)
        {
            if (node.GetTopCell().GetCellEntityType() == Cell.EntityType.Grape)
                return true;
        }

        return false;
    }

    private void HandleRemoveTopCells(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                node.RemoveTopCell();
                isGridAvailable = true;
            }
        }
    }

    private void GenerateTongue()
    {
        if (nodesActivelyOnVisit == null || nodesActivelyOnVisit.Count == 1)
        {
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

            if (nodesActivelyOnVisit[currentIndex]?.GetEntityObject() != null && nodesActivelyOnVisit[currentIndex].GetTopCell().GetCellEntityType() != Cell.EntityType.Arrow)
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
                if (nodesActivelyOnVisit[i]?.GetEntityObject() != null && nodesActivelyOnVisit[i].GetTopCell().GetCellEntityType() != Cell.EntityType.Arrow)
                {
                    Transform entityTransform = nodesActivelyOnVisit[i].GetEntityObject().transform;
                    entityTransform.DOMove(startPosition, 0.15f);
                }
            }
        });

        sequence.AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                EventManager.Instance.RemoveTopCells(nodesActivelyOnVisit);
            })
            .AppendCallback(() =>
            {
                nodesActivelyOnVisit.Clear();
            })
            .AppendCallback(() =>
            {
                RetractTongue();
            });

        sequence.Play();
    }

    private void RetractTongue()
    {
        Sequence retractSequence = DOTween.Sequence();

        for (int i = lineRenderer.positionCount - 1; i >= 0; i--)
        {
            int index = i;
            retractSequence.AppendInterval(0.1f).AppendCallback(() =>
            {
                lineRenderer.positionCount = index;
            });
        }

        retractSequence.Play();
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

            if (currentNode.GetTopCell().GetCellEntityType() == Cell.EntityType.Arrow)
            {
                direction = currentNode.GetTopCell().GetPointDirection();
            }
        }
    }

    private Node GetNextNode(Node currentNode, PointDirection direction)
    {
        return currentNode.GetNeighboringNode(direction);
    }
}
