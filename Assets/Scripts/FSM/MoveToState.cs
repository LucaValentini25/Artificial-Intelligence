using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class MoveToState : IState
    {
        private Player _user;
        private Queue<Node> _patrolQ;
        private Node _actualNode;

        public MoveToState(Player user, ref Action<List<Node>> onNewPath)
        {
            _user = user;
            onNewPath += NewPath;
            _patrolQ = new Queue<Node>();
            _actualNode = null;
        }

        public void OnEnter()
        {
            
            Debug.Log(_actualNode);
            if (_actualNode == null)
                GetNextNode();

        }

        public void OnExit()
        {

        }

        public void OnUpdate()
        {
            //    if (_user.InFOV())
            //    {
            //        _user.ChangeState("Chase");
            //    }
            //    else
            //    {
            if (_patrolQ.Count > 0 || _actualNode != null)
                Patrol();
            //}
        }

        private void Patrol()
        {
            var dir = _actualNode.transform.position - _user.transform.position;
            if (dir.magnitude <= 0.05f)
            {
                GetNextNode();
            }
            else
            {
                _user.MoveTo(dir);
            }
        }

        private void GetNextNode()
        {
            if(_patrolQ.Count > 0)
                _actualNode = _patrolQ.Dequeue();
        
        }

        private void NewPath(List<Node> nodes)
        {
            _patrolQ = new();
            foreach (Node node in nodes)
            {
                _patrolQ.Enqueue(node);
            }
            GetNextNode();
        }
    }
}
