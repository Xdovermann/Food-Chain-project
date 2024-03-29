using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public enum ObjectPoolerType
    {
        FlashEffect,
        AmmoBox,
        DamageNumber,
    }

    public class ObjectPooler : MonoBehaviour
    {

        public static ObjectPooler FlashEffect;
        public static ObjectPooler AmmoBox;
         public static ObjectPooler DamageNumber;
    [SerializeField]
        private int amountToSpawnOnStart = 30;



        [SerializeField]
        private GameObject PooledObject;
        private bool notEnoughInPool = true;
        public List<GameObject> PoolObjects;



        public ObjectPoolerType Pooler_Type;



        private void Awake()
        {
            switch (Pooler_Type)
            {
                case ObjectPoolerType.FlashEffect:
                    FlashEffect = this;
                    break;

            case ObjectPoolerType.AmmoBox:
                AmmoBox = this;
                break;

            case ObjectPoolerType.DamageNumber:
                DamageNumber = this;
                break;

            default:
                    break;
            }

        }

        void Start()
        {


            PoolObjects = new List<GameObject>();

            for (int i = 0; i < amountToSpawnOnStart; i++)
            {

                GameObject bul = Instantiate(PooledObject, transform);

                bul.SetActive(false);
            

            }
            SetObjectList();
        }


        public GameObject GetObject()
        {
            if (PoolObjects.Count > 0)
            {
                for (int i = 0; i < PoolObjects.Count; i++)
                {

                if(PoolObjects[i] != null)
                {
                    if (!PoolObjects[i].activeInHierarchy)
                    {

                        return PoolObjects[i];
                    }
                }
                   
                }
            }

            if (notEnoughInPool)
            {
                GameObject bul = Instantiate(PooledObject, transform);


                return bul;
            }

            return null;
        }

        public void SetObjectList()
        {
            PoolObjects.Clear();
            foreach (Transform child in transform)
            {
                PoolObjects.Add(child.gameObject);
            }

           

        }

   
        public void SetObjectPosition(GameObject bul, Vector3 position)
        {
            bul.transform.position = position;
        }

    public void DisableAll()
    {
        for (int i = 0; i < PoolObjects.Count; i++)
        {
            if (PoolObjects[i] != null)
            {
                if (PoolObjects[i].activeInHierarchy)
                {
                    PoolObjects[i].SetActive(false);
                }
            }
           
        }
    }


    }


