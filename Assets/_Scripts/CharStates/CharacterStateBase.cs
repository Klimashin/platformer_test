using Overtime.FSM;

public enum CharacterStateID
{
    IDLE,
    RUN,
    JUMP,
    FALL
}

public enum CharacterStateTransition
{
    IDLE_TO_RUN,
    RUN_TO_IDLE,
    IDLE_TO_JUMP,
    RUN_TO_JUMP,
    JUMP_TO_FALL,
    FALL_TO_IDLE,
    RUN_TO_FALL,
    IDLE_TO_FALL,
    JUMP_TO_JUMP,
    FALL_TO_JUMP
}

public abstract class CharacterStateBase : State<PlayerCharacter, CharacterStateID, CharacterStateTransition>
{
    public override void BuildTransitions ()
    {
		
    }

    public override void Enter ()
    {
		
    }

    public override void Exit ()
    {
		
    }

    public override void FixedUpdate ()
    {
		
    }

    public override void Update ()
    {
		
    }
}
