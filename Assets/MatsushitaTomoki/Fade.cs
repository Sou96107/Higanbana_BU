using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public Image fadeImage; // フェードに使用するパネルのイメージコンポーネント
    public float fadeDuration = 1f; // フェードの時間

    private static Fade instance;
    private Coroutine fadeCoroutine; // コルーチンの参照を保持する変数

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.black;
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (fadeCoroutine != null) // もしフェード中なら停止する
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.black, Color.clear, timer / fadeDuration);
            yield return null;
        }
    }


    IEnumerator FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.clear;

        yield return new WaitForSeconds(0.1f); // フェードインとの間に少し待機する

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        fadeCoroutine = StartCoroutine(FadeIn()); // フェードインを開始する
    }
}
