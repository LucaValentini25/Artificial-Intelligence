using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    [SerializeField] private Node[] _nodes = default;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        CreateNodeGrid();
    }

    private void CreateNodeGrid()
    {
        AssaignNeighbours();
    }

    public Node GetNearestNode(Transform pos) => GetNearestNode(pos.position);

    public Node GetNearestNode(Vector3 pos)
    {
        Node nearNode = _nodes[0];
        float distance = Vector3.Distance(nearNode.transform.position, pos);
        for (int i = 1; i < _nodes.Length; i++)
        {
            if (GameManager.Instance.LosToWall(nearNode.transform.position, _nodes[i].transform.position))
            {

                var newdistance = Vector3.Distance(pos, _nodes[i].transform.position);
                if (distance > newdistance)
                {
                    distance = newdistance;
                    nearNode = _nodes[i];
                }
            }
        }
        return nearNode;
    }


    private void AssaignNeighbours()
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            List<Node> neighbours = new List<Node>();

            for (int j = 0; j < _nodes.Length; j++)
            {
                if (i != j)
                {
                    if (GameManager.Instance.LosToWall(_nodes[i].transform.position, _nodes[j].transform.position))
                    {

                        neighbours.Add(_nodes[j]);

                    }
                }
            }
            _nodes[i].AssaignNeighbours(neighbours);
        }
    }

    public void UpdateMovingNode(MovingNode movingNode)
    {
        RemoveOldNeighbours(movingNode);
        List<Node> neighbours = new List<Node>();

        for (int j = 0; j < _nodes.Length; j++)
        {
            if (GameManager.Instance.LosToWall(movingNode.transform.position, _nodes[j].transform.position))
            {

                neighbours.Add(_nodes[j]);

            }

        }
        movingNode.AssaignNeighbours(neighbours);
        foreach (Node node in neighbours)
        {
            node.AddNeighbour(movingNode);
        }
    }

    private void RemoveOldNeighbours(MovingNode movingNode)
    {
        if (movingNode.NeighboursAmount <= 0) return;
        foreach (Node node in movingNode.GetNeighbours())
        {
            node.RemoveNeighbour(movingNode);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (Node node in _nodes)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(node.transform.position, 0.45f);


        }
    }

#endif
}
