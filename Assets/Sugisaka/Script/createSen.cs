using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class createSen : MonoBehaviour
{
    // 弾道予測線オブジェクト
    [SerializeField] GameObject self;                   // pivot
    [SerializeField] public Transform player;           // プレイヤー

    [Header("予測線の色")]
    [SerializeField] Material LineColor;
    [SerializeField] private Color startcolor;
    [SerializeField] private Color endcolor;
    [Range(0, 1)]
    public float colorRange; 
    [Header("線の太さ")]
    [SerializeField] Vector2 LineWide = new (0.05f, 0.05f);

    private RaycastHit hit;                             // 
    private float distance = 20f;                       // 
    private Vector3 startpos = new (0, 0, 0);           // 
    private Vector3 targetpos = new (0, 0, 0);          // 
    private Vector3[] positions;                        // 
    LineRenderer lineRenderer;
    

    void Start()
    {
        // ラインレンダラーの設定
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //オブジェクトの座標を取得
        positions = new Vector3[]
        {
            self.transform.position,
            self.transform.position
        };

        //線の太さを設定
        lineRenderer.startWidth = LineWide.x;
        lineRenderer.endWidth = LineWide.y;

        //マテリアルの設定
        var material = LineColor;
        lineRenderer.material = material;

        // 最初は非表示
        //lineRenderer.enabled = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // プレイヤーに対してレイをとばす
    //        // レイを飛ばす方向の計算、正規化
    //        Vector3 temp = player.transform.position - self.transform.position;
    //        Vector3 direction = temp.normalized;
    //        // レイを飛ばす
    //        Ray ray = new Ray(self.transform.position, direction);
    //        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 0.1f);
    //        // レイが当たったオブジェクトを調べる
    //        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
    //        {
    //            // レイが当たった場所に予測線を表示
    //            // 既に予測線が表示されている場合は場所を変更する
    //            positions[0] = self.transform.position;
    //            //positions[1] = hit.transform.position;
    //            positions[1] = hit.point;
    //            lineRenderer.SetPositions(positions);
    //        }
    //    }
    //}
    //private void Update()
    //{
    //    LineColor.SetColor("_EmissionColor", Color.Lerp(startcolor, endcolor, colorRange));
    //    lineRenderer.material = LineColor;
    //    positions[0] = self.transform.position;
    //    positions[1] = player.position;
    //    lineRenderer.SetPositions(positions);
    //}

    public void DrawLine()
    {
        // レイを飛ばす方向の計算、正規化
        Vector3 temp = player.position - self.transform.position;
        Vector3 direction = temp.normalized;

        // レイを飛ばす
        Ray ray = new (self.transform.position, direction);
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, 0.1f);

        // レイが当たったオブジェクトを調べる
        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
        {
            lineRenderer.enabled = true;
            // レイが当たった場所に予測線を表示
            // 既に予測線が表示されている場合は場所を変更する
            positions[0] = self.transform.position;
            positions[1] = player.position;
            lineRenderer.SetPositions(positions);
        }
    }
    public void DrawLine2()
    {
        Vector3 temp = targetpos - startpos;
        Vector3 direction = temp.normalized;

        // レイを飛ばす
        Ray ray = new (self.transform.position, direction);
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, 0.1f);

        // レイが当たったオブジェクトを調べる
        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
        {
            positions[0] = self.transform.position;
            positions[1] = hit.point;

            // 弾道の表示がしたいならここを有効にする
            //lineRenderer.SetPositions(positions);
        }
    }


    public void SetPos(Transform Target, Transform startPos)
    {
        player = Target;
        targetpos = player.position;
        startpos = startPos.position;
    }
    public void EnebleLine()
    {
        lineRenderer.enabled = false;
    }
    public void SetLineColor(float time)
    {
        //time *= 
        colorRange = time;
        LineColor.SetColor("_EmissionColor", Color.Lerp(startcolor, endcolor, colorRange));
        lineRenderer.material = LineColor;
    }
}
