using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public Image fadeImage; // �t�F�[�h�Ɏg�p����p�l���̃C���[�W�R���|�[�l���g
    public float fadeDuration = 1f; // �t�F�[�h�̎���

    private static Fade instance;
    private Coroutine fadeCoroutine; // �R���[�`���̎Q�Ƃ�ێ�����ϐ�

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
        if (fadeCoroutine != null) // �����t�F�[�h���Ȃ��~����
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

        yield return new WaitForSeconds(0.1f); // �t�F�[�h�C���Ƃ̊Ԃɏ����ҋ@����

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        fadeCoroutine = StartCoroutine(FadeIn()); // �t�F�[�h�C�����J�n����
    }
}
