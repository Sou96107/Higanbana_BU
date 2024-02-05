using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    [SerializeField] FadeT fade;

    float FadeTime = FadeIn.FadeTime;

    // Start is called before the first frame update
    void Start()
    {
        float time = FadeTime;

        //ŠÔŒo‰ß‚µ‚Ä‚©‚ç‚ÉƒV[ƒ“ˆÚ“®‚·‚é
        fade.FadeOut(time);
    }
}
