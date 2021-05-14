using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FoodChain.BulletML;

public class Base_Enemy_AI : MonoBehaviour
{
    private Movement Player;
    public float AggroRange = 5f;

    public Transform Weapon;
    public SpriteRenderer WeaponRenderer;
    public SpriteRenderer EnemyRenderer;

    public float AttackTimer = 1;
    private float AttackTimerHolder;

    public BulletSourceScript weaponEmitter;
    public GameObject ShotEffect;


    private float OffsetY;

    public float WeaponHandling=0.1f;
    private Vector3 Target;
    private Vector3 AimVector;

    private Rigidbody2D rb;
    private ThrowableObject throwableObject;
    private float OriginalWeaponPos;

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

    public float Speed;
    int MoveDirection =1;

    private Coroutine IdleRoutine;

    public LayerMask ClimbLayer;
    public float WalkTimer;
    private float StartWalkTimer;

    public float JumpTimer = 1f;
    private float JumpTimerHolder;
    private bool wallGrab =false;
    private CollisionManager_Enemy CollManager;
    public float JumpPower;

    private float TopCollTimer;

    private bool StartAI = false;


    private float WeaponAngle;
    private Sequence ShootMovementSequence;
    // Start is called before the first frame update
    void Start()
    {
        Player = Movement.PlayerMovement;
        rb = GetComponent<Rigidbody2D>();
        throwableObject = GetComponent<ThrowableObject>();

        OriginalWeaponPos = WeaponRenderer.transform.localPosition.x;

        OffsetY = Weapon.transform.localPosition.y;

        CollManager = GetComponent<CollisionManager_Enemy>();

        AttackTimerHolder = AttackTimer;
        weaponEmitter.SetUpEmitterOnStart();
        DoneShootingPattern();

        holderStandUpTimer = StandUpTimer;
        JumpTimerHolder = JumpTimer;
        StartWalkTimer = WalkTimer;
        rb.freezeRotation = true;

        StartCoroutine(EnableAI());
    }

