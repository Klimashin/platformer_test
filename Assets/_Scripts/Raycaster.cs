using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _interactionsLayer;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private int _horizontalRayCount = 4;
    [SerializeField] private int _verticalRayCount = 4;
    
    [System.Serializable]
    public struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }

    public const float SkinWidth = 0.015f;
    
    public RaycastOrigins Origins => _raycastOrigins;
    public LayerMask GroundLayer => _groundLayer;
    public LayerMask PlatformLayer => _platformLayer;
    public int HorizontalRayCount => _horizontalRayCount;
    public int VerticalRayCount => _verticalRayCount;
    public float HorizontalRaySpacing => _horizontalRaySpacing;
    public float VerticalRaySpacing => _verticalRaySpacing;

    private Collider2D _collider;
    private RaycastOrigins _raycastOrigins;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        CalculateRaySpacing();
    }

    private readonly IReadOnlyList<IPlayerCharacterInteraction> _interactionsCastEmptyResult = new List<IPlayerCharacterInteraction>(); 
    public IReadOnlyList<IPlayerCharacterInteraction> GetInteractions()
    {
        var hitResults = new List<RaycastHit2D>();
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_interactionsLayer);
        _collider.Cast(Vector2.zero, contactFilter, hitResults);
        if (hitResults.Count == 0)
        {
            return _interactionsCastEmptyResult;
        }

        return hitResults
            .FindAll(hit => hit.transform.GetComponent<IPlayerCharacterInteraction>() != null)
            .Select(h => h.transform.GetComponent<IPlayerCharacterInteraction>())
            .ToList();
    }

    public void UpdateRaycastOrigins()
    {
        var bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private const int MinRays = 2;
    private void CalculateRaySpacing()
    {
        var bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);
        
        _horizontalRayCount = Mathf.Max(_horizontalRayCount, MinRays);
        _verticalRayCount = Mathf.Max(_verticalRayCount, MinRays);

        _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
    }
}

public interface IPlayerCharacterInteraction
{
    void Interact(PlayerCharacter character);
}
