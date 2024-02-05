using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fadetext : MonoBehaviour
{
    [SerializeField, Header("フェードイン時間")]
    float FadeInTime = 2.0f;
    [SerializeField, Header("フェードアウト時間")]
    float FadeOutTime = 2.0f;
    [SerializeField, Header("フェードイン後の待ち時間")]
    float FadeInWaitTime = 1.0f;
    [SerializeField, Header("フェードアウト後の待ち時間")]
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

    // フェードイン
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

    // フェードアウト
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
