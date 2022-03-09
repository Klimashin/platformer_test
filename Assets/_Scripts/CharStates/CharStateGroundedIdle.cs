using UnityEngine;

namespace Gameplay
{
    public class CharStateGroundedIdle : CharacterStateBase
    {
        public override void BuildTransitions()
        {
            AddTransition(CharacterStateTransition.IDLE_TO_RUN, CharacterStateID.RUN);
            AddTransition(CharacterStateTransition.IDLE_TO_JUMP, CharacterStateID.JUMP);
            AddTransition(CharacterStateTransition.IDLE_TO_FALL, CharacterStateID.FALL);
        }

        public override void Enter()
        {
            Parent.Velocity.x = 0;
            Parent.JumpCounter = 0;
        }

        public override void FixedUpdate()
        {
            if (!Parent.CharMotor.Collisions.Below)
            {
                MakeTransition(CharacterStateTransition.IDLE_TO_FALL);
                return;
            }
            
            var input = Parent.InputBuffer;
            if (input.IsJumpPressed)
            {
                MakeTransition(CharacterStateTransition.IDLE_TO_JUMP);
                return;
            }
            
            if (Mathf.Abs(input.MoveInput.x) > 0f)
            {
                MakeTransition(CharacterStateTransition.IDLE_TO_RUN);
                return;
            }
        }
    }
}
