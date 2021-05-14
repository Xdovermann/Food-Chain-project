using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Wall"))
		{
			DestroyProjectile();
		}
		else if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
            if (player.collManager.onGround || player.collManager.onWall && !player.collManager.WallGrab)
            {
				player.TakeDamage(1);
				DestroyProjectile();
			}
		
		}
	}

    private void DestroyProjectile()
    {
	    GameObject go=	ObjectPooler.FlashEffect.GetObject();
		go.transform.position = transform.position;
		go.SetActive(true);


		gameObject.SetActive(false);
    }

}