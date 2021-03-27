using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
    public Vector2 MapPosition;

    public void SetPosition(Vector2 pos)
    {
        MapPosition = pos;
    }
}
