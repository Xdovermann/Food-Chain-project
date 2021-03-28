using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Vector3 dir;
	float speed, startSpeed = 20;
	public SpriteRenderer bulletRend;
	public void Setup (Vector3 _dir) { 
		dir = _dir; 
		speed = startSpeed;
	}
	void FixedUpdate () {
		Move(); 		
	}
	void Move(){
		
		Vector3 tempPos = transform.position; 
		tempPos += dir * speed * Time.fixedDeltaTime; 
		transform.position = tempPos; 
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Wall"))
		{
			DestroyProjectile();
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
			DestroyProjectile();
		}
	}

    private void DestroyProjectile()
    {
	    GameObject go=	ObjectPooler.FlashEffect.GetObject();
		go.transform.position = transform.position;
		go.SetActive(true);

		Destroy(gameObject);
    }

}