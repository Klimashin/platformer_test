using System.Collections.Generic;
using Overtime.FSM;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterMotor2D))]
public class PlayerCharacter : MonoBehaviour, InputActions.ICharacterActions
{
    public MovementSettings MovementSettings;
    public JumpSettings JumpSettings;
    public ShootSettings ShootSettings;
    public StateMachineSettings StateMachineSettings;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject WinMenu;

    public InputBuffer InputBuffer { get; private set; }
    public CharacterMotor2D CharMotor { get; private set; }
    public float JumpSpeed => Mathf.Abs(_gravity) * JumpSettings.TimeToApex;
    public Vector2 Velocity;
    public int JumpCounter;
    
    [ShowInInspector]
    public string CurrentState => Application.isPlaying ? _stateMachine?.CurrentStateName : "";

    [ShowInInspector] public List<Item> Inventory { get; } = new List<Item>();


    private InputActions _inputActions;
    private float _gravity;
    private SpriteRenderer _renderer;
    private Raycaster _raycaster;
    private StateMachine<PlayerCharacter, CharacterStateID, CharacterStateTransition> _stateMachine;
    private float _missileShotCooldown;
    private Vector3 _checkpointPosition;

    private void Start()
    {
        InputBuffer = new InputBuffer();
        CharMotor = GetComponent<CharacterMotor2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _raycaster = GetComponent<Raycaster>();

        _inputActions = GameManager.Instance.InputActions;
        _inputActions.Character.SetCallbacks(this);
        _inputActions.Character.Enable();

        _gravity = - 2 * JumpSettings.MaxHeight / Mathf.Pow(JumpSettings.TimeToApex, 2);

        _checkpointPosition = transform.position;
        
        _stateMachine = new StateMachine<PlayerCharacter, CharacterStateID, CharacterStateTransition>(this,
            StateMachineSettings.States, StateMachineSettings.InitialState, StateMachineSettings.Debug);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputBuffer.MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        InputBuffer.IsJumpPressed = context.performed;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        InputBuffer.IsShootPressed = context.performed;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseMenu.SetActive(true);
        }
    }

    public void Die()
    {
        //transform.position = _checkpointPosition;
        gameObject.SetActive(false);
        _inputActions.Character.Disable();
        GameOverMenu.gameObject.SetActive(true);
    }

    public void Win()
    {
        _inputActions.Character.Disable();
        WinMenu.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
        
        ApplyGravity();

        CharMotor.Move(Velocity * Time.fixedDeltaTime);

        SetRendererDirection();

        ObjectsInteractions();

        _missileShotCooldown -= Time.fixedDeltaTime;
        if (InputBuffer.IsShootPressed && _missileShotCooldown <= 0f)
        {
            _missileShotCooldown = ShootSettings.MissileShotCooldown;
            Shoot();
        }
        
        InputBuffer.Flush();
    }
    
    private void Shoot()
    {
        var lookDirection = _renderer.flipX ? 1 : -1;
        var missile = Instantiate(ShootSettings.MissilePrefab);
        missile.transform.position = ShootSettings.MissileLaunchOrigin.position;
        missile.Shoot(new Vector3(lookDirection, 0f, 0f));
    }

    private void ObjectsInteractions()
    {
        var interactableObjects = _raycaster.GetInteractions();
        if (interactableObjects.Count <= 0)
        {
            return;
        }

        foreach (var playerCharacterInteraction in interactableObjects)
        {
            playerCharacterInteraction.Interact(this);
        }
    }

    private void SetRendererDirection()
    {
        if (Mathf.Abs(Velocity.x) > 0f)
        {
            _renderer.flipX = Velocity.x > 0;
        }
    }

    private void ApplyGravity()
    {
        if (!CharMotor.Collisions.Below)
        {
            Velocity.y += _gravity * Time.fixedDeltaTime;
        }
    }
}

public class InputBuffer
{
    public Vector2 MoveInput = Vector2.zero;
    public bool IsJumpPressed;
    public bool IsShootPressed;

    public void Flush()
    {
        IsJumpPressed = false;
        IsShootPressed = false;
    }

    public void UtilizeJumpPressed()
    {
        IsJumpPressed = false;
    }
}

[System.Serializable]
public class StateMachineSettings
{
    public CharacterStateID InitialState;
    public ScriptableObject[] States;
    public bool Debug;
}

[System.Serializable]
public class MovementSettings
{
    public float Speed = 8;
}

[System.Serializable]
public class JumpSettings
{
    public float MaxHeight = 3.5f;
    public float TimeToApex = 0.4f;
}

[System.Serializable]
public class ShootSettings
{
    public Missile MissilePrefab;
    public Transform MissileLaunchOrigin;
    public float MissileShotCooldown = 0.5f;
}
