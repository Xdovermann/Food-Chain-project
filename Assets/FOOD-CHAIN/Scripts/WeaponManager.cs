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
    private void Start()
    {
        
    }

    
    private void Update()
    {
        GetMouseInput();
        Animation();

       
    }

    private void FixedUpdate()
    {
       // transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime);
    }

    private void GetMouseInput()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = transform.position.z;
        //mouseVector = (mousePos - transform.position).normalized;

         positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
         mouseOnScreen = (Vector3)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {

            //  weaponRenderer.transform.DOShakeScale(0.1f);
           // Offset += 75;
            if (WeaponSide == 1)
            {
                float pos = transform.position.x;
                pos =-0.5f;

                //  transform.DOLocalMoveX(pos, 0.1f);
          
                WeaponSide = 0;
            }
            else
            {
                float pos = transform.position.x;
                pos = 0.5f;

           //   transform.DOLocalMoveX(pos, 0.1f);
             
                WeaponSide = 1;
            }
    
     
            //CameraController.cameraController.Shake()
        }




    }



    private void Animation()
    {

        // 2d wapens
        angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        //angle *= -1;
        angle += 180;
       // angle += Offset;
      //  Weapon.localEulerAngles = new Vector3(Weapon.localEulerAngles.x, angle, Weapon.localEulerAngles.z );
          // Weapon.DOLocalRotate(new Vector3(Weapon.localEulerAngles.y,  Weapon.localEulerAngles.z, ), 0.25f);
        // 3d wapens methode als ik voor de 3d models zou gaan
        // angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
          Weapon.localEulerAngles = new Vector3(Weapon.localEulerAngles.x, Weapon.localEulerAngles.y, angle);



        if (angle >= 180 && angle <= 360)
        {
            weaponRenderer.sortingOrder = -1;
        }
        else
        {
            weaponRenderer.sortingOrder = 0;
        }

        if (angle >= 90 && angle<=270)
        {
          
           // player.FlipCharacter(1);
        
            FlipWeapon(1);
        }
        else 
        {
    
        //    player.FlipCharacter(0);
            FlipWeapon(0);
        }
    }

    private void FlipWeapon(int side)
    {
        if(side == 0)
        {
           // weaponRenderer.flipY = false;
            float pos = weaponRenderer.transform.position.x;
            pos = -0.65f;

        //  weaponRenderer.transform.DOLocalMoveX(pos, 0.1f);
           
        }
        else if(side == 1)
        {
            float pos = weaponRenderer.transform.position.x;
            pos = 0.65f;

     //   weaponRenderer.transform.DOLocalMoveX(pos, 0.1f);
  //
           // weaponRenderer.flipY = true;
        }
       
    }

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
