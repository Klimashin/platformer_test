namespace Gameplay
{
    public class CharStateJumping : CharacterStateBase
    {
        public override void BuildTransitions()
        {
            AddTransition(CharacterStateTransition.JUMP_TO_FALL, CharacterStateID.FALL);
        }

        public override void Enter()
        {
            Parent.Velocity.y = Parent.JumpSpeed;
            Parent.InputBuffer.UtilizeJumpPressed();
            Parent.JumpCounter++;
        }

        public override void FixedUpdate()
        {
            var input = Parent.InputBuffer;
            var jumpEnded = Parent.CharMotor.Velocity.y <= 0f;
            
            if (jumpEnded)
            {
                MakeTransition(CharacterStateTransition.JUMP_TO_FALL);
                return;
            }

            if (input.IsJumpPressed && Parent.JumpCounter < 2)
            {
                Parent.Velocity.y = Parent.JumpSpeed;
                Parent.InputBuffer.UtilizeJumpPressed();
                Parent.JumpCounter++;
            }

            var moveInput = input.MoveInput;
            Parent.Velocity.x = moveInput.x * Parent.MovementSettings.Speed;
        }
        
    }
}
