using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodChain.BulletML;

public class Player_Projectile : MonoBehaviour
{
    //private BulletScript BulletMLbulletScript;

    //private void Awake()
    //{
    //    BulletMLbulletScript = GetComponent<BulletScript>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }else if (collision.gameObject.CompareTag("Enemy"))
        {

        }
    }


}
