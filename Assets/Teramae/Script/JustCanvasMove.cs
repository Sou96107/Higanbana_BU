using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCanvasMove : MonoBehaviour
{
    Vector3 PlayerPos;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPos = transform.parent.transform.position;
        PlayerPos.y += 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPos = transform.parent.transform.position;
        PlayerPos.y += 2.5f;
        transform.position = PlayerPos;
    }
}
