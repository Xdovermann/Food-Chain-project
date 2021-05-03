using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : MonoBehaviour
{


    public List<WeaponStatPair> RawStats;
    public Dictionary<WeaponStatType, float> WeaponPartStats = new Dictionary<WeaponStatType, float>();
    public Rarity rarity;
   [HideInInspector]
    public SpriteRenderer PartRenderer;

    [Space(10)]
    [Header("Part type")]
    public WeaponPartType weaponPartType;
    public enum WeaponStatType
    {
        Damage,// damage
        Accuracy, // dit is weapon handling hoe snel die rond beweegt
        AmmoPerShot,// ammo consumptiie
        FireRate // hoe snel die schiet
    }

    public enum WeaponPartType
    {
        Body,
        Barrel,
        Scope,
        Stock,
        Magazine,
        Grip,
    }

    public enum Rarity
    {
        Uncommon,
        Common,
        Rare,
        Epic,
        Legendary
    }
   
    private void Awake()
    {
      
        foreach (WeaponStatPair statPair in RawStats)
        {
            float Value = Random.Range(statPair.minValue, statPair.maxValue);
        
        
            WeaponPartStats.Add(statPair.StatType, Value);
        }
    }

    [System.Serializable]
    public class WeaponStatPair
    {
        public WeaponStatType StatType;
     
        public float minValue;
        public float maxValue;
    }
}
