using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public enum ObjectPoolerType
    {
        FlashEffect,
        AmmoBox,

    }

    public class ObjectPooler : MonoBehaviour
    {

        public static ObjectPooler FlashEffect;
        public static ObjectPooler AmmoBox;

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
                    if (!PoolObjects[i].activeInHierarchy)
                    {

                        return PoolObjects[i];
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


    }


