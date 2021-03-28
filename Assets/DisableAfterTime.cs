using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float DisableTimer=0.1f;

    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(DisableObject());
    }

    private IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(DisableTimer);
        gameObject.SetActive(false);
    }
}
