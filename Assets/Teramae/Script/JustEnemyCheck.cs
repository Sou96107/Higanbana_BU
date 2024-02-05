using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustEnemyCheck : MonoBehaviour
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
        if (other.gameObject.tag == "Enemy")
            player.GetComponent<Player>().SetAutAtkEnemy(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            player.GetComponent<Player>().SetAutAtkEnemy(null);
    }
}
