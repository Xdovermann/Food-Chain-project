using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;

    public void TakeDamage(int hit)
    {
        Health -= hit;
        CameraController.cameraController.Shake(Random.onUnitSphere, 2.5f, 0.1f);
        if (Health <= 0)
        {
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        Destroy(gameObject);
    }
}
