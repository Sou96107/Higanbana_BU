using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarPos : MonoBehaviour
{
    Player player;
    Transform playerTrans;
    RectTransform myRect;
    Vector3 offset = new Vector3(150.0f, 150.0f, 0);
    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerObj = GameObject.Find("Player");
        player = PlayerObj.GetComponent<Player>();
        playerTrans = PlayerObj.transform;
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float dis = Vector3.Distance(playerTrans.position, Camera.main.transform.position);
        if (dis <= 5.0f)
        {
            // ‹ß‚¢‚È‚çÁ‚·
            myRect.position = new Vector3(-1000.0f, -1000.0f, 0.0f);
            return;
        }

        myRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, playerTrans.position);
        myRect.position += new Vector3(offset.x, offset.y, 0.0f);
        
    }
}
