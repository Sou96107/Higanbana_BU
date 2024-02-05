using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fadetext : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C������")]
    float FadeInTime = 2.0f;
    [SerializeField, Header("�t�F�[�h�A�E�g����")]
    float FadeOutTime = 2.0f;
    [SerializeField, Header("�t�F�[�h�C����̑҂�����")]
    float FadeInWaitTime = 1.0f;
    [SerializeField, Header("�t�F�[�h�A�E�g��̑҂�����")]
    float FadeOutWaitTime = 1.0f;

    private void Start()
    {
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    public void Fadestart()
    {
        StartCoroutine("FadeIn");
        StartCoroutine("FadeOut");
    }

    void FadeEnd()
    {
        SceneManager.LoadScene("Title");
    }

    // �t�F�[�h�C��
    IEnumerator FadeIn()
    {
        float time = 0;
        while (time <= FadeInTime)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, time / FadeInTime);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(FadeInWaitTime);
    }

    // �t�F�[�h�A�E�g
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(FadeInTime + FadeInWaitTime);

        float time = 0;
        while (time <= FadeOutTime)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1 - time / FadeOutTime);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(FadeOutWaitTime);
        FadeEnd();
    }
}
