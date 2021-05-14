using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask CollidbleLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public bool WallGrab;
    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisions();
        CheckWallGrab();
    }

    public void CheckCollisions()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, CollidbleLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, CollidbleLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, CollidbleLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, CollidbleLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, CollidbleLayer);

        wallSide = onRightWall ? -1 : 1;

       
    }

    private void CheckWallGrab()
    {
        if (onWall)
        {
            if (movement.x != 0)
            {
                WallGrab = true;
            }
            else
            {
                WallGrab = false;
            }
        }
        else
        {
            WallGrab = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugCollisionColor;

      

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
