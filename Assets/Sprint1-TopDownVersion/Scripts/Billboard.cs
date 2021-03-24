using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    //public Transform pointToLook;

    void LateUpdate()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        transform.eulerAngles = new Vector3(CameraController.cameraController.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);     
    }
}
