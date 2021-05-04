using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public LevelGenerator levelGenerator;
    private int runNumber;

    public Movement player;
    public Transform ItemParent;

    public List<Enemy> EnemyList = new List<Enemy>();

    public WaveData EnemyWaveData;

    public Color DamagePopUpColor;
    public Color AmmoBoxPickUp;
    

    private void Awake()
    {
        gameManager = this;

        EnemyWaveData.WaveCount = EnemyWaveData.SetWaveCount();
        EnemyWaveData.currentEnemyGroupCount = EnemyWaveData.SetEnemyGroupCount();

        levelGenerator.SpawnMap();
    }

    public void DestroyThrownItems()
    {
        foreach (Transform child in ItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void MapCompleted()
    {
   
        runNumber++;
        levelGenerator.RemoveMap();

        ObjectPooler.AmmoBox.DisableAll();
        WeaponGenerator.weaponGenerator.DestroyWeapons();
        DestroyThrownItems();

        levelGenerator.SpawnMap();

        SpawnUsableItems();
    }

   public IEnumerator WaveManager()
    {

        yield return new WaitUntil(() => EnemyList.Count == 0); // alle gespawnde enemies dood

        for (int i = 0; i < EnemyWaveData.WaveCount; i++)
        {
            EnemyWaveData.currentEnemyGroupCount = EnemyWaveData.SetEnemyGroupCount(); 
      

            for (int x = 0; x < EnemyWaveData.currentEnemyGroupCount; x++)
            {
                SpawnUsableItems();
                yield return new WaitForSeconds(0.5f);

                for (int u = 0; u < 4; u++)
                {
                  

                    int index = Random.Range(0, levelGenerator.SpawnPoints.Count);
                    Transform pos = levelGenerator.SpawnPoints[index];

                   // pos.x -= 25.5f;// de renderer hebben zetten we met een offset
                   // pos.y -= 25.5f;


                    GameObject go = Instantiate(levelGenerator.TestEnemie, pos.position, transform.rotation);
                    go.transform.SetParent(levelGenerator.GeneratedMap.transform);

                    if (runNumber == 4 && u ==4)
                    {
                        Debug.LogError("spawn boss as its the final map stage");

                    }

                    levelGenerator.destructibleTerrain.DestroyTerrainRadius(pos.position, 3f);
                }
               

                // moet enemy zetten maar als je pos zet call je dat op de server want als dat vanuit client gaat werkt niet
                //int rand = Random.Range(0, EnemyspawnPoints.Length);
                //Transform spawnPoint = EnemyspawnPoints[rand];

                //PlaceEnemy(spawnPoint.position);

                // spawn een groep van enemies 



            }

            // spawn enemys 
            yield return new WaitUntil(() => EnemyList.Count == 0);
            yield return new WaitForSeconds(0.5f);


        }

       

        MapExit.mapExit.PlayerCanExit();
    }
    public void SpawnUsableItems()
    {
        int holder = Random.Range(0, 3);
        if(holder == 1)
        {
            int index = Random.Range(0, levelGenerator.SpawnPoints.Count);
            Transform pos = levelGenerator.SpawnPoints[index];

            int indexItem = Random.Range(0, levelGenerator.UsableItems.Length);

            Instantiate(levelGenerator.UsableItems[indexItem], pos.position, transform.rotation);

            levelGenerator.destructibleTerrain.DestroyTerrainRadius(pos.position, 1f);
        }
        else
        {
            return;
        }

   
    }

    public void AddEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
    }
}



