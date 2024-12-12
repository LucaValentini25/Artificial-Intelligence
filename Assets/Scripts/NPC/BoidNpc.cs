using System.Collections.Generic;
using System.Linq;
using DecisionTree;
using UnityEngine;
using FSM;
namespace NPC
{
    public class BoidNpc : Npc
    {


        // public override Npc Initialize(NpcStats npcStats, ColorBand color,Transform target)
        // {
        //     return base.Initialize(color);
        // }
        public override void Attack()
        {

        }

        public override void Chase()
        {
            if (LinOfSightToLider())
            {
                
            AddForce(Seek(Lider.position));
            Flocking();
            }
            else
            {
                //pathfind
                if (!_nextNode)
                {
                    if(!GetNextNode())
                    {
                        GetPathToLider();
                        if(!GetNextNode()) Debug.LogError("No Path To Lider"); 
                    }
                }
                var distance = Vector3.Distance(transform.position, _nextNode.transform.position);
                if (distance < stats.arriveDistanceThreshold)
                {
                    if (!GetNextNode())
                    {
                        GetPathToLider();
                        if(!GetNextNode()) Debug.LogError("No Path To Lider"); 
                    }
                }
                AddForce(Seek(_nextNode.transform.position));
            }
        }

        private void GetPathToLider()
        {
            var newPath =  Pathfinding.CalculateThetaStar(transform.position, Lider.position);
            _path = new Queue<Node>();
            foreach (var node in newPath)
            {
                _path.Enqueue(node);
            }
        }

        private bool GetNextNode()
        {
            return _path.TryDequeue(out _nextNode); 
        }

        private bool LinOfSightToLider()
        {
            return GameManager.Instance.LosToWall(transform.position,Lider.position);
        }

        public void Hide()
        {

        }


        public bool HpLow()
        {
            return stats.hp < 5;
        }
        public override bool NearTarget()
        {
            bool near = Vector3.Distance(transform.position, Lider.position) <= stats.viewRadius &&
                        GameManager.Instance.LosToWall(transform.position, Lider.position);
            return near;
        }
        public override bool OnCombat()
        {
            //chequear enemigos con FOV
            var boids = GameManager.Instance.GetAllBoids().Where(x => !x.Equals(NpcColor)).ToList();
            //asignar enemytarget
            // bool targetNear = EnemyManager.Instance.GetEnemyTarget(out enemyTarget);
            // return  targetNear;
            return  false;
        }

        public override void ChangeStateTo(NpcStates state)
        {
            if (StateMachine.IsActualState(state)) return;

            StateMachine.ChangeState(state);
        }

        protected override Fsm CreateStateMachine()
        {
            Fsm fsm = new ();
            fsm.CreateState(NpcStates.Idle,new IdleState(this,Anim));
            fsm.CreateState(NpcStates.Attack,new AttackState(this, Anim));
            fsm.CreateState(NpcStates.Chase,new ChaseState(this, Anim));
            fsm.CreateState(NpcStates.Hide,new HideState(this, Anim));
            return fsm;
        }

        protected override Branch CreateDesitionTree()
        {
            var hideAb = new ActionBranch(() => ChangeStateTo(NpcStates.Hide));
            var attackAb = new ActionBranch(() => ChangeStateTo(NpcStates.Attack));
            var chaseAb = new ActionBranch(() => ChangeStateTo(NpcStates.Chase));
            var idleAb = new ActionBranch(() => ChangeStateTo(NpcStates.Idle));
            var nearQb = new QuestionBranch( idleAb, chaseAb, NearTarget);
            var combatQb = new QuestionBranch(attackAb, nearQb, OnCombat);
            return new QuestionBranch(hideAb, combatQb, HpLow);
        }
    }
}
