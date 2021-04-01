using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform ScopeSlot;
    public Transform BarrelSlot;
    public Transform StockSlot;
    public Transform MagazinSlot;

    [HideInInspector]
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ThrowWeapon()
    {
        rb.AddForce(Random.onUnitSphere * 10, ForceMode2D.Impulse);
    }
}
