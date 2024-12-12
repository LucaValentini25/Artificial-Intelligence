using System.Collections.Generic;
using NPC;
using UnityEngine;

namespace FSM
{
    public class PatrolState : NpcBaseState
    {
        private Enemy _user = default;
        private Queue<Node> _patrolQ = default;
        private Node _actualNode;
        public PatrolState(Npc userNpc, Animator anim, Enemy user, List<Node> patrolQ) : base(userNpc, anim)
        {
            _user = user;
            _patrolQ = new();
            foreach (Node node in patrolQ)
            {
                _patrolQ.Enqueue(node);
            }
            GetNextNode();
        }
        public override void OnEnter()
        {
            Debug.Log(_actualNode);
            if (_actualNode)
                GetNextNode();

        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {
            if (_user.InFOV())
            {
                _user.ChangeState("Chase");
            }
            else
            {
                Patrol();
            }
        }

        private void Patrol()
        {
            Debug.Log(_user);
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
            _actualNode = _patrolQ.Dequeue();
            _patrolQ.Enqueue(_actualNode);
        }
    }
}
