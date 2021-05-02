using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int WaveCount;
    public int currentEnemyGroupCount;

    public int minAmountWaves = 1;
    public int maxAmountWaves = 3;

    public int minAmountOfEnemys = 1;
    public int maxAmountOfEnemys = 3;

    public int SetWaveCount()
    {

        int number = Random.Range(minAmountWaves, maxAmountWaves);    
        return number;
    }

    public int SetEnemyGroupCount()
    {

       int number = Random.Range(minAmountOfEnemys, maxAmountOfEnemys);
        return number;
    }
}
