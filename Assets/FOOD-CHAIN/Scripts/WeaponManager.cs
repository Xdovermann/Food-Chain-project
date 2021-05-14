using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
   


    public float moveSpeed = 10;
    public Transform Weapon;
    public Transform weaponRendererParent;

    private Vector3 mousePos;
    [HideInInspector]
    public  Vector3 mouseVector;

    public Movement playerMovement;
    public float WeaponHandling = 0.1f;

    private float OriginalWeaponPos;
    private GameObject ShotEffect;
    [HideInInspector]
    public Gun EquipedWeapon;

    [Header("Max Ammo ")]
    public int MaxPistolAmmo = 250;
    public int MaxSmgAmmo = 500;
    public int MaxRifleAmmo = 300;
    public int MaxShotgunAmmo = 125;
    public int MaxSniperRifleAmmo = 50;
    public int MaxExplosiveAmmo = 50;

    private int PistolAmmo =50;
    private int SmgAmmo = 50;
    private int RifleAmmo = 50;
    private int ShotgunAmmo = 50;
    private int SniperRifleAmmo = 50;
    private int ExplosiveAmmo = 50;

    [Header("UI")]
    public TextMeshProUGUI AmmoText;



    private void Awake()
    {
        OriginalWeaponPos = weaponRendererParent.transform.localPosition.x;
        AmmoText.SetText(""); // reset ammo text
        weaponManager = this;
    }

    private void Update()
    {
        GetMouseInput();
        Animation();

       
    }

 
    private void GetMouseInput()
    {
     
        //if(TimeBtwnShots <= 0)
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        GameObject go = Instantiate(Bullet, Shotpoint.position, Shotpoint.rotation);
        //        go.GetComponent<Bullet>().Setup(mouseVector);

        //        GameObject Effect = ObjectPooler.GunShotEffect.GetObject();
        //        Effect.transform.position = Shotpoint.position;
        //        Effect.SetActive(true);
        //        Effect.transform.DOShakeScale(0.1f,0.5f);

        //        TimeBtwnShots = TimeBtwnShotsHolder;
        //        


        //    }
        //}
        //else
        //{
        //    TimeBtwnShots -= Time.deltaTime;
        //}

       




    }



    private void Animation()
    {
         transform.DOMove(playerMovement.transform.position, WeaponHandling);
       // transform.position = playerMovement.transform.position;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //position of cursor in world
        mousePos.z = transform.position.z; //keep the z position consistant, since we're in 2d
        mouseVector = (mousePos - transform.position).normalized; //normalized vector from player pointing to cursor
        float gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg; //find angle in degrees from player to cursor
       

        Weapon.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back); //rotate gun sprite around that angle

        if(EquipedWeapon != null)
        {
          
            if (gunAngle >= 0 && gunAngle <= 180)
            {
                // weaponRendererParent.sortingOrder = -1;
                EquipedWeapon.HigerSprites();

            }
            else
            {
                //  weaponRendererParent.sortingOrder = 0;
                EquipedWeapon.LowerSprites();
              
            }
        }
      

        PlayerDirection();


    }

  

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void PlayerDirection()
    {
     //   if (playerMovement.wallGrab || !playerMovement.canMove)
      //      return;



        if (mouseVector.x > 0)
        {
            playerMovement.side = 1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRendererParent.localScale = new Vector3(1, 1, 1);
        }
        else if (mouseVector.x < 0)
        {
            playerMovement.side = -1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRendererParent.localScale = new Vector3(1, -1, 1);
        }
    }

    public void WeaponJuice()
    {
        // camerashake
        float power = Random.Range(2.5f, 3f);
        float randTime = Random.Range(0.05f, 0.1f);
        CameraController.cameraController.Shake(-mouseVector,power, randTime);

        //weaponscale
        weaponRendererParent.transform.localScale = new Vector3(1, 1, 1);


        // weaponknockback
        Sequence ShootMovementSequence = DOTween.Sequence();
        float MovePoint = OriginalWeaponPos;
        MovePoint -= Random.Range(0.25f, 0.5f);

        ShootMovementSequence.Append(weaponRendererParent.transform.DOShakeScale(0.1f));
        ShootMovementSequence.Append(weaponRendererParent.transform.DOLocalMoveX(MovePoint, 0.1f));
        ShootMovementSequence.Append(weaponRendererParent.transform.DOLocalMoveX(OriginalWeaponPos, 0.1f));

        if(ShotEffect != null)
        {
            GameObject Effect = Instantiate(ShotEffect);
            Effect.transform.position = EquipedWeapon.weaponEmitter.transform.position;
            Effect.SetActive(true);
            Effect.transform.DOShakeScale(0.1f, 0.5f);
      
        }
        else
        {
            Debug.LogWarning("het wapen heeft geen shoteffect");
        }
     
        


    }

    public void EquipNewWeapon(Transform weaponToEquip)
    {
        if(EquipedWeapon != null)
        {
  //          EquipedWeapon.GetComponent<ThrowableObject>().ThrowObject(transform.position, Random.onUnitSphere * 1f);    
            EquipedWeapon.GetComponent<Gun>().DequipWeapon();
            EquipedWeapon = null;
        }

        EquipedWeapon = weaponToEquip.GetComponent<Gun>();

        weaponToEquip.transform.SetParent(weaponRendererParent);
        weaponToEquip.transform.localRotation = new Quaternion(0, 0, 0, 0);
        weaponToEquip.GetComponent<ThrowableObject>().DisablePhysics();
        weaponToEquip.transform.localPosition = new Vector3(0, EquipedWeapon.weaponData.WeaponPos.y, 0);
        weaponRendererParent.transform.localPosition = new Vector2(EquipedWeapon.weaponData.WeaponPos.x, 0);
        OriginalWeaponPos = EquipedWeapon.weaponData.WeaponPos.x;
        weaponToEquip.transform.localScale = new Vector3(1, 1, 1);

      

        ShotEffect = EquipedWeapon.weaponData.ShotEffect;

        EquipedWeapon.EquipWeapon();
    }
    
    public bool EnoughAmmo(AmmoType ammoType,int AmountToUse)
    {
        switch (ammoType)
        {
            case AmmoType.Pistol:
                return WeaponShotPossible(PistolAmmo, AmountToUse);
            
            case AmmoType.SMG:
                return WeaponShotPossible(SmgAmmo, AmountToUse);
            
            case AmmoType.Rifle:
                return WeaponShotPossible(RifleAmmo, AmountToUse);
             
            case AmmoType.Shotgun:
                return WeaponShotPossible(ShotgunAmmo, AmountToUse);
            
            case AmmoType.SniperRifle:
                return WeaponShotPossible(SniperRifleAmmo, AmountToUse);
           
            case AmmoType.Explosives:
                return WeaponShotPossible(ExplosiveAmmo, AmountToUse);
            
            default:
                break;

               
        }

        Debug.LogWarning("geen fatsoenlijke ammo type mee gegeven");
        return true;
    }

    private bool WeaponShotPossible(int AmmoStack,int AmmoForAShot)
    {
        if(AmmoStack > 0)
        {
            int amount = AmmoStack - AmmoForAShot;
            if (amount >= 0) // we kunnen schieten
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void AmmoHandling(AmmoType ammoType, int AmountToUse,bool removeAmmo)
    {

        switch (ammoType)
        {
            case AmmoType.Pistol:
                if (removeAmmo)
                {
                    PistolAmmo -= AmountToUse;
                    PistolAmmo = Mathf.Clamp(PistolAmmo, 0, MaxPistolAmmo);
                }
                else
                {
                    PistolAmmo += AmountToUse;
                    PistolAmmo = Mathf.Clamp(PistolAmmo, 0, MaxPistolAmmo);
                }
              
                UpdateAmmoUI(PistolAmmo);
                break;

            case AmmoType.SMG:
                if (removeAmmo)
                {
                    SmgAmmo -= AmountToUse;
                    SmgAmmo = Mathf.Clamp(SmgAmmo, 0, MaxSmgAmmo);

                }
                else
                {
                    SmgAmmo += AmountToUse;
                    SmgAmmo = Mathf.Clamp(SmgAmmo, 0, MaxSmgAmmo);

                }

                UpdateAmmoUI(SmgAmmo);
                break;

            case AmmoType.Rifle:
                if (removeAmmo)
                {
                    RifleAmmo -= AmountToUse;
                    RifleAmmo = Mathf.Clamp(RifleAmmo, 0, MaxRifleAmmo);
                }
                else
                {
                    RifleAmmo += AmountToUse;
                    RifleAmmo = Mathf.Clamp(RifleAmmo, 0, MaxRifleAmmo);
                }
             
                UpdateAmmoUI(RifleAmmo);
                break;

            case AmmoType.Shotgun:
                if (removeAmmo)
                {
                    ShotgunAmmo -= AmountToUse;
                    ShotgunAmmo = Mathf.Clamp(ShotgunAmmo, 0, MaxShotgunAmmo);
                }
                else
                {
                    ShotgunAmmo += AmountToUse;
                    ShotgunAmmo = Mathf.Clamp(ShotgunAmmo, 0, MaxShotgunAmmo);
                }
              
                UpdateAmmoUI(ShotgunAmmo);
                break;

            case AmmoType.SniperRifle:
                if (removeAmmo)
                {
                    SniperRifleAmmo -= AmountToUse;
                    SniperRifleAmmo = Mathf.Clamp(SniperRifleAmmo, 0, MaxSniperRifleAmmo);
                }
                else
                {
                    SniperRifleAmmo += AmountToUse;
                    SniperRifleAmmo = Mathf.Clamp(SniperRifleAmmo, 0, MaxSniperRifleAmmo);
                }
             
                UpdateAmmoUI(SniperRifleAmmo);
                break;

            case AmmoType.Explosives:
                if (removeAmmo)
                {
                    ExplosiveAmmo -= AmountToUse;
                    ExplosiveAmmo = Mathf.Clamp(ExplosiveAmmo, 0, MaxExplosiveAmmo);
                }
                else
                {
                    ExplosiveAmmo += AmountToUse;
                    ExplosiveAmmo = Mathf.Clamp(ExplosiveAmmo, 0, MaxExplosiveAmmo);
                }
          
                UpdateAmmoUI(ExplosiveAmmo);
                break;

            default:
                break;


        }

    }

    public bool isAmmoFullOnCurrentWeapon()
    {
    
        if(EquipedWeapon == null)
        {
            return true;
        }

        switch (EquipedWeapon.AmmoUsage)
        {
            case AmmoType.Pistol:
              
                return CheckAmmoAmount(PistolAmmo, MaxPistolAmmo);
            case AmmoType.SMG:

                return CheckAmmoAmount(SmgAmmo, MaxSmgAmmo);
            case AmmoType.Rifle:

                return CheckAmmoAmount(RifleAmmo, MaxRifleAmmo);
            case AmmoType.Shotgun:

                return CheckAmmoAmount(ShotgunAmmo, MaxShotgunAmmo);
            case AmmoType.SniperRifle:

                return CheckAmmoAmount(SniperRifleAmmo, MaxSniperRifleAmmo);
            case AmmoType.Explosives:

                return CheckAmmoAmount(ExplosiveAmmo, MaxExplosiveAmmo);
            default:
                break;
        }

        return true;

    }

    private bool CheckAmmoAmount(int currentAmount, int maxAmount)
    {
        if (currentAmount == maxAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateAmmoUI(int currentAmmo)
    {
        AmmoText.SetText(currentAmmo.ToString());
    }

   

}
