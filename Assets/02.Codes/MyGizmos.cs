using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float _radious = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radious);
        Gizmos.DrawSphere(transform.position, _radious);
    }
}
