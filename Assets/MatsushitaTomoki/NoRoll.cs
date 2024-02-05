using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRoll : MonoBehaviour
{
    [Header("í«ê’ëŒè€Transform")]
    public Transform followTarget;
    public float posY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            Vector3 newPosition = followTarget.position;
            newPosition.y += posY;
            transform.position = newPosition;
        }
    }
}
