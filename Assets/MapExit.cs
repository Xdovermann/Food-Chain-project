using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapExit : MonoBehaviour
{
    public static MapExit mapExit;
    private bool canExit = false;

    private void Awake()
    {
        mapExit = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canExit)
            return;

        if (collision.CompareTag("Player"))
        {
            ExitMap();
        }
    }

    public void PlayerCanExit()
    {
        canExit = true;
    }

    private void ExitMap()
    {
        // call naar dungeonmaster pak volgende seed 
        // laad die mmap in 
        // move player

        GameManager.gameManager.MapCompleted();

        // placeholder
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
