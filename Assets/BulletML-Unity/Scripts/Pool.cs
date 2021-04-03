
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FoodChain
{
    public abstract class Pool<T>
    {
        /// <summary>
        /// Create some objects at the pool creation
        /// </summary>
        public bool objectWarmup = true;
        public bool autoAdjust = true;

        private Queue<T> freeSlots;
        private List<T> busySlots;
        private int capacity;

        private int frameId;
        private bool hasBeenDestroyed = false;

        public Pool(int capacity)
        {
            this.capacity = capacity;
        }

        /// <summary>
        /// Create the pool
        /// </summary>
        public virtual void Initialize()
        {
            if (frameId == 0)
            {
                frameId = Time.frameCount;
            }

            // Initialize cache
            freeSlots = new Queue<T>();
            busySlots = new List<T>();

            if (objectWarmup)
            {
                Adjust();
            }

            //Debug.Log("POOL " + frameId + "/" + typeof(T).Name + " - Initialized");
        }

        protected void Adjust()
        {
            for (int i = 0; i < capacity; i++)
            {
                T newSpace = Create();
                if (newSpace != null)
                {
                    Recycle(newSpace);
                }
            }
        }

        /// <summary>
        /// Create a new object
        /// </summary>
        /// <returns></returns>
        protected abstract T Create();

        /// <summary>
        /// Get a free space
        /// </summary>
        /// <returns></returns>
        public virtual T Get()
        {
            if (hasBeenDestroyed)
            {
                Debug.LogError("POOL " + frameId + "/" + typeof(T).Name + " - Using a dead pool!");
            }

            if (freeSlots.Count > 0)
            {
                T slot = freeSlots.Dequeue();
                busySlots.Add(slot);

                if (slot == null)
                {
                    Debug.LogError("POOL " + frameId + "/" + typeof(T).Name + " - Something wrong happened in adjust, the free slot is null! Check Recycle?");
                }
                else
                {
                    Assign(slot);
                }
                return slot;
            }
            else
            {

                if (autoAdjust == false)
                {
                    Debug.LogError("POOL " + frameId + "/" + typeof(T).Name + " - Not enough space!");
                    return default(T);
                }
                else
                {
                    // Not the initialization?
                    if (busySlots.Count > 0)
                    {
                        Debug.LogWarning("POOL " + frameId + "/" + typeof(T).Name + " - capacity is too small, allocating " + capacity + " new " + Name);
                    }
                    Adjust();

                    return Get();
                }
            }
        }

        /// <summary>
        /// Enable an object
        /// </summary>
        /// <param name="objectToActivate"></param>
        public abstract void Assign(T objectToActivate);

        /// <summary>
        /// Remove/disable an existing object
        /// </summary>
        /// <param name="objectToRelease"></param>
        public virtual void Recycle(T objectToRelease)
        {
            busySlots.Remove(objectToRelease);
            freeSlots.Enqueue(objectToRelease);
        }

        /// <summary>
        /// Remove all objects
        /// </summary>
        public virtual void Clear()
        {
            foreach (var d in busySlots)
            {
                Recycle(d);
                freeSlots.Enqueue(d);
            }

            busySlots.Clear();

            //Debug.Log("POOL " + frameId + "/" + typeof(T).Name + " - Cleared");
        }

        public virtual void DestroyPool()
        {
            foreach (var d in freeSlots)
            {
                Delete(d);
            }

            busySlots.Clear();
            freeSlots.Clear();

            hasBeenDestroyed = true;

            //Debug.Log("POOL " + frameId + "/" + typeof(T).Name + " - Destroyed");
        }

        /// <summary>
        /// Delete an object permanently
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract void Delete(T obj);

        public abstract string Name
        {
            get;
        }

        public bool Destroyed
        {
            get
            {
                return hasBeenDestroyed;
            }
        }
    }
}