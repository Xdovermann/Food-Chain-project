using FoodChain.BulletML;
using System.Collections.Generic;
using UnityEngine;


public enum AmmoType
{
    Pistol,
    SMG,
    Rifle,
    Shotgun,
    SniperRifle,
    Explosives,
}

public class Gun : WeaponPart
{

    [Header("Weapon Parts")]
    public Transform ScopeSlot;
    public Transform BarrelSlot;
    public Transform StockSlot;
    public Transform MagazinSlot;

    [Space(10)]

    [HideInInspector]
    public Rigidbody2D rb;
    private List<WeaponPart> weaponParts = new List<WeaponPart>();
    private Dictionary<WeaponStatType, float> weaponStats = new Dictionary<WeaponStatType, float>();
    [HideInInspector]
    public BulletSourceScript weaponEmitter;
    [HideInInspector]
    public bool isEquiped = false;
    [HideInInspector]
    public List<SpriteRenderer> WeaponPartSprites = new List<SpriteRenderer>();

    public WeaponData weaponData;

    public List<PerkBehaviour> PerkRoles = new List<PerkBehaviour>();

    private WeaponManager weaponManager;

    [Header("Gun Body Rollable Stats")]
    public float FireRate = 0.1f;
    private float TimeBtwnShotsHolder;
    private int RarityCounter = 0;
    [Space(10)]


 
  
    [Header("Weapon Stats")]
    public float WeaponAttackSpeed_Stat = 0.1f;
    public int WeaponAmmoOnShot_Stat;
    public float WeaponDamage_Stat;
    public float WeaponHandling_Stat;
    [Space(10)]
    public AmmoType AmmoUsage;
     
    public void AddWeaponPart(WeaponPart part,bool SetShotPoint)
    {
        weaponParts.Add(part);

        SpriteRenderer weaponSprite = part.gameObject.GetComponent<SpriteRenderer>();
        if(weaponSprite != null)
        {
            WeaponPartSprites.Add(weaponSprite);
        }

        if (SetShotPoint) // betekent dat we een barrel zijn
        {
            if(part.transform.GetChild(0) != null) // kijk of er een child bestaat 
            {
                // we moeten de shootdirection naar voren bewegen als we de emitter ook naar voor bewegen
                // pak posite van waar de emitter heen gaat en slap hier gwn wat extra ruimte op
                Vector3 POS = part.transform.GetChild(0).position;
                POS.x += 50f;
                POS.x = Mathf.RoundToInt(POS.x);
                weaponEmitter.ShootDirection.position = POS;
                
                weaponEmitter.transform.position = part.transform.GetChild(0).position; // ze de bullet emitter op deze spot
            }
            else
            {
                Debug.LogWarning("Barrel heeft geen shotpoint positie");
            }
          
        }

      
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        WeaponPartSprites.Add(GetComponent<SpriteRenderer>()); // de body van het wapen

        TimeBtwnShotsHolder = FireRate;
    
        weaponEmitter = GetComponentInChildren<BulletSourceScript>();
        weaponEmitter.SetUpEmitterOnStart();

        weaponManager = WeaponManager.weaponManager;

        DisablePatternEmitter();
       
    }

    private void Update()
    {
        if (!isEquiped)
            return;

        if (weaponEmitter.IsEnded && weaponEmitter.gameObject.activeInHierarchy)
        {
            weaponEmitter.gameObject.SetActive(false);
        }
            // maak dit opnieuw fire een callback wanneer de pattern emitter klaar is 
            // als die callback af is gegaan zetten we de timebtwnshots terug 
            // en als timebtwnshots laag genoge is mmogen we firen

       if(weaponEmitter.gameObject.activeInHierarchy == false && weaponManager.EnoughAmmo(AmmoUsage, WeaponAmmoOnShot_Stat))
        {
            if (FireRate <= 0)
            {
                if (Input.GetMouseButton(0))
                {

                    ShootPattern();
                    weaponManager.AmmoHandling(AmmoUsage, WeaponAmmoOnShot_Stat,true);


                }
            }
            else 
            {
                FireRate -= Time.deltaTime;
            }
        }
          
        
       

    }

