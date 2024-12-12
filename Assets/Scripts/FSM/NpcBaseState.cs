using NPC;
using UnityEngine;

namespace FSM
{
    public class NpcBaseState : IState
    {
        protected Npc UserNpc;
        protected readonly Animator Anim;
        protected static readonly int IdleHash = Animator.StringToHash("Idle");
        protected static readonly int WalkHash = Animator.StringToHash("Walk");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");
        protected static readonly int DieHash = Animator.StringToHash("Die");
        protected const float TransitionSpeed = 0.1F;
        public NpcBaseState(Npc userNpc, Animator anim)
        {
            UserNpc = userNpc;
            Anim = anim;
        }
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnUpdate()
        {
        }
    }
}