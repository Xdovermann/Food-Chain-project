


using UnityEngine;
using System.Collections;
using FoodChain.BulletML;
using System.Collections.Generic;

namespace FoodChain.BulletML
{
    /// <summary>
    /// Pooling bullets
    /// </summary>
    public class BulletPoolScript : MonoBehaviour
    {
        private BulletPool defaultPool;
        private Dictionary<string, BulletPool> pools;
        public BulletManagerScript bulletManager;
        public string PoolName = "Bullets";
        private GameObject bulletRoot;
        private int frameId;

        void Start()
        {
            if (frameId == 0)
            {
                frameId = Time.frameCount;
            }

            pools = new Dictionary<string, BulletPool>();

            bulletRoot = new GameObject(PoolName);
            bulletRoot.transform.SetParent(transform);
            bulletRoot.transform.position = Vector3.zero;

          //  bulletManager = FindObjectOfType<BulletManagerScript>();
            if (bulletManager != null)
            {
                bulletManager.OnBulletSpawned += OnBulletSpawned;
                bulletManager.OnBulletDestroyed += OnBulletDestroyed;

                // Init pool from bank
                bool isDefault = true;

                foreach (var bankEntry in bulletManager.bulletBank.bullets)
                {
                    string bankName = bankEntry.name.ToLower();

                    BulletPool pool = PoolForBullet(bankEntry, bankName, isDefault);

                    if (isDefault)
                    {
                        defaultPool = pool;
                        isDefault = false;
                    }
                }
            }
        }

        void OnDestroy()
        {
            // Destroy all objects
            if (pools != null)
            {
                foreach (var p in pools)
                {
                    if (p.Value != null)
                    {
                        p.Value.DestroyPool();
                    }
                }

                pools.Clear();
            }
        }

        public BulletPool PoolForBullet(BulletBankEntry bankEntry, string bankName, bool preloadBullets)
        {
            BulletPool pool;
            if (pools.TryGetValue(bankName, out pool) == false)
            {
                // Create a sub object
                GameObject bulletParent = new GameObject(bankName);
                bulletParent.transform.parent = bulletRoot.transform;

                // Create a pool
                pool = new BulletPool(bankName, Mathf.Max(1, bankEntry.poolCapacity), bankEntry.prefab, bulletParent, preloadBullets);
                pool.Initialize();

                pools.Add(bankName, pool);
            }

            return pool;
        }

        BulletScript OnBulletSpawned(BulletObject bullet, string bulletName)
        {
            // Get a pool object
            BulletPool pool = null;
     
            if (pools.TryGetValue(bulletName, out pool) == false)
            {
                // Default
                pool = defaultPool;

            }
            else
            {
                Debug.LogError("test");
            }

            // Get a ready-to-use bullet
            if (pool != null && pool.Destroyed == false)
            {
                BulletScript bulletScript = pool.Get();
                if (bulletScript != null)
                {
                    // Make sure to assign the position
                    bulletScript.gameObject.name = bulletName;
                    bulletScript.transform.position = bullet.position;

                
                    BulletBankEntry bankEntry = bulletManager.GetBulletPrefabFromBank(bulletName);
                    bulletManager.SetBulletSettings(bullet, bankEntry, bulletScript.gameObject);
              
                    // kijk naar bb als voorbeeld
                //    bulletManager.SetTimer(bullet, bulletName, bulletScript);

                    return bulletScript;
                }
                else
                {
                    Debug.LogError("Bullet " + bulletName + " could not be instantiated because of the pool!");
                }
            }
            else
            {
                if (pool != null && pool.Destroyed)
                {
                    Debug.LogError("Noooooooooooooooooooo, you're using a dead pool!");
                }
                else
                {
                    Debug.LogError("Unknow bullet to spawn: " + bulletName);
                }
            }

            return null;
        }

        void OnBulletDestroyed(GameObject bullet)
        {
            // Get bullet
            if(bullet != null)
            {
                BulletScript bulletScript = bullet.GetComponent<BulletScript>();
                if (bulletScript != null)
                {
                    BulletPool pool = null;
                    string bulletName = bulletScript.Bullet.Label.ToLower();

                    if (pools.TryGetValue(bulletName, out pool) == false)
                    {
                        pool = defaultPool;
                    }

                    if (pool != null)
                    {
                        pool.Recycle(bulletScript);
                    }
                    else
                    {
                        Debug.LogError("Unknow bullet to destroy: " + bullet.name);
                    }
                }
            }
           
        }

        /// <summary>
        /// Load bullet's pool asynchronously
        /// </summary>
        /// <param name="bulletName"></param>
        /// <returns></returns>
        public void PreloadBullets(string bulletName)
        {
            if (pools.ContainsKey(bulletName) == false)
            {
                StartCoroutine(LoadBullets(bulletName));
            }
        }

        internal IEnumerator LoadBullets(string bulletName)
        {
            // Find entry
            BulletBankEntry bankEntry = bulletManager.GetBulletPrefabFromBank(bulletName);

            

            if (bankEntry == null)
            {
                Debug.LogError("POOL - Unknow bullet " + bulletName);
            }
            else
            {
                PoolForBullet(bankEntry, bulletName, true);
            }
            yield return null;
        }

        // Cleaning

        /// <summary>
        /// Delete asynchronously all objects of a pool
        /// </summary>
        /// <param name="bulletName"></param>
        public void RemoveBullets(string bulletName)
        {
            BulletPool pool;
            if (pools.TryGetValue(bulletName, out pool))
            {
                StartCoroutine(CleanPool(bulletName, pool));
            }
        }

        internal IEnumerator CleanPool(string bulletName, BulletPool pool)
        {
            pools.Remove(bulletName);
            pool.Clear();

            yield return null;
        }
    }
}