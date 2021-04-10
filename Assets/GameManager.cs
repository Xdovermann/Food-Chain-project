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

    private void Awake()
    {
        gameManager = this;
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
        if(runNumber == 4)
        {
            Debug.LogError("spawn boss as its the final map stage");

        }
        runNumber++;
        levelGenerator.RemoveMap();

        ObjectPooler.AmmoBox.DisableAll();
        WeaponGenerator.weaponGenerator.DestroyWeapons();
        DestroyThrownItems();

        levelGenerator.SpawnMap();
    }
}
