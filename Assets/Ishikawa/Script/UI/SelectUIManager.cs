using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelectUIManager : MonoBehaviour
{
    [SerializeField, Header("セレクトUI")] GameObject[] SelectObj;
    [SerializeField, Header("連続入力間隔")] float interval = 0.1f;
    bool isKeyBoard;
    bool isFinish;
    int m_SelectNum;
    float m_frame;
    // Start is called before the first frame update
    void Start()
    {
        isKeyBoard = false;
        isFinish = false;
        m_SelectNum = m_SelectNum % SelectObj.Length;
        m_frame = 100.0f;
        SelectObj[m_SelectNum].GetComponent<SelectUIBase>().StartChangeScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinish)
            return;

        //- Tabキーでキーボード操作とコントローラー操作変更
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isKeyBoard ^= true;
            Debug.Log("キーボード操作：" + isKeyBoard);
        }

        // keyboardもコントローラーも接続されてない時
        if (!isKeyBoard && Gamepad.current == null)
            return;

        // 下を押したら(左スティック下)
        if (CheckInputDown() && m_frame >= interval)
        {
            m_frame = 0.0f;
            SelectObj[m_SelectNum].GetComponent<SelectUIBase>().StopChangeScale();
            m_SelectNum = (m_SelectNum + 1) % SelectObj.Length;
            SelectObj[m_SelectNum].GetComponent<SelectUIBase>().StartChangeScale();
        }
        
        // 上を押したら(左スティック上)
        if (CheckInputUp() && m_frame >= interval)
        {
            m_frame = 0.0f;
            SelectObj[m_SelectNum].GetComponent<SelectUIBase>().StopChangeScale();
            m_SelectNum = (m_SelectNum - 1 + SelectObj.Length) % SelectObj.Length;
            SelectObj[m_SelectNum].GetComponent<SelectUIBase>().StartChangeScale();
        }
        
        // Enter(決定)押したら
        if (CheckDecition())
        {
            SelectObj[m_SelectNum].GetComponent<SelectUIBase>().Action();
            isFinish = true;
        }

        m_frame += Time.deltaTime;
    }

    bool CheckInputDown()
    {
        if (isKeyBoard)
        {
            var current = Keyboard.current;
            var sKey = current[Key.S];
            if (sKey.isPressed)
            {
                return true;
            }
        }
        else
        {
            var current = Gamepad.current;
            float value = current.leftStick.down.ReadValue();
            if (value >= 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckInputUp()
    {
        if (isKeyBoard)
        {
            var current = Keyboard.current;
            var wKey = current[Key.W];
            if (wKey.isPressed)
            {
                return true;
            }
        }
        else
        {
            var current = Gamepad.current;
            float value = current.leftStick.up.ReadValue();
            if (value >= 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckDecition()
    {
        if (isKeyBoard)
        {
            var current = Keyboard.current;
            var enterKey = current[Key.Enter];
            if (enterKey.isPressed)
            {
                return true;
            }
        }
        else
        {
            var current = Gamepad.current;
            var Key = current.bButton;
            if (Key.wasPressedThisFrame)
            {
                return true;
            }
        }
        return false;
    }
}
