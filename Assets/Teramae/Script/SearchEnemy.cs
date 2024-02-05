using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemy : MonoBehaviour
{
    private int enemyNum;
    private bool isOnce;
    // Start is called before the first frame update
    void Start()
    {
        enemyNum = GameObject.FindGameObjectsWithTag("Enemy").Length;
        isOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemyNum = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("ìGÇÃêîÅF" + enemyNum);
        if (enemyNum == 0)
        {
            if (!isOnce)
            {
                isOnce = true;
                FadeIn.instance.NextScene("Clear");
            }
        }
    }
}
