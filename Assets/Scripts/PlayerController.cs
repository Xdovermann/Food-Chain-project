using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float xInput = 0;
	private float yInput = 0;
    public float speed = 5;
	public float tiltAmount;

	private Rigidbody rb;

	public Animator anim;
	public static PlayerController player;

	public Transform playerRenderer;

	public int side = 0;

	private void Awake()
    {
		player = this;
	}

   private void Start () 
	{

		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();

	}

	private	void Update ()
	{

		GetInput(); 
		
	
		
	}

    private void FixedUpdate()
    {
		Movement();
	}

    private void GetInput(){

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

		TiltCharacter();
	}

	private void Movement(){

	
		Vector3 tempPos = new Vector3(xInput,0, yInput) * speed * Time.fixedDeltaTime;
		rb.velocity = tempPos;

		
	}

	public void FlipCharacter(int side)
    {

		this.side = side;

		if (this.side == 0)
        {
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
			

		}
		else if (this.side == 1)
        {
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
			
		}
    }

	private void TiltCharacter()
    {
        if(xInput == 0)
        {
			transform.localEulerAngles = new Vector3(0, 0, 0);
			return;
        }

		if(xInput > 0)
        {
			// tilt naar rechts

			transform.localEulerAngles = new Vector3(0, 0, -tiltAmount);

		}
		else if(xInput < 0)
        {
			// tilt naar links
			transform.localEulerAngles = new Vector3(0, 0, tiltAmount);
		}
    }

	
}