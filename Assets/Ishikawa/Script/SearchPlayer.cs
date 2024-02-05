using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayer : MonoBehaviour
{
    RaycastHit hit;
    Enemy EnemyCom;
    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        EnemyCom = gameObject.transform.parent.gameObject.GetComponent<Enemy>();
        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (isStart)
        {
            if (other.transform.tag.Contains("Player"))
            {

                var diff = other.transform.position - transform.position;
                var distance = diff.magnitude;
                var direction = diff.normalized;

                if (Physics.Raycast(transform.position, direction, out hit, distance))
                {

                    if (hit.transform.tag.Contains("Player"))
                    {
                        if (!EnemyCom.GetSetIsFound) // Œ©‚Â‚¯‚Ä‚È‚©‚Á‚½‚ç
                        {
                            EnemyCom.GetSetIsFound = true;
                            //int rand = Random.Range(1, 3); // 1`2
                            EnemyCom.ChangeState(2);
                        }

                    }
                    else
                    {
                        // ˆê“xŒ©‚Â‚¯‚Ä‚¢‚½‚çŒ©¸‚Á‚½‚Æ‚«‚ÌPlayer‚ÌêŠ‚ğæ“¾‚µ‚Ä‚¨‚­
                        if (EnemyCom.GetSetIsFound)
                        {
                            EnemyCom.GetSetLostPos = other.transform.position;

                            EnemyCom.GetSetIsFound = false;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Contains("Player"))
        {
            EnemyCom.GetSetLostPos = other.transform.position;
            EnemyCom.GetSetIsFound = false;
        }
    }

    public void IsStart()
    {
        isStart = true;
    }
}
