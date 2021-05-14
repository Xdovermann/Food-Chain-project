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
    [Space(5)]
    [Tooltip("The weapon can roll with a Grip")]
    public bool useGrips = false;

    [Space(5)]
    [Header("WEAPON PARTS RANDOM CHANCE TO ROLL")]
    public bool RandomChanceForStock = true;
    public bool RandomChanceForGrip = true;
    public bool RandomChanceForScope = true;
    [Space(5)]

    // weapon parts
    [Header("WEAPON PARTS")]
    public GameObject[] ScopeParts;
    public GameObject[] BodyParts;
    public GameObject[] BarrelParts;
    public GameObject[] StockParts;
    public GameObject[] MagazineParts;
    public GameObject[] GripParts;

    [Space(5)]
    [Header("WEAPON JUICE")]
    public Vector2 WeaponPos = new Vector2(0.875f, 0);
    public GameObject ShotEffect;
}
