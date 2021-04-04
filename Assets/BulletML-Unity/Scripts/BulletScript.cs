
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Collections;

namespace FoodChain.BulletML
{
  /// <summary>
  /// Script attached to the bullet sprite
  /// </summary>
  public class BulletScript : MonoBehaviour
  {
    /// <summary>
    /// Rotate the projectile with the fire angle
    /// </summary>
    public bool autoRotation = true;

    /// <summary>
    /// Add those degrees to the rotation
    /// </summary>
    public float autoRotationAngleBonus = 0;

    private Renderer[] renderers;


        public BulletPool pool;

        void Update()
    {
      // Do we have a valid bullet?
      if (bullet != null)
      {
        // Update data
        bullet.Update();

        // Change position

               if(bullet != null)
                {
                    this.transform.position = Bullet.position;

                    if (this.autoRotation)
                    {
                        this.transform.rotation = Quaternion.identity;
                        this.transform.Rotate(0, 0, ((bullet.Direction * Mathf.Rad2Deg) - 90) + autoRotationAngleBonus);
                    }
                }
     

        // Orientation
       
      }

      // Out of screen + autodestruction
      //if (DestroyWhenOutOfScreen)
      //{
      //  if (renderers == null)
      //  {
      //    renderers = GetComponentsInChildren<Renderer>();
      //  }

      //  bool isVisible = true;
      //  foreach (Renderer r in renderers)
      //  {
      //    isVisible &= (r.isVisible);
      //  }

      //  if (isVisible == false)
      //  {
      //    OnDestroy();
      //  }
      //}
    }

    void OnDestroy()
    {
      RemoveBullet();
    }

    public void RemoveBullet()
    {
      // If the object has been killed by a "Destroy" command, we need to make sure the engine is clean
      if (bullet != null)
      {
        bullet.MyBulletManager.RemoveBullet(bullet);
      }
    }

    private BulletObject bullet;

    /// <summary>
    /// Attached bullet object
    /// </summary>
    public BulletObject Bullet
    {
      get
      {
        return bullet;
      }
      set
      {
        bullet = value;

        if(bullet != null)
        {
           bullet.Parent = this.gameObject;
         }                
      
               
              
             
             
                  
      }
    }

    /// <summary>
    /// Destroy if outside the screen
    /// </summary>
    public bool DestroyWhenOutOfScreen
    {
      get;
      set;
    }
  }
}