using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Cell> _cells;
    
    [SerializeField] private Node _topNeighborNode;
    [SerializeField] private Node _rightNeighborNode;
    [SerializeField] private Node _bottomNeighborNode;
    [SerializeField] private Node _leftNeighborNode;
    
    
}
