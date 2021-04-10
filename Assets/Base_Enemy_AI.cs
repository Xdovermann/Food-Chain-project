using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Enemy_AI : MonoBehaviour
{
    private Movement Player;
    public float AggroRange = 5f;

    public Transform Weapon;
    private Vector3 Target;
    private Vector3 AimVector;

    private Rigidbody2D rb;
    private ThrowableObject throwableObject;
    public enum AI_State
    {
        Idle,
        Moving,
        Aggro,
        Grabbed,
        Stunned
    }

    public AI_State enemyState = AI_State.Idle;
    private bool isGrabbed = false;
    public bool isThrown = false;
    public float StandUpTimer = 2f;
    public float holderStandUpTimer;

    // Start is called before the first frame update
    void Start()
    {
        Player = Movement.PlayerMovement;
        rb = GetComponent<Rigidbody2D>();
        throwableObject = GetComponent<ThrowableObject>();
        holderStandUpTimer = StandUpTimer;

        rb.freezeRotation = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case AI_State.Idle:

                CheckForAggro(false);

                break;
            case AI_State.Moving:

                CheckForAggro(false);

                break;
            case AI_State.Aggro:
                AimWeapon();
                break;

            case AI_State.Grabbed:

                isGrabbed = true;

                break;

            case AI_State.Stunned:
                CheckStunnedStatus();
                break;
            default:
                break;
        }
    }

    public void CheckForAggro(bool inAggro)
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if (!inAggro)
        {
      
            if (distance <= AggroRange)
            {
                enemyState = AI_State.Aggro;
            }
        }
        else
        {
            if (distance > AggroRange)
            {
                enemyState = AI_State.Idle;
            }
        }
       
    }



    private void AimWeapon()
    {
        Target = Player.transform.position;
        Target.z = transform.position.z;
        AimVector = (Target - transform.position).normalized; 
        float gunAngle = -1 * Mathf.Atan2(AimVector.y, AimVector.x) * Mathf.Rad2Deg; 
        Weapon.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back);

        CheckForAggro(true); // we zijn hier in aggro check of de player ook in aggro blijft
    }

    public void EnemyIsGrabbed()
    {
        enemyState = AI_State.Grabbed;
    }

    public void EnemyIsThrown()
    {
        isGrabbed = false;
        isThrown = true;
        rb.freezeRotation = false;

        isStunned();
    }

    public void isStunned()
    {
        enemyState = AI_State.Stunned;
        Debug.LogError("ENEMY IS STUNNED");
    }

    private void CheckStunnedStatus()
    {
        if (throwableObject.isThrown)
        {
            // do nothing cause object physics are still moving
        }
        else
        {
            enemyState = AI_State.Idle;
            isThrown = false;
            rb.freezeRotation = true;
            transform.localEulerAngles = new Vector2(0, 0);
        }
       
    }
  
}
