using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
    private PlayerController player;

    public float moveSpeed = 10;
    Vector3 mousePos, mouseVector;
    int playerSortingOrder = 20;

    public Transform Weapon;
    public SpriteRenderer weaponRenderer;

    void Start()
    {
        player = PlayerController.player;
    }

    
    void Update()
    {
        GetMouseInput();
        Animation();

        transform.position= Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    void GetMouseInput()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mouseVector = (mousePos - transform.position).normalized;

    }

    void Animation()
    {
        float gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        Weapon.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back);

        weaponRenderer.sortingOrder = playerSortingOrder - 1;
        if (gunAngle > 0)
        {
            weaponRenderer.sortingOrder = playerSortingOrder + 1;
        }
    }
}
