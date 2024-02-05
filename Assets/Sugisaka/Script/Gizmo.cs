using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] enum GizmoType
    {
        Sphere,
        Cube,
        WireSphere,
        WireCube,
    }

    [SerializeField] GizmoType gizmoType = GizmoType.Sphere;
    [SerializeField] float gizmoSize = 0.3f;
    [SerializeField] Vector3 gizmosize3 = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] Color gizmoColor = Color.yellow;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        switch(gizmoType)
        {
            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position, gizmoSize);
                break;
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position, gizmosize3);
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position, gizmoSize);
                break;
            case GizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position, gizmosize3);
                break;
        }
    }
}
