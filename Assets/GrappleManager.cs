using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    public float GrappleRange = 2f;

    public Transform GrabParent;

    [HideInInspector]
    public GameObject GrappledObject;
    [HideInInspector]
    public ThrowableObject throwableObject;


    [Header("Layers")]
    public LayerMask CollidbleLayer;
    private Movement MovementManager;
    public static GrappleManager grappleManager;


    private void Awake()
    {
        MovementManager = GetComponent<Movement>();
        grappleManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (GrappledObject == null)
            {
           
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,10, CollidbleLayer);
                if (hit.collider != null)
                {

                    //  if (hit.collider.gameObject.CompareTag("Enemy"))
                    //  {
                    float Distance = Vector2.Distance(transform.position, hit.transform.position);

                        if (Distance <= GrappleRange)
                        {
                            GrappledObject = hit.collider.gameObject;
                            GrabObject();

                        }

                   // }

                }
            }
            else
            {
                // we hebben een object vast dus gooi hem
                ThrowEnemy();
            }

            
           


        }else if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, CollidbleLayer);
            if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                //  if (hit.collider.gameObject.CompareTag("Enemy"))
                //  {
                float Distance = Vector2.Distance(transform.position, hit.transform.position);

                if (Distance <= GrappleRange)
                {

                    Debug.Log("equip weapon");
                    WeaponManager.weaponManager.EquipNewWeapon(hit.transform);
                }

                // }

            }
        }


    }

    private void ThrowEnemy()
    {
        
        CameraController.cameraController.Shake(MovementManager.weaponManager.mouseVector, 5, 0.15f);
        Gun weapon = throwableObject.GetComponent<Gun>();
        if (weapon != null)
        {
            throwableObject.ThrowObject(transform.position, MovementManager.weaponManager.mouseVector, true);
        }
        else
        {
            throwableObject.ThrowObject(transform.position, MovementManager.weaponManager.mouseVector, false);
            Base_Enemy_AI isEnemy = throwableObject.GetComponent<Base_Enemy_AI>();
            if(isEnemy != null)
            {
                isEnemy.EnemyIsThrown();
            }
        }
        
        GrappledObject = null;
        throwableObject = null;
    }

    private void GrabObject()
    {
        Vector3 GrabDir = (GrappledObject.transform.position - transform.position).normalized;
        CameraController.cameraController.Shake(GrabDir,3,0.1f);

        throwableObject = GrappledObject.GetComponent<ThrowableObject>();

        if(throwableObject.grabState == ThrowableObject.GrabState.Throwable)
        {
            Base_Enemy_AI isEnemy = throwableObject.GetComponent<Base_Enemy_AI>();
            if (isEnemy != null)
            {
                isEnemy.EnemyIsGrabbed();
            }

            throwableObject.GrabObject(GrabParent);
        }
        else
        {
            Debug.Log("Object to heavy to grab");
        }
      
     
    }

 

    void OnDrawGizmos()
    {
   //     Gizmos.color = Color.cyan;

  

      //  Gizmos.DrawWireSphere((Vector2)transform.position , GrappleRange);
   
    }
}
