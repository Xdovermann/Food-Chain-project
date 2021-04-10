using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    public Transform SpawnPoint;

    // Start is called before the first frame update
    private void Awake()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        GameManager.gameManager.player.transform.position= SpawnPoint.position;
    }
}
