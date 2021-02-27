using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float xInput = 0;
	private float yInput = 0;
    public float speed = 5;
	
	public SpriteRenderer gunRend;	
	CameraController Cam;
	public Animator anim;
	public static PlayerController player;

    private void Awake()
    {
		player = this;
	}

    void Start () {

	

		Cam = CameraController.cameraController;
		anim = GetComponent<Animator>();
		

	}
	void Update () {

		GetInput(); 
		Movement(); 
	//	Animation();
		
	}

	void GetInput(){

		xInput = Input.GetAxisRaw("Horizontal"); 
		yInput = Input.GetAxisRaw("Vertical"); 

		if(xInput != 0 || yInput != 0)
        {
			anim.SetBool("isRunning", true);


        }
        else
        {
			anim.SetBool("isRunning", false);
        }
       

		
	}



	void Movement(){

		Vector3 tempPos = transform.position;
		tempPos += new Vector3(xInput,yInput,0) * speed * Time.deltaTime; 
		transform.position = tempPos;
	}



	
}