    public void CalculateStats()
    {
        foreach (WeaponPart part in weaponParts)
        {
            RarityCounter += (int)part.rarity;
      

            foreach (KeyValuePair<WeaponStatType,float> stat in part.WeaponPartStats)
            {
                weaponStats.Add(stat.Key, stat.Value);

                switch (stat.Key)
                {
                    case WeaponStatType.Damage:
                        WeaponDamage_Stat += stat.Value;
                        break;
                    case WeaponStatType.Accuracy:
                        WeaponHandling_Stat += stat.Value;
                        break;
                    case WeaponStatType.AmmoPerShot:
                        WeaponAmmoOnShot_Stat += (int)stat.Value;
                        break;
                    case WeaponStatType.FireRate:
                        WeaponAttackSpeed_Stat += stat.Value;
                        break;
                    default:
                        break;
                }

                Debug.Log(stat.Key);
                Debug.Log(stat.Value);
            }
        }

        ClampValues();

        SetRarity();
    
        RollPerks();
    }

    private void ClampValues()
    {
        WeaponAmmoOnShot_Stat = Mathf.Clamp(WeaponAmmoOnShot_Stat, 1, 50);
    }

    private void SetRarity()
    {
        int averageRarity = RarityCounter / weaponParts.Count;
        averageRarity = Mathf.Clamp(averageRarity, 0, 4);

        rarity = (Rarity)averageRarity;

        Debug.Log("weapon rarity" + rarity);
    }

    public void ThrowWeapon()
    {
        GetComponent<ThrowableObject>().ThrowObject(transform.position, Random.onUnitSphere * 1f,true);
    
      //  rb.AddForce(Random.onUnitSphere * 10, ForceMode2D.Impulse);
    }

    private void ShootPattern()
    {
        weaponEmitter.gameObject.SetActive(true);
        weaponEmitter.Reset();
 
        WeaponManager.weaponManager.WeaponJuice();
    }

    public void PatternDone()
    {
    
        DisablePatternEmitter();

    }

    private void DisablePatternEmitter()
    {
        weaponEmitter.gameObject.SetActive(false);
        FireRate = TimeBtwnShotsHolder;
    }

    public void EquipWeapon()
    {
        FireRate = 0.1f;
        isEquiped = true;
        ActivatePerks();
        weaponManager.AmmoHandling(AmmoUsage,0,false); // callen dit hier om UI te zetten als je wapen opraapt
    }

    public void DequipWeapon()
    {
        isEquiped = false;
        ThrowWeapon();
        DisablePerks();
    }

    public void LowerSprites()
    {
        if (WeaponPartSprites[0].sortingLayerName == "Player") // check of renderer al gezet is 
            return;
        // ga naar player layer 
        // en sorting order nummer is -1
        foreach (SpriteRenderer renderer in WeaponPartSprites)
        {
            renderer.sortingLayerName = "Player";
            renderer.sortingOrder = -1;

        }
    }

    public void HigerSprites()
    {
        if (WeaponPartSprites[0].sortingLayerName == "Weapon") // check of renderer al gezet is 
            return;
        // ga naar weapon layer
        // en sorting order is 0
        foreach (SpriteRenderer renderer in WeaponPartSprites)
        {
            renderer.sortingLayerName = "Weapon";
            renderer.sortingOrder = 0;
        }
    }

    public void RollPerks()
    {
        // kijk naar rarity 
        // en gebaseerd op rarity rol je een aantal perks

        //uncommon : 0
        //common : 1
        // rare : 2
        // epic : 3
        // legendary : 4 

        for (int i = 0; i < (int)rarity; i++)
        {
            GameObject perkToUse = WeaponGenerator.weaponGenerator.RollRandomPerk();
            GameObject go = Instantiate(perkToUse, transform);
            PerkRoles.Add(go.GetComponent<PerkBehaviour>());
        }
   


    }

    public void ActivatePerks()
    {
        // scroll door perks heen
        // activate de perks
        foreach (PerkBehaviour perk in PerkRoles)
        {
            perk.AddAbility();
        }
        
    }


    public void DisablePerks()
    {
        // scroll door perks
        // disable ze allemaal
        foreach (PerkBehaviour perk in PerkRoles)
        {
            perk.RemoveAbility();
        }
    }
}
