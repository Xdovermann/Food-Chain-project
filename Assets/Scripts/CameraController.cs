using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;

    private	Vector3 target;
	private Vector3 mousePos;
	private Vector3 refVel;
	private Vector3 shakeOffset;
	private Vector3 shakeVector;

	public float cameraDist = 3.5f;
	public float smoothTime = 0.2f;
	public float Zoffset;

	private float shakeMag;
	private float shakeTimeEnd;
	private bool shaking;

	public static CameraController cameraController;

	public Transform pointToLook;

	void Awake () {

		Application.targetFrameRate = 60;
		cameraController = this;

		target = player.position;
	
	}

	void Update () {

        mousePos = CaptureMousePos();
	
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
       

    }

    private void FixedUpdate()
    {
		UpdateCameraPosition();
	}

    Vector3 CaptureMousePos(){

		Vector3 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
		ret *= 2;
		ret -= Vector3.one; //set (0,0) of mouse to middle of screen

		ret.z = ret.y;
		ret.y = 0;


		float max = 0.9f;
		if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.z) > max)
		{
			ret = ret.normalized; //helps smooth near edges of screen
		}


		return ret;
	}

	Vector3 UpdateTargetPos(){

		Vector3 mouseOffset = mousePos * cameraDist; 
		Vector3 ret = player.position + mouseOffset; 
		ret += shakeOffset; 
		ret.z += -Zoffset; 
		return ret;
	}

	Vector3 UpdateShake(){
		if (!shaking || Time.time > shakeTimeEnd){
			shaking = false; 
			return Vector3.zero; 
		}
		Vector3 tempOffset = shakeVector; 
		tempOffset *= shakeMag; 
		return tempOffset;
	}

	void UpdateCameraPosition(){
		Vector3 tempPos;
		tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime); 
		transform.position = tempPos;
	}

	public void Shake(Vector3 direction, float magnitude, float length){ 
		shaking = true;
		shakeVector = direction;
		shakeMag = magnitude; 
		shakeTimeEnd = Time.time + length; 
	}
}