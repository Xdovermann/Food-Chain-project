using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
   

    public float moveSpeed = 10;
    public float rotationSpeed = 10;

    public int playerSortingOrder = 20;

    public Transform Weapon;
    public SpriteRenderer weaponRenderer;

    public float angle;
    Vector3 positionOnScreen;
    Vector3 mouseOnScreen;

    public float Offset = 0;
    public int WeaponSide = 1;


    private Movement playerMovement;

    private void Awake()
    {
        playerMovement = Movement.PlayerMovement;
    }

    private void Update()
    {
        GetMouseInput();
        Animation();

       
    }

 
    private void GetMouseInput()
    {
         positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
         mouseOnScreen = (Vector3)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {

                
         
        }




    }



    private void Animation()
    {

        angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        angle += 180;  
        Weapon.localEulerAngles = new Vector3(Weapon.localEulerAngles.x, Weapon.localEulerAngles.y, angle);



        if (angle >= 180 && angle <= 360)
        {
            weaponRenderer.sortingOrder = -1;
        }
        else
        {
            weaponRenderer.sortingOrder = 0;
        }

        PlayerDirection();


    }

  

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void PlayerDirection()
    {
        if (playerMovement.wallGrab || !playerMovement.canMove)
            return;

        Vector3 Pos = mouseOnScreen;
        Pos -= positionOnScreen;

        if (Pos.x > 0)
        {
            playerMovement.side = 1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRenderer.flipY = false;
        }
        else if (Pos.x < 0)
        {
            playerMovement.side = -1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRenderer.flipY = true;
        }
    }
}
