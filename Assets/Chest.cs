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
        isHolding = true;
    }

    private void Update()
    {
        if (!isHolding)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenChest(Movement.PlayerMovement.transform.position);
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

   

    private void OpenChest(Vector2 pos)
    {
        isHolding = false;
        ChestRenderer.sprite = null;

        transform.SetParent(GameManager.gameManager.ItemParent);
        transform.position = pos;
        objectCollider.enabled = true;
        rb.isKinematic = false;
       
        isThrown = true;

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
