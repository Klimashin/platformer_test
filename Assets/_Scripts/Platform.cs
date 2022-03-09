using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformSettings PlatformSettings;

    public Vector3 GetForce() => _externalForce;

    private void Start()
    {
        StartCoroutine(PlatformMoveCoroutine());
    }
    
    private Vector3 _externalForce = Vector3.zero;
    private IEnumerator PlatformMoveCoroutine()
    {
        var startPos = transform.position;
        var endPos = startPos + new Vector3(0f, PlatformSettings.MaxHeight, 0f);
        
        while (true)
        {
            _externalForce = Vector3.zero;
            yield return new WaitForSecondsRealtime(PlatformSettings.WaitTime);
            
            yield return MoveTo(endPos);

            _externalForce = Vector3.zero;
            yield return new WaitForSecondsRealtime(PlatformSettings.WaitTime);
            
            yield return MoveTo(startPos);
        }
    }

    private IEnumerator MoveTo(Vector3 endPos)
    {
        while (!transform.position.y.Equals(endPos.y))
        {
            var prevPos = transform.position;
            transform.position = Vector3.MoveTowards(prevPos, endPos, PlatformSettings.Speed * Time.fixedDeltaTime);
            _externalForce = transform.position - prevPos;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        var startPos = transform.position;
        var endPos = startPos + new Vector3(0f, PlatformSettings.MaxHeight, 0f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, 0.5f);
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawSphere(endPos, 0.5f);
    }
}

[System.Serializable]
public class PlatformSettings
{
    public float Speed;
    public float WaitTime;
    public float MaxHeight;
}
