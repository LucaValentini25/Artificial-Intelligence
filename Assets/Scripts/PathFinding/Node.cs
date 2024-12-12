using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]private List<Node> _neighbours = default;

    public int NeighboursAmount =>_neighbours.Count;
    public List<Node> GetNeighbours () => _neighbours;
    
    public void AssaignNeighbours(List<Node> neighbours)
    {
        _neighbours= neighbours;
    }
    public void AddNeighbour(Node node)
    {
       _neighbours.Add(node);
    }
    public void RemoveNeighbour(Node node)
    {
        if(_neighbours.Contains(node))
        {
            _neighbours.Remove(node);
        }
    }
#if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        if(_neighbours.Count > 0)
        {

        Gizmos.color= Color.yellow;
        foreach (Node node in _neighbours)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
        }
    }
#endif
}
