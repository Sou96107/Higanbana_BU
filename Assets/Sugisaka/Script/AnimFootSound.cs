using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFootSound : MonoBehaviour
{
    private GameObject SoundManager;

    private void Start()
    {
        SoundManager = GameObject.Find("SoundManager");
    }

    void FootSound()
    {
        SoundManager.GetComponent<SoundManager>().PlaySE("ï‡çs");
    }
}
