using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleCursor : MonoBehaviour
{
    /*
    private PlayerInput input;
    private Rigidbody2D rb2;
    public Vector2 move;

    void Start()
    {
        //- InputActionの設定
        // InputAction初期化
        input = new PlayerInput();
        // 各項目の設定
        input.UI.Move.started += OnMove;
        input.UI.Move.canceled += OnMove;
        input.UI.Move.performed += OnMove;
        input.UI.Decition.performed += OnDecition;
        // InputAction有効化
        input.Enable();

        // Rigidbody2D取得
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        //- InputAction終了処理
        input.Dispose();
    }

    private void Update()
    {
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        RectTransform.
        //rb2.AddForce(move);
    }

    private void OnDecition(InputAction.CallbackContext context)
    {
        // 選択していたら
        

    }
    */

    [SerializeField, Header("UI")]
    private GameObject[] uiObj;
    [SerializeField, Header("カーソルUI")]
    private GameObject[] cursorObj;
    [SerializeField, Header("カーソル移動速度")]
    private float moveSpeed = 10.0f;
    [SerializeField]
    int selectNum;



    float ScreanWidth = Screen.width;           // 
    float ScreanHeight = Screen.height;         // 
    bool isKeyBoard;                            // キーボード操作かどうか
    Vector2 input;                              // 入力格納用
    public bool isFinish;                       // 遷移中フラグ
    [SerializeField] GameObject m_SelectObj;    // 選択オブジェクト保持用
    GameObject SoundManager;
    private UIinput inputUI;
    bool isSelect;
    float selectTime;
    bool isOnce;

    private void Start()
    {
        input = Vector2.zero;
        SoundManager = GameObject.Find("SoundManager");

        inputUI = new UIinput();
        inputUI.UI.SelectUP.performed += OnSelectUP;
        inputUI.UI.SelectDown.performed += OnSelectDown;
        inputUI.Enable();

        selectNum = 0;
        transform.position = uiObj[selectNum].transform.position;
        m_SelectObj = uiObj[selectNum]; 
        if (m_SelectObj.tag == "TitleUI")
            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScaleTitle();
        else if (m_SelectObj.tag == "PauseUI")
            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScalePause();
        isSelect = false;
        selectTime = 0.0f;
        isOnce = false;
    }

    private void Update()
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

        // 入力処理
        //CheckInput();


        // Enter(決定)押したら
        if (CheckDecition())
        {
            if (m_SelectObj == null)
                return;

            isSelect = CheckDecition();

            // SE再生
            SoundManager.GetComponent<SoundManager>().PlaySE("決定音_発砲");
            //Invoke("glassSE", 0.2f);

            //cursorObj[0].SetActive(false);

            //StartCoroutine("a");
            //isFinish = true;
        }

        if(isSelect)
        {
            selectTime += Time.unscaledDeltaTime;
            if (selectTime >= 0.2f)
            {
                if (!isOnce)
                {
                    glassSE();
                    isOnce = true;
                }
                if (cursorObj[0].gameObject.activeSelf)
                {
                    cursorObj[0].SetActive(false);
                }
                if (selectTime >= 0.5f)
                {
                    Time.timeScale = 1.0f;
                    m_SelectObj.GetComponent<SelectUIBase>().Action();
                    isFinish = true;
                    isSelect = false;
                    isOnce = false;
                    selectTime = 0.0f;
                }
            }
        }
    }

    void CheckInput()
    {
        if (isKeyBoard)
        {// キーボード操作
            if (Input.GetKey(KeyCode.W))
                input.y = 1.0f;
            else if (Input.GetKey(KeyCode.S))
                input.y = -1.0f;
            else
                input.y = 0.0f;

            if (Input.GetKey(KeyCode.A))
                input.x = -1.0f;
            else if (Input.GetKey(KeyCode.D))
                input.x = 1.0f;
            else
                input.x = 0.0f;
        }
        else
        {// パッド操作
            var current = Gamepad.current;
            input = current.leftStick.ReadValue();
        }

        // 移動量計算
        Vector3 move = new Vector3(input.x * moveSpeed, input.y * moveSpeed, 0.0f);

        // 移動
        transform.position += move;

        // 画面外に出ないようにする
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 50.0f, ScreanWidth - 50.0f);
        pos.y = Mathf.Clamp(pos.y, 50.0f, ScreanHeight - 50.0f);
        transform.position = pos;
    }

    bool CheckDecition()
    {
        if (isKeyBoard)
        {
            if (Input.GetKey(KeyCode.Return))
                return true;
        }
        else
        {
            var current = Gamepad.current;
            var Key = current.aButton;
            if (Key.wasPressedThisFrame)
            {
                return true;
            }
        }
        return false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("選択" + collision.gameObject.tag);

    //    // SE再生
    //    SoundManager.GetComponent<SoundManager>().PlaySE("選択音");

    //    // 選択したオブジェクトを保持
    //    m_SelectObj = collision.gameObject;


    //    collision.gameObject.GetComponent<SelectUIBase>().StartChangeScale();
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 選択したオブジェクトを解除
    //    m_SelectObj = null;

    //    collision.gameObject.GetComponent<SelectUIBase>().StopChangeScale();
    //}

    private void glassSE()
    {
        Debug.Log("ううう");
        SoundManager.GetComponent<SoundManager>().PlaySE("決定音");
        cursorObj[1].SetActive(true);
    }

    IEnumerator a()
    {
        yield return new WaitForSeconds(0.7f);
        m_SelectObj.GetComponent<SelectUIBase>().Action();
    }

    private void OnSelectUP(InputAction.CallbackContext context)
    {
        selectNum--;
        if (selectNum < 0)
            selectNum = uiObj.Length - 1;
        transform.position = uiObj[selectNum].GetComponent<RectTransform>().position;
        // SE再生
        SoundManager.GetComponent<SoundManager>().PlaySE("選択音");

        if (m_SelectObj.tag == "TitleUI")
        {
            m_SelectObj.GetComponent<SelectUIBase>().StopChangeScaleTitle();
            // 選択したオブジェクトを保持
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScaleTitle();
        }
        else if(m_SelectObj.tag == "PauseUI")
        {
            m_SelectObj.GetComponent<SelectUIBase>().StopChangeScalePause();
            // 選択したオブジェクトを保持
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScalePause();
        }
    }

    private void OnSelectDown(InputAction.CallbackContext context)
    {
        selectNum++;
        if (selectNum > uiObj.Length - 1)
            selectNum = 0;
        transform.position = uiObj[selectNum].GetComponent<RectTransform>().position;
        // SE再生
        SoundManager.GetComponent<SoundManager>().PlaySE("選択音");

        if (m_SelectObj.tag == "TitleUI")
        {
            m_SelectObj.gameObject.GetComponent<SelectUIBase>().StopChangeScaleTitle();
            // 選択したオブジェクトを保持
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScaleTitle();
        }
        else if (m_SelectObj.tag == "PauseUI")
        {
            m_SelectObj.gameObject.GetComponent<SelectUIBase>().StopChangeScalePause();
            // 選択したオブジェクトを保持
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScalePause();
        }
    }
}