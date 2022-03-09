using UnityEngine;

namespace Gameplay
{
    public class CharStateRunning : CharacterStateBase
    {
        public override void BuildTransitions()
        {
            AddTransition(CharacterStateTransition.RUN_TO_IDLE, CharacterStateID.IDLE);
            AddTransition(CharacterStateTransition.RUN_TO_JUMP, CharacterStateID.JUMP);
            AddTransition(CharacterStateTransition.RUN_TO_FALL, CharacterStateID.FALL);
        }
        
        public override void FixedUpdate()
        {
            if (!Parent.CharMotor.Collisions.Below)
            {
                MakeTransition(CharacterStateTransition.RUN_TO_FALL);
                return;
            }
            
            var input = Parent.InputBuffer;
            if (input.IsJumpPressed)
            {
                MakeTransition(CharacterStateTransition.RUN_TO_JUMP);
                return;
            }
            
            var moveInput = input.MoveInput;
            var velocity = Parent.MovementSettings.Speed;
            Parent.Velocity.x = moveInput.x * velocity;

            if (Mathf.Abs(Parent.Velocity.x) <= float.Epsilon)
            {
                MakeTransition(CharacterStateTransition.RUN_TO_IDLE);
            }
        }
    }
}
