using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{

    private Animator anim;
    private Movement move;
    private CollisionManager coll;

    [HideInInspector]
    public SpriteRenderer sr;

    private int Side;

    public float wallGrabAngle = 15;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<CollisionManager>();
        move = GetComponentInParent<Movement>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("wallSlide", move.wallGrab);
        anim.SetBool("canMove", move.canMove);
   

    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);

        // handelt fall animation hebben we tijdelijk nie nodig want heb geen zin om dat te tekenen
      //  anim.SetFloat("VerticalAxis", y);
      //  anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {
        Side = side;

        if (move.wallGrab)
        {
            if (Side == -1 && sr.flipX)
                return;

            if (Side == 1 && !sr.flipX)
            {
                return;
            }
        }

        bool state = (Side == 1) ? false : true;
        sr.flipX = state;
    }

    public void DoubleJumpAnim()
    {

        ResetRendererRotation();
        float RotationSpeed = 0.35f;

        Tween holder;
        if (Side == 1)
        {
            holder =sr.transform.DORotate(new Vector3(sr.transform.localEulerAngles.x, sr.transform.localEulerAngles.y, -360), RotationSpeed, RotateMode.FastBeyond360);
           
        }
        else
        {
             holder=  sr.transform.DORotate(new Vector3(sr.transform.localEulerAngles.x, sr.transform.localEulerAngles.y, 360), RotationSpeed, RotateMode.FastBeyond360);
          
        }

        GameObject go = ObjectPooler.FlashEffect.GetObject();
        go.transform.position = transform.position;
        go.transform.localScale = new Vector2(1f, 1f);
        go.SetActive(true);

         holder.SetEase(Ease.Linear);
    }

    public void WallGrabRotation()
    {
        float RotationSpeed = 0.1f;

        if (Side == 1)
        {
            sr.transform.DORotate(new Vector3(sr.transform.localEulerAngles.x, sr.transform.localEulerAngles.y, -wallGrabAngle), RotationSpeed, RotateMode.Fast);
        }
        else
        {
            sr.transform.DORotate(new Vector3(sr.transform.localEulerAngles.x, sr.transform.localEulerAngles.y, wallGrabAngle), RotationSpeed, RotateMode.Fast);
        }
    }

    public void ResetRendererRotation()
    {
        sr.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
