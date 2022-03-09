using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerCharacterInteraction
{
    public float Speed;

    public event NotifyEnemyStateChange OnDefeated;

    private Vector2 _velocity;
    private CharacterMotor2D _motor2D;
    private int _moveDirection = 1;
    private void Start()
    {
        _motor2D = GetComponent<CharacterMotor2D>();
    }

    public void OnHit()
    {
        gameObject.SetActive(false);
        OnDefeated?.Invoke(this);
    }

    private void FixedUpdate()
    {
        _velocity.x = _moveDirection * Speed * Time.fixedDeltaTime;

        ApplyGravity();

        _motor2D.Move(_velocity);
        
        if (_motor2D.Collisions.BelowCount < _motor2D.Raycaster.VerticalRayCount 
            || _motor2D.Collisions.Right && _moveDirection == 1 
            || _motor2D.Collisions.Left && _moveDirection == -1)
        {
            _moveDirection = -_moveDirection;
        }
    }
    
    private void ApplyGravity()
    {
        if (!_motor2D.Collisions.Below)
        {
            _velocity.y += -9.8f * Time.fixedDeltaTime;
        }
    }

    public void Interact(PlayerCharacter character)
    {
        character.Die();
    }
}

public delegate void NotifyEnemyStateChange(Enemy enemy);
