using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmmoBox : ThrowableObject
{
    // als player in grabrange is move dan naar de player toe
    // als zijn ammo vol is van zijn current weapon blijf still liggen
    public int AmmoAmount =25;
    private Movement Player;
    private WeaponManager weaponManager;
    public float moveRange = 2f;

    private void OnEnable()
    {
     
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
            CheckIfGrabbed();
            AddAmmo();

        }
    }

    private void AddAmmo()
    {
        weaponManager.AmmoHandling(weaponManager.EquipedWeapon.AmmoUsage, AmmoAmount, false);

        GameObject effect = ObjectPooler.FlashEffect.GetObject();
        effect.transform.position = transform.position;
        effect.SetActive(true);
        effect.transform.DOShakeScale(0.1f);

        gameObject.SetActive(false);
    }

    // checken we of we de ammobox vasthaden en dan toch opraapte
    private void CheckIfGrabbed()
    {
        if (GrappleManager.grappleManager.throwableObject == this)
        {
            ThrowObject(new Vector2(0, 0), new Vector2(0, 0));
            GrappleManager.grappleManager.throwableObject = null;
            GrappleManager.grappleManager.GrappledObject = null;
        }
    }

}
