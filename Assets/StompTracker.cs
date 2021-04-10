using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompTracker : MonoBehaviour
{
    

    private Movement movement;
    private BetterJumpManager betterJump;
    public bool GoombaStomp = false;
    public int StompCombo;



    private void Awake()
    {
        movement = GetComponentInParent<Movement>();
        betterJump = GetComponentInParent<BetterJumpManager>(); 
       
    }

    public void Stomping()
    {
        GoombaStomp = true;
    }

    public void StopStomping()
    {
        GoombaStomp = false;
       
    }

    public void ResetCombo()
    {
        StompCombo = 1;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GoombaStomp)
            return;
        if (collision.CompareTag("Enemy") && !movement.coll.onGround)
        {
            movement.rb.velocity = new Vector2(0, 0);

          movement.HeadJump(Vector2.up, false);
            GoombaStomp = true;
     
            CameraController.cameraController.Shake(-Vector2.up, 2.5f, 0.1f);
            collision.GetComponent<Enemy>().TakeDamage(StompCombo,false);
            StompCombo++;
        }
    }
}
