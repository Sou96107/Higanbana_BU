using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCheck : MonoBehaviour
{
    [SerializeField, Header("ÉvÉåÉCÉÑÅ[")]
    private GameObject player;

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
        if (other.gameObject.tag == "EnemyBullet")
        {
            player.GetComponent<Player>().SetEnemyBullet(other.gameObject);
            Debug.Log("ìGÇÃíeî≠å©");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            player.GetComponent<Player>().SetEnemyBullet(null);
            Debug.Log("ìGÇÃíeå©é∏Ç¢");
        }
    }
}
