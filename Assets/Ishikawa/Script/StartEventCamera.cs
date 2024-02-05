using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEventCamera : MonoBehaviour
{
    [SerializeField, Header("ターゲット")] Transform Target;
    [SerializeField, Header("到着点")] Transform ArrivalPos;
    [SerializeField, Header("移動スピード")] float Speed = 0.05f;
    float m_Interpolation;
    // Start is called before the first frame update
    void Start()
    {
        m_Interpolation = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.LookAt(Target);

        transform.position = Vector3.Lerp(transform.position, ArrivalPos.position, m_Interpolation);

        m_Interpolation += Speed * Time.deltaTime;

        if (m_Interpolation >= 1.0f)
            this.enabled = false;
    }


}
