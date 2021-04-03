using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : MonoBehaviour
{


    public List<WeaponStatPair> RawStats;
    public Dictionary<WeaponStatType, float> WeaponPartStats = new Dictionary<WeaponStatType, float>();
    public Rarity rarity;


    public enum WeaponStatType
    {
        Damage,
        Accuracy, // dit is weapon handling
        AmmoPerShot,
        FireRate
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
