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
    public Transform GripSlot;
    [Space(10)]

    [HideInInspector]
    public Rigidbody2D rb;
    private List<WeaponPart> weaponParts = new List<WeaponPart>();
  //  private Dictionary<WeaponStatType, float> weaponStats = new Dictionary<WeaponStatType, float>();
    [HideInInspector]
    public BulletSourceScript weaponEmitter;
    [HideInInspector]
    public bool isEquiped = false;
  

    public WeaponData weaponData;

    public List<PerkBehaviour> PerkRoles = new List<PerkBehaviour>();

    public LineRenderer RarityRenderer;

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
    public float WeaponAccuracy_Stat; // aim snelheid
    public float WeaponHandling_Stat; // weapon recoil
    [Space(10)]
    public AmmoType AmmoUsage;
     
    public void AddWeaponPart(WeaponPart part,bool SetShotPoint)
    {
        weaponParts.Add(part);

        SpriteRenderer weaponSprite = part.gameObject.GetComponent<SpriteRenderer>();
        part.PartRenderer = weaponSprite;

      

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

        PartRenderer = GetComponent<SpriteRenderer>();
        weaponParts.Add(this);

        TimeBtwnShotsHolder = FireRate;
    
        weaponEmitter = GetComponentInChildren<BulletSourceScript>();
        weaponEmitter.SetUpEmitterOnStart();

        RarityRenderer = gameObject.AddComponent<LineRenderer>();
        RarityRenderer.startWidth = 0.125f;
        RarityRenderer.endWidth = 0.125f;
        RarityRenderer.material = ToolTip.tooltip.RaritylineRenderer;
        RarityRenderer.sortingLayerName = "Weapon";


        weaponManager = WeaponManager.weaponManager;

        DisablePatternEmitter();
       
    }

    private void Update()
    {
        RarityRenderer.SetPosition(0,new Vector3(transform.position.x,transform.position.y,0));
        float POSY = transform.position.y;
        POSY += 2;
        RarityRenderer.SetPosition(1, new Vector3(transform.position.x, POSY, 0));

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
              
             //   weaponStats.Add(stat.Key, stat.Value);

                switch (stat.Key)
                {
                    case WeaponStatType.Damage:
                        WeaponDamage_Stat += stat.Value;
                        break;
                    case WeaponStatType.Accuracy:
                        WeaponAccuracy_Stat += stat.Value;
                        break;
                    case WeaponStatType.AmmoPerShot:
                        WeaponAmmoOnShot_Stat += (int)stat.Value;
                        break;
                    case WeaponStatType.FireRate:
                        WeaponAttackSpeed_Stat += stat.Value;
                        break;
                    case WeaponStatType.Handling:
                        WeaponHandling_Stat += stat.Value;
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
        SetRarityRenderColor();
    }

    private void SetRarityRenderColor()
    {

        Gradient gradient;
        float StartAlpha = 0.4f;
        float EndAlpha = 0f;
        switch (rarity)
        {
            //225
            //25

           // gradient values zijn raar en werken nie
            case Rarity.Uncommon:
                gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(GameManager.gameManager.Uncommon, 0.0f), new GradientColorKey(GameManager.gameManager.Uncommon, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, 0.0f), new GradientAlphaKey(EndAlpha, 1f) }
                );
                RarityRenderer.colorGradient = gradient;
                break;
            case Rarity.Common:
               

                gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(GameManager.gameManager.Common, 0.0f), new GradientColorKey(GameManager.gameManager.Common, 1.0f) },
                           new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, 0.0f), new GradientAlphaKey(EndAlpha, 1f) }
                );
                RarityRenderer.colorGradient = gradient;
                break;
            case Rarity.Rare:
          
               gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(GameManager.gameManager.Rare, 0.0f), new GradientColorKey(GameManager.gameManager.Rare, 1.0f) },
                           new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, 0.0f), new GradientAlphaKey(EndAlpha, 1f) }
                );
                RarityRenderer.colorGradient = gradient;

                break;
            case Rarity.Epic:
                gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(GameManager.gameManager.Epic, 0.0f), new GradientColorKey(GameManager.gameManager.Epic, 1.0f) },
                             new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, 0.0f), new GradientAlphaKey(EndAlpha, 1f) }
                );
                RarityRenderer.colorGradient = gradient;
                break;
            case Rarity.Legendary:
                gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(GameManager.gameManager.Legendary, 0.0f), new GradientColorKey(GameManager.gameManager.Legendary, 1.0f) },
                                new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, 0.0f), new GradientAlphaKey(EndAlpha, 1f) }
                );
                RarityRenderer.colorGradient = gradient;
                break;
            default:
                break;
        }
    }

    public void ThrowWeapon()
    {
        GetComponent<ThrowableObject>().ThrowObject(transform.position, Random.onUnitSphere * 1f,true);
        EnableRarityLine();
      //  rb.AddForce(Random.onUnitSphere * 10, ForceMode2D.Impulse);
    }

    private void ShootPattern()
    {
        Movement.PlayerMovement.ShootKnockBack(-weaponManager.mouseVector, 250);

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
        DisableRarityLine();
    }

    public void DequipWeapon()
    {
        isEquiped = false;
        ThrowWeapon();
        DisablePerks();
    }

    public void LowerSprites()
    {
        if (weaponParts[0].PartRenderer.sortingLayerName == "Weapon") // check of renderer al gezet is 
            return;
        // ga naar player layer 
        // en sorting order nummer is -1
        //foreach (SpriteRenderer renderer in WeaponPartSprites)
        //{
        //    renderer.sortingLayerName = "Player";
        //    renderer.sortingOrder = -1;

        //}

        for (int i = 0; i < weaponParts.Count; i++)
        {
            SpriteRenderer renderer = weaponParts[i].PartRenderer;
            renderer.sortingLayerName = "Weapon";

            if (weaponParts[i].weaponPartType == WeaponPartType.Body) // weapon part is body 
            {
                renderer.sortingOrder = -2; // boddy wil je boven de barrel hebben

            }
            else if (weaponParts[i].weaponPartType == WeaponPartType.Grip)
            {
                renderer.sortingOrder = -1; // grip wil je boven de body en barrel hebben

            }
            else if (weaponParts[i].weaponPartType == WeaponPartType.Scope)
            {
                renderer.sortingOrder = -1; // scope wil je boven de body en barrel hebben
            }
            else
            {
                renderer.sortingOrder = -3; // en de rest van de parts wil je onder de boddy hebben
            }

        }
    }

    public void HigerSprites()
    {
        if (weaponParts[0].PartRenderer.sortingLayerName == "Player") // check of renderer al gezet is 
            return;

        for (int i = 0; i < weaponParts.Count; i++)
        {
            SpriteRenderer renderer = weaponParts[i].PartRenderer;
            renderer.sortingLayerName = "Player";

            if(weaponParts[i].weaponPartType == WeaponPartType.Body) // weapon part is body 
            {
                renderer.sortingOrder = 1; // boddy wil je boven de barrel hebben

            }else if (weaponParts[i].weaponPartType == WeaponPartType.Grip)
            {
                renderer.sortingOrder = 2; // grip wil je boven de body en barrel hebben

            }else if(weaponParts[i].weaponPartType == WeaponPartType.Scope)
            {
                renderer.sortingOrder = 2; // scope wil je boven de body en barrel hebben
            }
            else
            {
                renderer.sortingOrder = 0; // en de rest van de parts wil je onder de boddy hebben
            }
            
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
    private void DisableRarityLine()
    {
        RarityRenderer.enabled = false;
    }
    private void EnableRarityLine()
    {
        RarityRenderer.enabled = true;
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
