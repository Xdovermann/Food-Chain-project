using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;
    private bool hasDied = false;

    private Base_Enemy_AI base_AI;

    private void Awake()
    {
        base_AI = GetComponent<Base_Enemy_AI>();
        GameManager.gameManager.AddEnemy(this);
    }

    public void TakeDamage(int hit,bool useShake)
    {
        if (hasDied)
            return;

        Health -= hit;
        GameObject TextPopUp = ObjectPooler.DamageNumber.GetObject();
        TextPopUp.GetComponent<TextPopUp>().SpawnText(transform.position, "56",GameManager.gameManager.DamagePopUpColor);
        if (useShake)
        {
            CameraController.cameraController.Shake(Random.onUnitSphere, 2.5f, 0.1f);
        }
      
        if (Health <= 0)
        {
          
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        hasDied = true;
        GameManager.gameManager.RemoveEnemy(this);

        int rand = Random.Range(0, 4);
        if(rand == 1)
        {
            GameObject ammoBox = ObjectPooler.AmmoBox.GetObject();
            ammoBox.transform.position = transform.position;
            ammoBox.SetActive(true);

        }
    

        Destroy(gameObject);
    }
}
