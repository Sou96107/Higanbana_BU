using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCanvasMove : MonoBehaviour
{
    Transform EnemyTrans;
    // Start is called before the first frame update
    void Start()
    {
        EnemyTrans = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = EnemyTrans.position;
        Vector3 campos = Camera.main.transform.position;
        Vector3 v = new Vector3(campos.x - transform.position.x, campos.y - transform.position.y, campos.z - transform.position.z).normalized;

        transform.position += v;
    }
}
