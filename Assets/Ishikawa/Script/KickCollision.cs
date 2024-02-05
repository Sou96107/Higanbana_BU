using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickCollision : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    public bool hit = false;
    GameObject playerObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hit)
            return;

        if (other.transform.tag.Contains("Player"))
        {
            hit = true;
            Vector3 v = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

            other.transform.parent.GetComponent<Player>().Blow(v,enemy.GetKickPower());
            //playerObj = other.transform.parent.gameObject;
        }
    }

    private void OnEnable()
    {
        hit = false;
    }

    
}
