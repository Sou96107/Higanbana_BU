using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirariImage : MonoBehaviour
{
    [SerializeField,Header("目標地点")] Vector2 GoalPos;
    [SerializeField, Header("スピード")] float speed = 1.0f;
    RectTransform rect;
    Vector2 originPos;
    float interpolation;
    bool IsOn;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        originPos = new Vector2(0.0f,0.0f);
        interpolation = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (interpolation > 1.0f)
        {
            interpolation = 0.0f;
            IsOn = false;
        }

        if (!IsOn)
            return;

        rect.anchoredPosition = Vector2.Lerp(originPos, GoalPos, interpolation);

        interpolation += Time.unscaledDeltaTime * speed;
    }

    public void SetOn()
    {
        if (IsOn)
            return;
        IsOn = true;
        interpolation = 0.0f;
    }

    public void SetReset()
    {
        IsOn = false;
        interpolation = 0.0f;
        rect.anchoredPosition = Vector2.zero;
    }
}
