using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * speed * Time.deltaTime;

            Destroy(gameObject);
        }
            
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position += -transform.forward * speed * Time.deltaTime;
        //transform.position += transform.forward * speed * Time.deltaTime;
    }
}
