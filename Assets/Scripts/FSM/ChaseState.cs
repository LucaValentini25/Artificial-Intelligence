using System.Collections.Generic;
using NPC;
using UnityEngine;

namespace FSM
{
    public class ChaseState : NpcBaseState
    {
        private Enemy _user = default;
        private List<Node> _path = new List<Node>();
        private Vector3 _lastPlaceTarget = default;
        private bool checkLastPlace = false;
        public ChaseState(Npc userNpc, Animator anim) : base(userNpc, anim)
        {
            //_user = user;

        }
        public override void OnEnter()
        {
            _lastPlaceTarget = _user.GetTarget().position;
            if (!_user.LOStoTarget())
            {
                _path = Pathfinding.CalculateAStar(
                    NodeManager.Instance.GetNearestNode(_user.transform),
                    NodeManager.Instance.GetNearestNode(_lastPlaceTarget));
                checkLastPlace = false;
            }
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {
            Debug.Log($"{_user.gameObject.name} path count {_path.Count}") ;
            if (_user.LOStoTarget())
            {
                if(_path.Count >0)_path.Clear();
                _user.ChaseTarget();
                _lastPlaceTarget = _user.GetTarget().position;
                checkLastPlace = false;
            }
            else
            {
                if (_path.Count > 0)
                {
                    var dir = _path[0].transform.position - _user.transform.position;
                    _user.MoveTo(dir.normalized);
                    if (dir.magnitude < 0.1f)
                        _path.RemoveAt(0);
                    if(_path.Count <= 0 && !_user.LOStoTarget())
                        checkLastPlace = true;
                }
                else if (_path.Count <= 0)
                {
                    if(checkLastPlace)
                    {
                        _user.ChangeState("Patrol");
                    }
                    else
                    {
                        _path = Pathfinding.CalculateAStar(
                            NodeManager.Instance.GetNearestNode(_user.transform),
                            NodeManager.Instance.GetNearestNode(_lastPlaceTarget));
                    
                    }
                }
            }
        }
    }
}
