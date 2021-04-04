using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weaponsystem/WeaponDataSet")]
public class WeaponData : ScriptableObject
{
    // vink dit alleen aan als een wapen hier support voor heeft laat uit als nie gebruik van gemaakt word
    [Header("WEAPON DATA")]
    public string WeaponName;
    [Space(5)]
    [Tooltip("The weapon can roll with a scope")]
    public bool useScope = false;
    [Space(5)]
    [Tooltip("The weapon can roll with a barrel")]
    public bool useBarrel = false;
    [Space(5)]
    [Tooltip("The weapon can roll with a Stock")]
    public bool useStock = false;
    [Space(5)]
    [Tooltip("The weapon can roll with a magazine")]
    public bool useMagazine = false;

    public Material WeaponMaterial;

    public GameObject WeaponEffect;

    // weapon parts
    [Header("WEAPON PARTS")]
    public GameObject[] ScopeParts;
    public GameObject[] BodyParts;
    public GameObject[] BarrelParts;
    public GameObject[] StockParts;
    public GameObject[] MagazineParts;
   

}
