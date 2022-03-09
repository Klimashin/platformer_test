using System;
using UnityEngine;

[RequireComponent(typeof(Raycaster))]
[RequireComponent(typeof(Collider2D))]
public class CharacterMotor2D : MonoBehaviour
{
    [Serializable]
    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public int BelowCount;

        public void Reset()
        {
            Above = Below = Left = Right = false;
            BelowCount = 0;
        }
    }
    
    public CollisionInfo Collisions => _collisionInfo;
    public Vector3 ExternalForce { get; set; }
    public Raycaster Raycaster => _raycaster;
    public Vector3 Velocity { get; private set; }

    private Raycaster _raycaster;
    private CollisionInfo _collisionInfo;

    private void Start()
    {
        _raycaster = GetComponent<Raycaster>();
        Velocity = Vector3.zero;
    }

    public void Move(Vector3 velocity)
    {
        _raycaster.UpdateRaycastOrigins();
        _collisionInfo.Reset();

        CheckForHorizontalCollision(ref velocity);
        CheckForVerticalCollision(ref velocity);

        if (!_collisionInfo.Below && velocity.y < 0f)
        {
            CheckForPlatformCollision(ref velocity);
        }
        
        Velocity = velocity + ExternalForce;
        
        transform.Translate(Velocity);

        ExternalForce = Vector3.zero;
    }

    private void CheckForHorizontalCollision(ref Vector3 velocity)
    {
        var directionX = (int)Mathf.Sign(velocity.x);
        var rayLength = Mathf.Abs(velocity.x) + Raycaster.SkinWidth;

        if (Mathf.Abs(velocity.x) < Raycaster.SkinWidth)
        {
            rayLength = 2 * Raycaster.SkinWidth;
        }

        for (var i = 0; i < _raycaster.HorizontalRayCount; i++)
        {
            var rayOrigin = directionX == -1 ? _raycaster.Origins.BottomLeft : _raycaster.Origins.BottomRight;
            rayOrigin += Vector2.up * (_raycaster.HorizontalRaySpacing * i);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _raycaster.GroundLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * (directionX * rayLength), Color.yellow);
            
            if (!hit || hit.distance == 0)
            {
                continue;
            }
                
            velocity.x = (hit.distance - Raycaster.SkinWidth) * directionX;

            _collisionInfo.Left = directionX == -1;
            _collisionInfo.Right = directionX == 1;
        }
    }

    private void CheckForVerticalCollision(ref Vector3 velocity)
    {
        var directionY = (int)Mathf.Sign(velocity.y);
        var rayLength = Mathf.Abs(velocity.y) + Raycaster.SkinWidth;

        for (var i = 0; i < _raycaster.VerticalRayCount; i++)
        {
            var rayOrigin = (directionY == -1) ? _raycaster.Origins.BottomLeft : _raycaster.Origins.TopLeft;
            rayOrigin += Vector2.right * (_raycaster.VerticalRaySpacing * i + velocity.x);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _raycaster.GroundLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * (directionY * rayLength), Color.yellow);

            if (!hit)
            {
                continue;
            }

            if (directionY == -1)
            {
                _collisionInfo.BelowCount++;
            }
            
            velocity.y = (hit.distance - Raycaster.SkinWidth) * directionY;

            _collisionInfo.Below = directionY == -1;
            _collisionInfo.Above = directionY == 1;
        }
    }
    
    private void CheckForPlatformCollision(ref Vector3 velocity)
    {
        var directionY = -1;
        var rayLength = Mathf.Abs(velocity.y) + Raycaster.SkinWidth;

        for (var i = 0; i < _raycaster.VerticalRayCount; i++)
        {
            var rayOrigin = _raycaster.Origins.BottomLeft;
            rayOrigin += Vector2.right * (_raycaster.VerticalRaySpacing * i + velocity.x);

            var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _raycaster.PlatformLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * (directionY * rayLength), Color.yellow);

            if (!hit)
            {
                continue;
            }
            
            _collisionInfo.Below = true;
            ExternalForce = hit.transform.GetComponent<Platform>().GetForce();
            
            velocity.y = (hit.distance - Raycaster.SkinWidth) * directionY;
        }
    }
}
