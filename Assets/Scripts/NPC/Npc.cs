using System.Collections.Generic;
using System.Linq;
using DecisionTree;
using FSM;
using UnityEngine;

namespace NPC
{
    public abstract class Npc : MonoBehaviour
    {
        [SerializeField] protected NpcStats stats;
        public ColorBand NpcColor { get; private set; }
        protected Animator Anim;
        private Branch _desitionTree;
        protected Fsm StateMachine;
        protected Transform Lider;
        protected Transform  EnemyTarget;

        protected Vector3 _velocity;
        protected Queue<Node> _path;
        protected Node _nextNode;
        public virtual void Update()
        {
            _desitionTree.Execute();
            StateMachine.Execute();
        }
        public  Npc Initialize( ColorBand color)
        {
            NpcColor = color;
            Anim = GetComponent<Animator>();
            _desitionTree = CreateDesitionTree();
            StateMachine = CreateStateMachine();
            return this;
        }

        public Npc SetStats(NpcStats npcStats)
        {
            stats = npcStats;
            return this;
        }

        public Npc SetTarget(Transform target)
        {
            Lider = target;
            return this;
        }
        #region QuestionBranch
        public abstract bool OnCombat();
        public abstract bool NearTarget();
        #endregion

        #region ActionBranch
        public abstract void ChangeStateTo(NpcStates state);
        #endregion
    
        #region ActionsFSM
        public abstract void Attack();
        public abstract void Chase();
        #endregion

        protected abstract Fsm CreateStateMachine();
        protected abstract Branch CreateDesitionTree();

        #region Movement

    #region FLOCKING
        protected void Flocking()
        {
            var boids = GetBoidsByColor(NpcColor);
        AddForce(Separation(boids, stats.separationRadius) * stats.weightSeparation);
        AddForce(Alignment(boids) * stats.weightAlignment);
        AddForce(Cohesion(boids) * stats.weightCohesion);
    }

        protected List<Npc> GetBoidsByColor(ColorBand colorBand)
        {
            return GameManager.Instance.GetAllBoids().Where(x => x.NpcColor.Equals(colorBand)).ToList();
        }

        Vector3 Separation(List<Npc> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in boids)
        {
            var dir = item.transform.position - transform.position;
            if (dir.magnitude > radius || item == this)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= stats.maxSpeed;

        return CalculateSteering(desired);
    }
    
    Vector3 Alignment(List<Npc> boids)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in boids)
        {
            if (item == this) continue;

            if(Vector3.Distance(transform.position, item.transform.position) <= stats.viewRadius)
            {
                desired += item._velocity;

                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= stats.maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion(List<Npc> boids)
    {
        Vector3 desired = transform.position;
        int count = 0;

        foreach (var item in boids)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist > stats.viewRadius || item == this) continue;

            desired += item.transform.position;
            count++;
        }

        if(count == 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= stats.maxSpeed;

        return CalculateSteering(desired);
    }

    #endregion

    protected Vector3 Seek(Vector3 dir)
    {
        var desired = dir - transform.position;
        desired.Normalize();
        desired *= stats.maxSpeed;

        return CalculateSteering(desired);
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, stats.maxForce);

        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, stats.maxSpeed);
    }

        #endregion
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, stats.viewRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.attackRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stats.separationRadius);
        }
#endif

    }

    public enum ColorBand
    {
        Red,
        Blue
    }
}