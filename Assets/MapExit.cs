using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapExit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExitMap();
        }
    }

    private void ExitMap()
    {
        // call naar dungeonmaster pak volgende seed 
        // laad die mmap in 
        // move player

        // placeholder
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
