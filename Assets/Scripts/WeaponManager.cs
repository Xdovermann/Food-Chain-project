using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
    private PlayerController player;

    public float moveSpeed = 10;
    public float rotationSpeed = 10;

    public int playerSortingOrder = 20;

    public Transform Weapon;
    public SpriteRenderer weaponRenderer;

    public float angle;
    Vector3 positionOnScreen;
    Vector3 mouseOnScreen;
    private void Start()
    {
        player = PlayerController.player;
     
    }

    
    private void Update()
    {
        GetMouseInput();
        Animation();

       
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime);
    }

    private void GetMouseInput()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = transform.position.z;
        //mouseVector = (mousePos - transform.position).normalized;

         positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
         mouseOnScreen = (Vector3)Camera.main.ScreenToViewportPoint(Input.mousePosition);
      




    }



    private void Animation()
    {

        // 2d wapens
        angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        angle += 180;
        Weapon.localEulerAngles = new Vector3(Weapon.localEulerAngles.x, Weapon.localEulerAngles.y, angle);

        // 3d wapens methode als ik voor de 3d models zou gaan
       // angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
      //  Weapon.localEulerAngles = new Vector3(Weapon.localEulerAngles.x, angle, Weapon.localEulerAngles.z);



        if (angle >= 180 && angle <= 360)
        {
            weaponRenderer.sortingOrder = 1;
        }
        else
        {
            weaponRenderer.sortingOrder = -1;
        }

        if (angle >= 90 && angle<=270)
        {
          
            player.FlipCharacter(1);
            FlipWeapon(1);
        }
        else 
        {
    
            player.FlipCharacter(0);
            FlipWeapon(0);
        }
    }

    private void FlipWeapon(int side)
    {
        if(side == 0)
        {
            weaponRenderer.flipY = false;
        }
        else if(side == 1)
        {
            weaponRenderer.flipY = true;
        }
       
    }

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
