using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float Lifetime;
    public float Speed;
    public LayerMask EnemyMask;
    public Collider2D Collider2D;
    
    public void Shoot(Vector3 direction)
    {
        StartCoroutine(ShootCoroutine(direction));
    }

    private IEnumerator ShootCoroutine(Vector3 direction)
    {
        var lifetime = Lifetime;

        while (lifetime > 0f)
        {
            lifetime -= Time.fixedDeltaTime;
            transform.Translate(direction * (Time.fixedDeltaTime * Speed));

            if (HitCheck())
            {
                Destroy(gameObject);
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
        
        Destroy(gameObject);
    }

    private bool HitCheck()
    {
        var hitResults = new List<RaycastHit2D>();
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(EnemyMask);
        Collider2D.Cast(Vector2.zero, contactFilter, hitResults);
        if (hitResults.Count == 0)
        {
            return false;
        }

        if (!hitResults[0].transform.TryGetComponent<Enemy>(out var enemy))
        {
            return false;
        }

        enemy.OnHit();
        return true;
    }
}
