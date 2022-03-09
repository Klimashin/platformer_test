
public class CharStateFalling : CharacterStateBase
{
    public override void BuildTransitions()
    {
        AddTransition(CharacterStateTransition.FALL_TO_IDLE, CharacterStateID.IDLE);
        AddTransition(CharacterStateTransition.FALL_TO_JUMP, CharacterStateID.JUMP);
    }

    public override void FixedUpdate()
    {
        if (Parent.CharMotor.Collisions.Below)
        {
            MakeTransition(CharacterStateTransition.FALL_TO_IDLE);
            return;
        }
        
        var input = Parent.InputBuffer;
        if (input.IsJumpPressed && Parent.JumpCounter < 2)
        {
            MakeTransition(CharacterStateTransition.FALL_TO_JUMP);
            return;
        }
        
        var moveInput = input.MoveInput;
        Parent.Velocity.x = moveInput.x * Parent.MovementSettings.Speed;
    }
}

