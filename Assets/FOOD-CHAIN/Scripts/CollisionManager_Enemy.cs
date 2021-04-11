using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager_Enemy : CollisionManager
{
    [Space(10)]
    public bool RightPlatformChecker;
    public bool LeftPlatformChecker;
    public bool HittingTop;
    public LayerMask PlatformLayer;
    public Vector2 RightPlatformOffest, LeftPlatformOffset,TopOffest;

    private void Update()
    {
        CheckCollisions();

        HittingTop = Physics2D.OverlapCircle((Vector2)transform.position + TopOffest, collisionRadius, PlatformLayer);
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + TopOffest, Vector2.up, 3f, PlatformLayer);
        //HittingTop = hit;

        RightPlatformChecker = Physics2D.OverlapCircle((Vector2)transform.position + RightPlatformOffest, collisionRadius, PlatformLayer);
        LeftPlatformChecker = Physics2D.OverlapCircle((Vector2)transform.position + LeftPlatformOffset, collisionRadius, PlatformLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + TopOffest, collisionRadius);
  
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + RightPlatformOffest, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftPlatformOffset, collisionRadius);

    }
}
