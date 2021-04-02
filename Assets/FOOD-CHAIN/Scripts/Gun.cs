using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int RarityCounter = 0;

    public void AddWeaponPart(WeaponPart part)
    {
        weaponParts.Add(part);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}
