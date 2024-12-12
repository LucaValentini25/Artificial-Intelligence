using System;
using System.Collections.Generic;
using FSM;
using NPC;
using UnityEngine;

public class Player : MonoBehaviour
{
    

    public Action<List<Node>> OnNewPath;
    [SerializeField] private float _speed;
    [SerializeField] private ColorBand _colorBand;
    [SerializeField] public ColorBand GetColorBand() { return _colorBand; }

    private Fsm _FSM;
    private void Awake()
    {

        _FSM = new Fsm();
        //_FSM.CreateState("MoveTo", new MoveToState(this, ref OnNewPath));
        //_nodeLayerMask = patrolNodes.First().gameObject.layer;
    }
    private void Update()
    {
        _FSM.Execute();
    }

    public void GoToPoint(Node node)
    {
        List<Node> path = new List<Node>();
        if (GameManager.Instance.LosToWall(transform.position, node.transform.position))
        {
            path.Add(node);
        }
        else
        {
            Node nearNode = NodeManager.Instance.GetNearestNode(transform.position);
            path = Pathfinding.CalculateThetaStar(nearNode, node);
        }
        OnNewPath?.Invoke(path);
    }

    //FSM
    public void MoveTo(Vector3 dir)
    {
        transform.position += dir.normalized * _speed * Time.deltaTime;
        transform.forward = dir.normalized;
    }

    public bool OnCombat()
    {
        return true;
    }
}
