using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNode : Node
{
    public void MoveNode(Vector3 pos)
    {
        pos.y = 1;
        transform.position = pos;
        NodeManager.Instance.UpdateMovingNode(this);
    }
}
