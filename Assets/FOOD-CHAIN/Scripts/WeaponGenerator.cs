using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    public WeaponData[] WeaponDatabase;

    private WeaponData WeaponToSpawn;


    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GenerateWeapons();

        }
    }

    private void GenerateWeapons()
    {
        // spawn random weapons met random parts
        WeaponToSpawn = GetWeaponToSpawn();

        GameObject WeaponBody = GetWeaponPart(WeaponToSpawn.BodyParts);
        GameObject Weapon = Instantiate(WeaponBody,Movement.PlayerMovement.transform.position,transform.rotation);

        Gun WeaponScript = Weapon.GetComponent<Gun>();
        // check voor scope
        if (WeaponToSpawn.useScope)
        {
            GameObject WeaponScopePart = GetWeaponPart(WeaponToSpawn.ScopeParts);
          GameObject scope=  Instantiate(WeaponScopePart, WeaponScript.ScopeSlot);
            scope.transform.localPosition = new Vector3(0, 0, 0);
        }

        // check voor barrel
        if (WeaponToSpawn.useBarrel)
        {
            GameObject WeaponBarrelPart = GetWeaponPart(WeaponToSpawn.BarrelParts);
            GameObject barrel =   Instantiate(WeaponBarrelPart, WeaponScript.BarrelSlot);
            barrel.transform.localPosition = new Vector3(0, 0, 0);
        }

        // check voor stock
        if (WeaponToSpawn.useStock)
        {
            GameObject WeaponStockPart = GetWeaponPart(WeaponToSpawn.StockParts);
           GameObject stock= Instantiate(WeaponStockPart, WeaponScript.StockSlot);
            stock.transform.localPosition = new Vector3(0, 0, 0);
        }

        // check voor magazine
        if (WeaponToSpawn.useMagazine)
        {
            GameObject WeaponMagazinePart = GetWeaponPart(WeaponToSpawn.MagazineParts);
          GameObject magazine = Instantiate(WeaponMagazinePart, WeaponScript.MagazinSlot);
            magazine.transform.localPosition = new Vector3(0, 0, 0);
        }

        WeaponScript.ThrowWeapon();
    }


    private WeaponData GetWeaponToSpawn()
    {
        int WeaponIndex = Random.Range(0, WeaponDatabase.Length);
        WeaponData Weapon = WeaponDatabase[WeaponIndex];
        return Weapon;
    }

    private GameObject GetWeaponPart(GameObject[] WeaponParts)
    {
        int randPart = Random.Range(0, WeaponParts.Length);
        GameObject PartToUse = WeaponParts[randPart];

        return PartToUse;


    }
}
