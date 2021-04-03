using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Collider2D objectCollider;
    private Rigidbody2D rb;
    public enum GrabState
    {
        Throwable,
        NonThrowable
    }

    public GrabState grabState = GrabState.Throwable;

    private void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void GrabObject(Transform parent)
    {
        transform.SetParent(parent);
        DisablePhysics();
    }

    public void ThrowObject(Vector2 pos,Vector2 ThrowDirection)
    {
        transform.parent = null;
        transform.position = pos;
        objectCollider.enabled = true;
        rb.isKinematic = false;

        rb.AddForce(ThrowDirection * 25, ForceMode2D.Impulse);
    }

    public void DisablePhysics()
    {
        objectCollider.enabled = false;
        rb.isKinematic = true;
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
