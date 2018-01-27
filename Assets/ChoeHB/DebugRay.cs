using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRay : MonoBehaviour {

    public void DebugString(RaycastHit2D hit)
    {
        Debug.Log(hit.collider.name);
    }
}
