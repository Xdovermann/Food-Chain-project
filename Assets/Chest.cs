using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ThrowableObject
{
    public GameObject[] Items;

    public bool isHolding = false;

    public void PickUpChest()
    {
        isHolding = true;
    }

    public void RemoveRotation()
    {
        // transform.localRotation.z = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0); 
        rb.velocity = new Vector2(0,0);
        rb.angularVelocity = 0;

        Debug.LogError("place down chest");
        isHolding = false;
    }
}
