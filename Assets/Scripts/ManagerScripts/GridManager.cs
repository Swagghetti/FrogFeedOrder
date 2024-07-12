using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    private List<Node> nodesActivelyOnVisit = new List<Node>();
    private List<Node> nodes = new List<Node>();
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool isGridAvailable = true;
    [SerializeField] private int moveCount = 10;

    public static GridManager Instance { get; private set; }

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
    
    
    private void OnEnable()
    {
        EventManager.Instance.Subscribe(HandleFrogClicked);
        EventManager.Instance.SubscribeRemoveTopCells(HandleRemoveTopCells);
        EventManager.Instance.SubscribeGameWin(HandleGameWin);
        EventManager.Instance.SubscribeGameLose(HandleGameLose);
        EventManager.Instance.SubscribeOnMoveCountChanged(HandleMoveCountChanged);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(HandleFrogClicked);
        EventManager.Instance.UnsubscribeRemoveTopCells(HandleRemoveTopCells);
        EventManager.Instance.UnsubscribeGameWin(HandleGameWin);
        EventManager.Instance.UnsubscribeGameLose(HandleGameLose);
        EventManager.Instance.UnsubscribeOnMoveCountChanged(HandleMoveCountChanged);
    }

    private void Start()
    {
        nodes = this.transform.GetComponentsInChildren<Node>().ToList();
        
        UpdateMoveCount(moveCount);
    }

    private void HandleFrogClicked(Node startNode)
    {
        if (!isGridAvailable || GameManager.Instance.IsGameFinished() || startNode.GetTopCell().GetCellEntityType() != Cell.EntityType.Frog)
            return;
        
        CalculateVisitNodes(startNode, nodesActivelyOnVisit);

        if (nodesActivelyOnVisit.Count < 1 || !PathContainsGrape(nodesActivelyOnVisit))
        {
            Debug.Log("Path doesnt contain grape");
            nodesActivelyOnVisit.Clear();
            return;
        }
            
        isGridAvailable = false;
        UpdateMoveCount(moveCount - 1);
        
        GenerateTongue();
    }
    
    private void HandleMoveCountChanged(int count)
    {
        // can be added later
    }
    
    private void UpdateMoveCount(int newMoveCount)
    {
        moveCount = newMoveCount;
        EventManager.Instance.TriggerOnMoveCountChanged(moveCount);
    }

    private void CheckGameEndConditions()
    {
        List<Node> frogNodes = new List<Node>();

        //get all nodes with a frog on top cell in a list
        foreach (var node in nodes)
        {
            var topCell = node.GetTopCell();
            if (topCell != null && topCell.GetCellEntityType() == Cell.EntityType.Frog)
            {
                frogNodes.Add(node);
            }
        }
        
        //check if there are no frogs on the grid
        if (frogNodes.Count == 0)
        {
            foreach (var node in nodes)
            {
                if (node.HasUnusedHiddenFrog())
                {
                    GameManager.Instance.GameLose();
                    return;
                }
            }

            //if no hidden frogs are available, the game is won
            GameManager.Instance.GameWin();
            return;
        }

        //check if the number of moves left is zero
        if (moveCount <= 0)
        {
            GameManager.Instance.GameLose();
            return;
        }

        //check if frogs can be clicked (they have a valid neighboring node to move to)
        foreach (var node in frogNodes)
        {
            List<Node> tempPath = new List<Node>();
            CalculateVisitNodes(node, tempPath);

            if (PathContainsGrape(tempPath))
            {
                return;
            }
        }

        //no legal movements available, game is lost
        GameManager.Instance.GameLose();
    }

    IEnumerator CheckGameEndConditionsCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        
        CheckGameEndConditions();
    }


    private void HandleGameWin()
    {
        LockTheGrid();
    }

    private void HandleGameLose()
    {
        LockTheGrid();
    }

    private void LockTheGrid()
    {
        isGridAvailable = false;
    }

    private bool PathContainsGrape(List<Node> path)
    {
        foreach (var node in path)
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

        StartCoroutine(CheckGameEndConditionsCoroutine());
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

    private void CalculateVisitNodes(Node startNode, List<Node> nodeList)
    {
        Cell topCell = startNode.GetTopCell();
        Cell.CellColor targetColor = topCell.GetCellColor();
        PointDirection direction = topCell.GetPointDirection();

        Node currentNode = startNode;
        
        
        if (startNode.GetNeighboringNode(direction) == null)
            return;

        while (true)
        {
            Node nextNode = GetNextNode(currentNode, direction);


            nodeList.Add(currentNode);

            if (nextNode == null || nextNode.GetTopCell().GetCellColor() != targetColor || nextNode.GetTopCell().GetCellEntityType() == Cell.EntityType.Frog)
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
