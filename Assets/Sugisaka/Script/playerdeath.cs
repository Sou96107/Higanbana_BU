using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerdeath : MonoBehaviour
{
    private GameObject fadeUI;

    void Start()
    {
        fadeUI = GameObject.Find("FadeInCon");
    }

    void DeathAnimFin()
    {
        GameObject SoundObject = GameObject.Find("SoundManager");

        SoundObject.GetComponent<SoundManager>().StopBGM("Game");
        SoundObject.GetComponent<SoundManager>().PlayBGM("GameOver");

        fadeUI.GetComponent<FadeIn>().FadeOnly(); // ゲームオーバー表示


        //Destroy(GameObject.Find("Player"));
    }

    void DeathSE()
    {
        GameObject SoundObject = GameObject.Find("SoundManager");
        SoundObject.GetComponent<SoundManager>().PlaySE("倒れる");
    }

}
