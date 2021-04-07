using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Wall"))
		{
			DestroyProjectile();
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<Enemy>().TakeDamage(1,true);
			DestroyProjectile();
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