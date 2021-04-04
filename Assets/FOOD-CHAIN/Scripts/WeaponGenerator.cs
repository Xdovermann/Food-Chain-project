using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    public WeaponData[] WeaponDatabase;

    private WeaponData WeaponToSpawn;

    public Texture2D WeaponTexture;
    int textureWidthCounter = 0;
    int width, height;


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
            WeaponScript.AddWeaponPart(scope.GetComponent<WeaponPart>(),false);

         
     
        }

        // check voor barrel
        if (WeaponToSpawn.useBarrel)
        {
            GameObject WeaponBarrelPart = GetWeaponPart(WeaponToSpawn.BarrelParts);
            GameObject barrel =   Instantiate(WeaponBarrelPart, WeaponScript.BarrelSlot);
            barrel.transform.localPosition = new Vector3(0, 0, 0);
            WeaponScript.AddWeaponPart(barrel.GetComponent<WeaponPart>(),true);

      
        }

        // check voor stock
        if (WeaponToSpawn.useStock)
        {
            GameObject WeaponStockPart = GetWeaponPart(WeaponToSpawn.StockParts);
           GameObject stock= Instantiate(WeaponStockPart, WeaponScript.StockSlot);
            stock.transform.localPosition = new Vector3(0, 0, 0);
            WeaponScript.AddWeaponPart(stock.GetComponent<WeaponPart>(),false);

        
        }

        // check voor magazine
        if (WeaponToSpawn.useMagazine)
        {
            GameObject WeaponMagazinePart = GetWeaponPart(WeaponToSpawn.MagazineParts);
          GameObject magazine = Instantiate(WeaponMagazinePart, WeaponScript.MagazinSlot);
            magazine.transform.localPosition = new Vector3(0, 0, 0);

            WeaponScript.AddWeaponPart(magazine.GetComponent<WeaponPart>(),false);

     

        }

        //    Sprite NewWeaponSprite = CreateTexture(Sprites);
        //   WeaponScript.GetComponent<SpriteRenderer>().sprite = NewWeaponSprite;
  
     
     
        WeaponScript.CalculateStats();

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
//    private readonly List<Vector3> _vertices = new List<Vector3>();
  //  private readonly List<Vector2> _UV = new List<Vector2>();
    //private Mesh CreateTexture(Sprite sprite,Sprite sprite2)
    //{

    //    var vertices = sprite.vertices; // first copy of vertex buffer occurs here, when it is marshalled from unmanaged to managed memory
    //    var vertices2 = sprite2.vertices;

    //    _vertices.Clear();  // this temporary buffer allows me to avoid one of those allocation, but i still do copy of data because Mesh.SetVertices can't accept Vector2[]

    //    for (var i = 0; i < vertices.Length; i++)
    //    {
    //        _vertices.Add(vertices[i]);
    //        _vertices.Add(vertices2[i]);
    //    }

    //    mesh.SetVertices(_vertices);  // here's the third copy of vertex buffer is created and marshalled back to unmanaged memory
    //    mesh.SetTriangles(sprite.triangles, 0);
    //    mesh.SetTriangles(sprite2.triangles, 0);

    

    //    mesh.SetUVs(0, sprite.uv);
    //    mesh.SetUVs(0, sprite2.uv);
    //    mesh.RecalculateBounds();

    //    return mesh;

    //}

}
