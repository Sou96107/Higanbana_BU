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

        //���Ԍo�߂��Ă���ɃV�[���ړ�����
        fade.FadeOut(time);
    }
}
