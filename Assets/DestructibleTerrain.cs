using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTerrain : MonoBehaviour
{
    private Tilemap destructibleTileMap;
    public LayerMask BlockDecorLayer;
    // Start is called before the first frame update
    void Awake()
    {
        destructibleTileMap = GetComponent<Tilemap>();
    }

    public void DestroyTerrainRadius(Vector3 RadiusLocation,float radius)
    {
        Collider2D[] holder = Physics2D.OverlapCircleAll((Vector2)RadiusLocation, radius/1.25f, BlockDecorLayer);
        foreach (Collider2D decor in holder)
        {
            Destroy(decor.gameObject);

        }

        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                Vector3Int tilePos = destructibleTileMap.WorldToCell(RadiusLocation + new Vector3(x, y, 0));
                if(destructibleTileMap.GetTile(tilePos) != null)
                {
                    DestroyTile(tilePos);
                }
            }
        }
    }

    void DestroyTile(Vector3Int pos)
    {
        destructibleTileMap.SetTile(pos, null);

        //spawn broken tile

        LevelGenerator.levelGenerator.BackgroundLayer.SetTile(pos, LevelGenerator.levelGenerator.BackgroundAutoTile);

        // spawn destroyed tile block
      
        // check of deze tile geen decor stuk heeft
        // zo wel destroy
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector3 Position = Vector3.zero;
    //        foreach (ContactPoint2D hit in collision.contacts)
    //        {
    //            Position.x = hit.point.x - 0.01f *hit.normal.x;
    //            Position.y = hit.point.y - 0.01f * hit.normal.y;
    //            destructibleTileMap.SetTile(destructibleTileMap.WorldToCell(Position), null);
    //        }
    //    }
    //}
}
