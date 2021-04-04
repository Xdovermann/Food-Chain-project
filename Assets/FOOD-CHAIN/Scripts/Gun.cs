using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodChain.BulletML;
using DG.Tweening;

public class Gun : WeaponPart
{

    public Transform ScopeSlot;
    public Transform BarrelSlot;
    public Transform StockSlot;
    public Transform MagazinSlot;

    [HideInInspector]
    public Rigidbody2D rb;

    private List<WeaponPart> weaponParts = new List<WeaponPart>();
    Dictionary<WeaponStatType, float> weaponStats = new Dictionary<WeaponStatType, float>();

    private BulletSourceScript weaponEmitter;

    int RarityCounter = 0;
    public bool isEquiped = false;

    public float TimeBtwnShots = 0.1f;
    private float TimeBtwnShotsHolder;

    [HideInInspector]
    public List<SpriteRenderer> WeaponPartSprites = new List<SpriteRenderer>();

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
                POS.x += 0.25f;
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

        TimeBtwnShotsHolder = TimeBtwnShots;
    
        weaponEmitter = GetComponentInChildren<BulletSourceScript>();
        weaponEmitter.SetUpEmitterOnStart();
      
        DisablePatternEmitter();
       
    }

    private void Update()
    {
        if (!isEquiped)
            return;


       
            if (TimeBtwnShots <= 0)
            {
                if (Input.GetMouseButton(0))
                {
                    
                    ShootPattern();



                }
            }
            else if(weaponEmitter.gameObject.activeInHierarchy == false && TimeBtwnShots >= 0)
            {
                TimeBtwnShots -= Time.deltaTime;
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
                
                Debug.Log(stat.Key);
                Debug.Log(stat.Value);
            }
        }

        SetRarity();
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
     
        rb.AddForce(Random.onUnitSphere * 10, ForceMode2D.Impulse);
    }

    private void ShootPattern()
    {
        weaponEmitter.Reset();
        weaponEmitter.gameObject.SetActive(true);
        WeaponManager.weaponManager.WeaponJuice();
    }

    public void PatternDone()
    {
    
        DisablePatternEmitter();

    }

    private void DisablePatternEmitter()
    {
        weaponEmitter.gameObject.SetActive(false);
        TimeBtwnShots = TimeBtwnShotsHolder;
    }

    public void EquipWeapon()
    {
        TimeBtwnShots = 0.1f;
        isEquiped = true;
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
}
