using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ThrowableObject
{
    public GameObject[] Items;

    public GameObject[] BrokenPieces;
    public SpriteRenderer ChestRenderer;


    public bool isHolding = false;

   

    public void PickUpChest()
    {
        if (grabState == GrabState.NonThrowable)
            return;

        isHolding = true;
    }

    private void Update()
    {
        if (!isHolding)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    public void RemoveRotation()
    {
        // transform.localRotation.z = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0); 
        rb.velocity = new Vector2(0,0);
        rb.angularVelocity = 0;

        isHolding = false;
    }

   

    private void OpenChest()
    {
        isHolding = false;
        // ChestRenderer.sprite = null;

        ThrowObject(Movement.PlayerMovement.transform.position,new Vector2(0,0),false);
         grabState = GrabState.NonThrowable;
        objectCollider.enabled = false;
       
        isThrown = true;
        ChestRenderer.enabled = false;
        foreach (var Piece in BrokenPieces)
        {
            Piece.gameObject.SetActive(true);
            Piece.GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere * 20f,ForceMode2D.Impulse);
        }

        CameraController.cameraController.Shake(Random.onUnitSphere, 2.5f, 0.1f);

        GameObject go = ObjectPooler.FlashEffect.GetObject();
        go.transform.position = transform.position;
        go.SetActive(true);

        GrappleManager.grappleManager.ResetSpot();
    

        for (int i = 0; i < 2; i++)
        {
            WeaponGenerator.weaponGenerator.GenerateWeapons();
        }
        // spat de chest uit elkaar 
    }
}
