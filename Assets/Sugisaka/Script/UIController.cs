using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField, Header("動かすUI")]
    private RectTransform canvasRectTfm;
    [SerializeField, Header("プレイヤー")]
    private Transform targetTfm;
    [SerializeField, Header("オフセット")]
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    private RectTransform myRectTfm;    // スタミナバーの情報格納

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 pos;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTfm.position + offset);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTfm, screenPos, Camera.main, out pos);

        myRectTfm.position = pos;
    }

}
