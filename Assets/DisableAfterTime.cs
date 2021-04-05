using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float DisableTimer=0.1f;
    public bool DestroyTheObject = false;
    private Vector2 scale;
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(DisableObject());
        scale = transform.localScale;
    }

    private IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(DisableTimer);
        if (DestroyTheObject)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = scale;
            gameObject.SetActive(false);
        }
       
    }
}
