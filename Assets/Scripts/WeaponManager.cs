using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
    private PlayerController player;

    public float moveSpeed = 10;
    public float rotationSpeed = 10;
    private Vector3 mousePos;
    private Vector3 mouseVector;

    public int playerSortingOrder = 20;

    public Transform Weapon;
    public SpriteRenderer weaponRenderer;

    private void Start()
    {
        player = PlayerController.player;
     
    }

    
    private void Update()
    {
        GetMouseInput();
        Animation();

        transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void GetMouseInput()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mouseVector = (mousePos - transform.position).normalized;

    }



    private void Animation()
    {
      
        float gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        Quaternion holder = Quaternion.AngleAxis(gunAngle, Vector3.back);

        Weapon.rotation = Quaternion.Slerp(Weapon.rotation, holder,rotationSpeed *Time.deltaTime);

        weaponRenderer.sortingOrder = playerSortingOrder - 1;
        if (gunAngle > 0)
        {
            weaponRenderer.sortingOrder = playerSortingOrder + 1;
        }

        if (mouseVector.x >= 0 )
        {
            player.FlipCharacter(0);
            FlipWeapon(0);
        }
        else if (mouseVector.x <= 0 )
        {
            player.FlipCharacter(1);
            FlipWeapon(1);
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
}
