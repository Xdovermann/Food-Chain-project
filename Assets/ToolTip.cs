using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ObjectToFollow;

    public GameObject Tooltip;
    private bool ToolTipOn;

    public static ToolTip tooltip;
    public Material RaritylineRenderer;

    private void Awake()
    {
        DisableToolTip();
        tooltip = this;
    }

    // Update is called once per frame
    void Update()
    {
        Tooltip.transform.position = ObjectToFollow.transform.position;
    }

    public void EnableToolTip()
    {
        Tooltip.gameObject.SetActive(true);
        ToolTipOn = true;
    }

    public void DisableToolTip()
    {
        Tooltip.gameObject.SetActive(false);
        ToolTipOn = false;
    }

    public bool ToolTipStatus()
    {
        return ToolTipOn;
    }
}
