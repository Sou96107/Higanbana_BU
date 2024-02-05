/*
 トランジションフェード
 そのままだとフェードアウトが上手く行かないので改良
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class FadeT : MonoBehaviour
{
    IFade fade;
    [SerializeField] bool startFade;

    void Start()
    {
        Init();
        fade.Range = cutoutRange;
    }

    float cutoutRange;

    void Init()
    {
        if (startFade)
        {
            cutoutRange = 1;
        }
        fade = GetComponent<IFade>();
    }

    void OnValidate()
    {
        Init();
        fade.Range = cutoutRange;
    }

    IEnumerator FadeoutCoroutine(float time, System.Action action)
    {
        float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
            fade.Range = cutoutRange;
            yield return endFrame;
        }
        cutoutRange = 0;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    IEnumerator FadeinCoroutine(float time, System.Action action)
    {
        float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);

        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
        {
            cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
            fade.Range = cutoutRange;
            yield return endFrame;
        }
        cutoutRange = 1;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    public Coroutine FadeOut(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeoutCoroutine(time, action));
    }

    public Coroutine FadeOut(float time)
    {
        return FadeOut(time, null);
    }

    public Coroutine FadeIn(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeinCoroutine(time, action));
    }

    public Coroutine FadeIn(float time)
    {
        return FadeIn(time, null);
    }
}