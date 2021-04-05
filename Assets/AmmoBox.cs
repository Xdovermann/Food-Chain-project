using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmmoBox : ThrowableObject
{
    // als player in grabrange is move dan naar de player toe
    // als zijn ammo vol is van zijn current weapon blijf still liggen

    private Movement Player;
    private WeaponManager weaponManager;
    public float moveRange = 2f;

    private void OnEnable()
    {
        grabState = GrabState.Throwable;
        Player = Movement.PlayerMovement;
        weaponManager = WeaponManager.weaponManager;
    }

    private void Update()
    {
        if (weaponManager.isAmmoFullOnCurrentWeapon()) // ammo is voll 
            return;
        // check distance tussen player en ammobox
        // als dichtbij genoeg dan checken we of current ammo van het wapen wat die vast heeft nie max is 
        // als dat nie max is move naar de player toe en zet grabstate op nie gooibaar
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if(distance <= moveRange)
        {
            
      
            transform.position = Vector2.Lerp(transform.position, Player.transform.position, 10 * Time.deltaTime);
        

         
        }
      

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (weaponManager.isAmmoFullOnCurrentWeapon()) // ammo is voll 
            return;

        if (collision.transform.CompareTag("Player"))
        {
            AddAmmo();
        }
    }

    private void AddAmmo()
    {
        weaponManager.AmmoHandling(weaponManager.EquipedWeapon.AmmoUsage, 100, false);
        gameObject.SetActive(false);
    }


}
