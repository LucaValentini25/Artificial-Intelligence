using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private Player player1= default;
    [SerializeField] private Player player2= default;
    [SerializeField] private MovingNode p1Node = default;
    [SerializeField] private MovingNode p2Node = default;

    private void Start()
    {
            player1.GoToPoint(p1Node);
            player2.GoToPoint(p2Node);
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveNodeToMousePosition(p1Node);
            player1.GoToPoint(p1Node);
        }
        if(Input.GetMouseButtonDown(1))
        {

            MoveNodeToMousePosition(p2Node);
            player2.GoToPoint(p2Node);
        }
    }

    private void MoveNodeToMousePosition(MovingNode p)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                Vector3 hitPoint = hit.point;
                p.MoveNode(hitPoint);

            }
        }
    }
}
