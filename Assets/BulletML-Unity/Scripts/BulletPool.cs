
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Collections;
using FoodChain.BulletML;

namespace FoodChain.BulletML
{
    /// <summary>
    /// Link between the game and BulletML
    /// </summary>
    public class BulletPool : Pool<BulletScript>
    {
        private GameObject bulletPrefab;
        private GameObject bulletRoot;
        private string name;

        public BulletPool(string name, int capacity, GameObject prefab, GameObject root, bool preload)
          : base(capacity)
        {
            this.name = name;
            this.bulletPrefab = prefab;
            this.autoAdjust = true;
            this.objectWarmup = preload;

            bulletRoot = root;
        }

        protected override BulletScript Create()
        {
            BulletScript bullet = null;

            GameObject gameObject = GameObject.Instantiate(bulletPrefab) as GameObject;

            if (gameObject != null)
            {
                gameObject.name = bulletPrefab.name;
                gameObject.transform.parent = this.bulletRoot.transform;

                bullet = gameObject.GetComponent<BulletScript>();
                if (bullet == null)
                {
                    bullet = gameObject.AddComponent<BulletScript>();
                }

                gameObject.SetActive(false);
            }

            return bullet;
        }

        public override void Assign(BulletScript bulletScript)
        {
            // Reset the script
            if (bulletScript != null)
            {
                bulletScript.transform.position = Vector3.zero;
                bulletScript.transform.rotation = Quaternion.identity;

                bulletScript.gameObject.SetActive(true);
            }
        }

        public override void Recycle(BulletScript bulletScript)
        {
            base.Recycle(bulletScript);

            if (bulletScript.Bullet != null)
            {
                bulletScript.Bullet.Parent = null;
            }
            bulletScript.Bullet = null;
            bulletScript.gameObject.SetActive(false);
        }

        public override void Delete(BulletScript obj)
        {
            if (obj != null)
            {
                if (obj.Bullet != null)
                {
                    obj.RemoveBullet();
                }

                obj.Bullet = null;
                GameObject.Destroy(obj.gameObject);
            }
        }

        public override string Name
        {
            get { return name; }
        }
    }
}