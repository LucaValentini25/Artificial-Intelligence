using NPC;
using UnityEngine;

namespace FSM
{
    public class IdleState : NpcBaseState
    {
        public IdleState(Npc userNpc, Animator anim) : base(userNpc, anim)
        {
        }

        public override void OnEnter()
        {
            Anim.CrossFade(IdleHash, TransitionSpeed);
        }

        public override void OnExit()
        {
    
        }

        public override void OnUpdate()
        {
     
        }
    }
}