    private IEnumerator EnableAI()
    {
        float rand = Random.Range(0, 2f);
        yield return new WaitForSeconds(rand);
        StartAI = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponPos();

       

        if (!StartAI)
            return;

        if (weaponEmitter.IsEnded && weaponEmitter.gameObject.activeInHierarchy)
        {
            weaponEmitter.gameObject.SetActive(false);
        }

        switch (enemyState)
        {
            case AI_State.Idle:

                

                CheckForAggro(false);

                SimpleMovement();



                break;
            case AI_State.Moving:

                CheckForAggro(false);

                SimpleMovement();

                break;
            case AI_State.Aggro:
                AimWeapon();
                WeaponFlipAnimations();
                SimpleMovement();
                AttackHandler();
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

    private void SimpleMovement()
    {
        if (wallGrab) // climbing
        {
            // als ai omhoog klimt en blijft hangen
            if (CollManager.HittingTop) 
            {
                if (TopCollTimer <= 0)
                {
                                   
                    TopCollTimer = 1;

                    CheckWallDirection();

                    wallGrab = false;

                    return;
                }
                else
                {
                    TopCollTimer -= Time.deltaTime;
                }
            }
            else
            {
                TopCollTimer = 1;
            }
           
           // rb.velocity = new Vector2(0, 0);
            rb.velocity = new Vector2(0, 5);

            if (!CollManager.onWall && !CollManager.onGround)
            {             
                wallGrab = false;
            }
        }
        else // normal movemment
        {
            CheckPlatformEdges();


            if (!CollManager.onLeftWall || !CollManager.onRightWall && CollManager.onGround)
            {
                if (WalkTimer > 0)
                {
                    MoveRandom(MoveDirection);
                    WalkTimer -= Time.deltaTime;
                }
                else if (WalkTimer <= 0 && IdleRoutine == null)
                {
                    // idle
                    IdleRoutine = StartCoroutine(Idle());

                }
            }

            if (IdleRoutine == null)
            {
                if (JumpTimer <= 0)
                {
                    CheckForWall();
                    JumpTimer = JumpTimerHolder;
                }
                else
                {


                    JumpTimer -= Time.deltaTime;
                }
            }
        }

       
       
      

    }

    private void AttackHandler()
    {
        if(weaponEmitter.gameObject.activeInHierarchy == false)
        {
            if (AttackTimer <= 0)
            {
         
                ShootPattern();
            }
            else
            {
                AttackTimer -= Time.deltaTime;
            }
        }
      
    }

    private void ShootPattern()
    {
        
        weaponEmitter.gameObject.SetActive(true);

        WeaponJuice();

        weaponEmitter.Reset();

    }

    private void WeaponJuice()
    {
        // camerashake
        float power = Random.Range(2.5f, 3f);
        float randTime = Random.Range(0.05f, 0.1f);

        //weaponscale
        WeaponRenderer.transform.localScale = new Vector3(1, 1, 1);
       

        // weaponknockback
        ShootMovementSequence = DOTween.Sequence();
        float MovePoint = OriginalWeaponPos;
        MovePoint -= Random.Range(0.25f, 0.5f);

        ShootMovementSequence.Append(WeaponRenderer.transform.DOShakeScale(0.1f));
        ShootMovementSequence.Append(WeaponRenderer.transform.DOLocalMoveX(MovePoint, 0.1f));
        ShootMovementSequence.Append(WeaponRenderer.transform.DOLocalMoveX(OriginalWeaponPos, 0.1f));
   
        if (ShotEffect != null)
        {
            GameObject Effect = Instantiate(ShotEffect);
            Effect.transform.position = weaponEmitter.transform.position;
            Effect.SetActive(true);
            Effect.transform.DOShakeScale(0.1f, 0.5f);

        }
    }
        private IEnumerator Idle()
    {
        rb.velocity = new Vector2(0, 0);
        float rand = Random.Range(0.25f, 1f);
        MoveDirection = 0;
        yield return new WaitForSeconds(rand);
        int randDir = Random.Range(-1, 2);
        MoveDirection = randDir;
        WalkTimer = StartWalkTimer;
        IdleRoutine = null;
    }

    private void CheckForWall()
    {
        if(CollManager.HittingTop == false)
        {
            if (CollManager.onWall)
            {

                WallGrab();
              
            }
            else
            {
                CheckWallDirection();
            }
        }
        else
        {
            CheckWallDirection();
        }
      
        
            
    }


    private void WallGrab()
    {       
            wallGrab = true;
    }

    public void Jump(Vector2 dir)
    {

        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.velocity += dir * JumpPower;      

    }



    private void CheckPlatformEdges()
    {
      

            //if (CollManager.LeftPlatformChecker == false) // is link op de edge
            //{

            //    MoveDirection = 1;
            //}
            //else if (CollManager.RightPlatformChecker == false) // is rechts op de edge
            //{
            //    MoveDirection = -1;
            //}
        

    }
   
    private void CheckWallDirection()
    {
        if (CollManager.onRightWall)
        {
            MoveDirection = -1;
        }
        else if (CollManager.onLeftWall)
        {
            MoveDirection = 1;
        }
    }

    private void MoveRandom(int dir)
    {
        if (wallGrab)
            return;

        rb.velocity = new Vector2(dir * Speed*Time.fixedDeltaTime, rb.velocity.y);
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

    private void UpdateWeaponPos()
    {

        Weapon.transform.position = Vector3.Lerp(Weapon.transform.position, transform.position, WeaponHandling * Time.deltaTime);
        Weapon.rotation = Quaternion.AngleAxis(WeaponAngle, Vector3.back);
    }

  

    private void WeaponFlipAnimations()
    {
        if(Target.x >= transform.position.x)
        {
            // rechts aimen 
            WeaponRenderer.flipY = false;
            EnemyRenderer.flipX = false;

            OffsetY *= 1;
            Vector2 hold = WeaponRenderer.transform.localPosition ;
            hold.y = OffsetY;
            WeaponRenderer.transform.localPosition = hold;

        }
        else if(Target.x <= transform.position.x)
        {
            WeaponRenderer.flipY = true;
            EnemyRenderer.flipX = true;
            OffsetY *= 1;
            Vector2 hold = WeaponRenderer.transform.localPosition;
            hold.y = OffsetY;
            WeaponRenderer.transform.localPosition = hold;
        }
    }

    private void AimWeapon()
    {
        Target = Player.transform.position;
        Target.z = transform.position.z;
        AimVector = (Target - transform.position).normalized;
        WeaponAngle = -1 * Mathf.Atan2(AimVector.y, AimVector.x) * Mathf.Rad2Deg; 
   

        if (WeaponAngle >= 0 && WeaponAngle <= 180)
        {
            // weaponRendererParent.sortingOrder = -1;
            HigherSprites();

        }
        else
        {
            //  weaponRendererParent.sortingOrder = 0;
            LowerSprites();

        }

        CheckForAggro(true); // we zijn hier in aggro check of de player ook in aggro blijft
    }

    private void HigherSprites()
    {
        if (WeaponRenderer.sortingLayerName == "Weapon") // check of renderer al gezet is 
            return;

        WeaponRenderer.sortingLayerName = "Weapon";
    }
    private void LowerSprites()
    {
        if (WeaponRenderer.sortingLayerName == "Player") // check of renderer al gezet is 
            return;

        WeaponRenderer.sortingLayerName = "Player";
    }

    public void DoneShootingPattern()
    {
        weaponEmitter.gameObject.SetActive(false);
        AttackTimer = AttackTimerHolder;
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

    private void OnDestroy()
    {
        ShootMovementSequence.Kill();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, AggroRange);

        
    }

}
