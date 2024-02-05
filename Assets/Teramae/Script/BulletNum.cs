using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletNum : MonoBehaviour
{
    private GameObject player;

    private TextMeshProUGUI text;
    private Player playerCS;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        text = GetComponent<TextMeshProUGUI>();
        playerCS = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerCS.GetBulletNum().ToString();
    }
}
