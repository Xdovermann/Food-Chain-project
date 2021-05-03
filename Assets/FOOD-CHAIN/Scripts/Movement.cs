using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public CollisionManager coll;
    [HideInInspector]
    public Rigidbody2D rb;
    public AnimationManager animationManager;
    public StompTracker stompTracker;
    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float jumpForceHeadStomp = 25;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public int AmountOfJumps = 2;
    private int startAmountOfJumps;

   
    public bool canMove;
    public bool wallJumped;
    public bool wallGrab;
    [Space]
    [Header("Renderer")]
    public Transform PlayerRenderer;
    [Space]
    private bool groundTouch;
   

    public int side = 1;

   private float x;
   private float y;
    private BetterJumpManager betterJumpingController;
    public static Movement PlayerMovement;
    [Space]
    [Header("Polish")]
    public ParticleSystem DownParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    public WeaponManager weaponManager;
    private bool GroundSmash = false;
    private void Awake()
    {
        PlayerMovement = this;
    }


    void Start()
    {
        coll = GetComponent<CollisionManager>();
        rb = GetComponent<Rigidbody2D>();
        animationManager = GetComponentInChildren<AnimationManager>();
        betterJumpingController = GetComponent<BetterJumpManager>();
        startAmountOfJumps = AmountOfJumps;
    }

   
    void Update()
    {
         x = Input.GetAxisRaw("Horizontal");
         y = Input.GetAxisRaw("Vertical");
      
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        animationManager.SetHorizontalMovement(x, y, rb.velocity.y);



        if (coll.onGround )
        {
            wallJumped = false;
            betterJumpingController.enabled = true;
        }
    
        if(coll.onWall && !coll.onGround)
        {
            if (x != 0)
            {
                wallGrab = true;
                WallGrab();
            }
        }

        if (!coll.onWall || coll.onGround)
        {
            wallGrab = false;
            if (AmountOfJumps > 1)
            {
                animationManager.ResetRendererRotation();
            }
        }
           

        if (Input.GetKeyDown(KeyCode.W))
        {
            animationManager.ResetRendererRotation();
            // zetten jump anim wel ff uit geen zin om sprites voor te tekenen
            //  anim.SetTrigger("jump");
            if (AmountOfJumps == 1 && !coll.onGround && !coll.onWall)
            {
                // play rotation anim for double jump
                animationManager.DoubleJumpAnim();

            }

            if (AmountOfJumps > 0)
           
            Jump(Vector2.up, false);

         
            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        if (Input.GetKeyDown(KeyCode.S) && !coll.onGround && !wallGrab)
        {
           
                PushDown();
            
            
        }else
        if (Input.GetKeyUp(KeyCode.S))
        {
            StopPushDown();
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        WallParticle();

        //if (wallGrab || !canMove)
        //    return;
      
        //if(x > 0)
        //{
        //    side = 1;
        //    animationManager.Flip(side);
        //}
        //if (x < 0)
        //{
        //    side = -1;
        //    animationManager.Flip(side);
        //}


    }

    void GroundTouch()
    {

        AmountOfJumps = startAmountOfJumps;
        stompTracker.ResetCombo();

       // float shakeStrength = rb.velocity.y;
       // shakeStrength /= 2.5f;
     
      

        side = animationManager.sr.flipX ? -1 : 1;


        //    ShakePlayer();
        if (GroundSmash)
        {
            GameObject go = ObjectPooler.FlashEffect.GetObject();
            Vector2 SpawnPos = transform.position;
            SpawnPos.y -= 0.25f;
            go.transform.position = SpawnPos;
            go.SetActive(true);
            CameraController.cameraController.Shake(Vector2.down, 2.5f, 0.1f);

        
        }
        else
        {
            CameraController.cameraController.Shake(Vector2.down, 0.75f, 0.1f);

            PlayerRenderer.transform.localScale = new Vector3(1, 1, 1);

            PlayerRenderer.DOShakeScale(0.1f, 0.25f, 3, 25);
          //  holder.SetEase(Ease.InElastic);
        }
        

        jumpParticle.Play();
    }

    private void PushDown()
    {
        betterJumpingController.enabled = true;
        betterJumpingController.fallMultiplier = 10;
        GroundSmash = true;
        DownParticle.Play();

        // schiet een ray 
        // als dit een enemy hit 
        // disable je gravity op de player
        // takedamge op de ennemy
        //enable gravity weer na een tijdje of wanneer de  player weer omlaag duwt
        stompTracker.Stomping();
      
    }
    private void StopPushDown()
    {
        DownParticle.Stop();
        GroundSmash = false;
        betterJumpingController.fallMultiplier = betterJumpingController.StartMultiplier;
        stompTracker.StopStomping();
    }
   

    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            animationManager.Flip(side);
        }


        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        if(x == 0)
        {
            Jump((Vector2.up + wallDir / 5), true);

           
        }
        else
        {
            Jump((Vector2.up + wallDir / 10), true);
        }

        
        wallJumped = true;
    }

    private void WallGrab()
    {
        if(coll.wallSide != side)
         animationManager.Flip(side * -1);
        animationManager.WallGrabRotation();
        AmountOfJumps = startAmountOfJumps;
        stompTracker.ResetCombo();
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

      

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
          
        }
        else
        {
              rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);

        }
    }

    public void Jump(Vector2 dir, bool wall)
    {
        if (!wall &&!coll.onWall)
        {
            AmountOfJumps--;
        }
        
     
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
   
        rb.velocity += dir * jumpForce;

        CameraController.cameraController.Shake(Vector2.up, 1.5f, 0.1f);


        particle.Play();
    }

    public void HeadJump(Vector2 dir, bool wall)
    {

        AmountOfJumps = 1;
        animationManager.DoubleJumpAnim();

        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(0, 0);
        rb.velocity += dir * jumpForceHeadStomp;
        stompTracker.StopStomping();
        particle.Play();
    }

    void WallParticle()
    {
        var main = slideParticle.main;

        if (wallGrab)
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    private void ShakePlayer()
    {
        PlayerRenderer.transform.localScale = new Vector3(1, 1, 1);
        PlayerRenderer.DOShakeScale(0.1f);
    }
}
