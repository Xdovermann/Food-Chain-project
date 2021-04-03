using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
   


    public float moveSpeed = 10;
    public float rotationSpeed = 10;

    public int playerSortingOrder = 20;

    public Transform Weapon;
    public Transform weaponRendererParent;

    public float angle;
    Vector3 mousePos;
  public  Vector3 mouseVector;

    public float Offset = 0;
    public int WeaponSide = 1;


    public Movement playerMovement;
    public float WeaponHandling = 0.1f;

    public Transform Shotpoint;
    public GameObject Bullet;

    public float OriginalWeaponPos;

    public float TimeBtwnShots = 0.1f;
    private float TimeBtwnShotsHolder;

    public Gun EquipedWeapon;

    private void Awake()
    {
        OriginalWeaponPos = weaponRendererParent.transform.localPosition.x;
        TimeBtwnShotsHolder = TimeBtwnShots;
        weaponManager = this;
    }

    private void Update()
    {
        GetMouseInput();
        Animation();

       
    }

 
    private void GetMouseInput()
    {
     
        if(TimeBtwnShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                GameObject go = Instantiate(Bullet, Shotpoint.position, Shotpoint.rotation);
                go.GetComponent<Bullet>().Setup(mouseVector);

                GameObject Effect = ObjectPooler.GunShotEffect.GetObject();
                Effect.transform.position = Shotpoint.position;
                Effect.SetActive(true);
                Effect.transform.DOShakeScale(0.1f,0.5f);

                TimeBtwnShots = TimeBtwnShotsHolder;
                WeaponJuice();


            }
        }
        else
        {
            TimeBtwnShots -= Time.deltaTime;
        }

       




    }



    private void Animation()
    {
        transform.DOMove(playerMovement.transform.position, WeaponHandling);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //position of cursor in world
        mousePos.z = transform.position.z; //keep the z position consistant, since we're in 2d
        mouseVector = (mousePos - transform.position).normalized; //normalized vector from player pointing to cursor
        float gunAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg; //find angle in degrees from player to cursor
        Weapon.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back); //rotate gun sprite around that angle


        if (angle >= 180 && angle <= 360)
        {
           // weaponRendererParent.sortingOrder = -1;
        }
        else
        {
          //  weaponRendererParent.sortingOrder = 0;
        }

        PlayerDirection();


    }

  

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void PlayerDirection()
    {
        if (playerMovement.wallGrab || !playerMovement.canMove)
            return;



        if (mouseVector.x > 0)
        {
            playerMovement.side = 1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRendererParent.localScale = new Vector3(weaponRendererParent.localScale.x, 1, weaponRendererParent.localScale.z);
        }
        else if (mouseVector.x < 0)
        {
            playerMovement.side = -1;
            playerMovement.animationManager.Flip(playerMovement.side);
            weaponRendererParent.localScale = new Vector3(weaponRendererParent.localScale.x, -1, weaponRendererParent.localScale.z);
        }
    }

    private void WeaponJuice()
    {
        // camerashake
        float power = Random.Range(1.5f, 2.5f);
        float randTime = Random.Range(0.05f, 0.1f);
        CameraController.cameraController.Shake(-mouseVector,power, randTime);

        //weaponscale
        weaponRendererParent.transform.localScale = new Vector3(1, 1, 1);
        weaponRendererParent.transform.DOShakeScale(0.1f);

        // weaponknockback
        Sequence mySequence = DOTween.Sequence();
        float MovePoint = OriginalWeaponPos;
        MovePoint -= Random.Range(0.25f, 0.5f);
        mySequence.Append(weaponRendererParent.transform.DOLocalMoveX(MovePoint, 0.1f));
        mySequence.Append(weaponRendererParent.transform.DOLocalMoveX(OriginalWeaponPos, 0.1f));


    }

    public void EquipWeapon(Transform weaponToEquip)
    {
        if(EquipedWeapon != null)
        {
            EquipedWeapon.GetComponent<ThrowableObject>().ThrowObject(transform.position, Random.onUnitSphere * 1f);
            EquipedWeapon = null;
        }

   
        weaponToEquip.transform.SetParent(weaponRendererParent);
        weaponToEquip.transform.localRotation = new Quaternion(0, 0, 0, 0);
        weaponToEquip.GetComponent<ThrowableObject>().DisablePhysics();
        weaponToEquip.transform.localPosition = new Vector3(0, 0, 0);
        weaponToEquip.transform.localScale = new Vector3(1, 1, 1);

        EquipedWeapon = weaponToEquip.GetComponent<Gun>();
    }
}
