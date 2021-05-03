using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [HideInInspector]
    public Collider2D objectCollider;

    [HideInInspector]
    public Rigidbody2D rb;

    private float StartingMass;

    public bool isThrown = false;

    private float ThrownTimer = 1f;
    public float PowerNeededForMovement = 1f;
    public float holderThrownTimer;

    public float PhysicsFallMultiplier = 1.5f;
    // private Transform Spawnparent;
    public enum GrabState
    {
        Throwable,
        NonThrowable,
      
    }

    public GrabState grabState = GrabState.Throwable;

    private void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        StartingMass = rb.mass;
        holderThrownTimer = ThrownTimer;
    }

    public void GrabObject(Transform parent)
    {
        transform.SetParent(parent);
        DisablePhysics();
        ResetThrownStatus();


    }

    public void ThrowObject(Vector2 pos,Vector2 ThrowDirection,bool isWeapon)
    {
        if (isWeapon)
        {
            transform.SetParent(WeaponGenerator.weaponGenerator.WeaponParent);
        }
        else
        {
            transform.SetParent(GameManager.gameManager.ItemParent);
        }
          
        
      
        transform.position = pos;
        objectCollider.enabled = true;
        rb.isKinematic = false;
        rb.mass = StartingMass;
        rb.AddForce(ThrowDirection * 25, ForceMode2D.Impulse);
        isThrown = true;
    }

    private void Update()
    {

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (PhysicsFallMultiplier - 1) * Time.deltaTime;
        }
        //else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        //{
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * (8 - 1) * Time.deltaTime;
        //}

        if (isThrown)
        {
            if (rb.velocity.x <= 0.1f && rb.velocity.y <= 0.1f && rb.angularVelocity <= 0.1f)
            {
                if (ThrownTimer <= 0)
                {
                    
                  
                    ResetThrownStatus();
                }
                else
                {
                    ThrownTimer -= Time.deltaTime;
                }

            }
            else
            {
                ThrownTimer = holderThrownTimer;
            }
        }
    }

    private void ResetThrownStatus()
    {

        isThrown = false;
        ThrownTimer = holderThrownTimer;
    }

    public void DisablePhysics()
    {
        objectCollider.enabled = false;
        rb.isKinematic = true;
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        rb.mass = 0;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && isThrown)
        {
            ThrowableObject throwable = collision.gameObject.GetComponent<ThrowableObject>();
            if (rb.velocity.x >= throwable.PowerNeededForMovement || rb.velocity.y >= throwable.PowerNeededForMovement)
            {
                throwable.isThrown = true;
              

                collision.gameObject.GetComponent<Base_Enemy_AI>().EnemyIsThrown();
              //  Vector2 dir = (transform.position - collision.transform.position).normalized;
             //   collision.gameObject.GetComponent<ThrowableObject>().ThrowObject(transform.position, dir, false);
               
            }
            else
            {
                Debug.Log("the thrown object hitting this one has not enough speed");
            }
       
        }
    }
}
