using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    public CollisionManager collManager;
    public int MaxHealth;
    public int CurrentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        player = this;
        CurrentHealth = MaxHealth;
        collManager = GetComponent<CollisionManager>();
    }

    public void TakeDamage(int damage)
    {
        // kijk of player shield heeft
        // doe de damage van de shield af halen 
        // zodra shield breekt krijg player i frame en de rest van de damage word nie gemined op de health

        CurrentHealth -= damage;
    }
}
