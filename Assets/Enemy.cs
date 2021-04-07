using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;
    private bool hasDied = false;
    public void TakeDamage(int hit,bool useShake)
    {
        if (hasDied)
            return;

        Health -= hit;

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
        GameObject ammoBox = ObjectPooler.AmmoBox.GetObject();
        ammoBox.transform.position = transform.position;
        ammoBox.SetActive(true);

        Destroy(gameObject);
    }
}
