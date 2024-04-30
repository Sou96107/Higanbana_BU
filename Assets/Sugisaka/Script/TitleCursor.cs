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
        //- InputAction�̐ݒ�
        // InputAction������
        input = new PlayerInput();
        // �e���ڂ̐ݒ�
        input.UI.Move.started += OnMove;
        input.UI.Move.canceled += OnMove;
        input.UI.Move.performed += OnMove;
        input.UI.Decition.performed += OnDecition;
        // InputAction�L����
        input.Enable();

        // Rigidbody2D�擾
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        //- InputAction�I������
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
        // �I�����Ă�����
        

    }
    */

    [SerializeField, Header("UI")]
    private GameObject[] uiObj;
    [SerializeField, Header("�J�[�\��UI")]
    private GameObject[] cursorObj;
    [SerializeField, Header("�J�[�\���ړ����x")]
    private float moveSpeed = 10.0f;
    [SerializeField]
    int selectNum;



    float ScreanWidth = Screen.width;           // 
    float ScreanHeight = Screen.height;         // 
    bool isKeyBoard;                            // �L�[�{�[�h���삩�ǂ���
    Vector2 input;                              // ���͊i�[�p
    public bool isFinish;                       // �J�ڒ��t���O
    [SerializeField] GameObject m_SelectObj;    // �I���I�u�W�F�N�g�ێ��p
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

        //- Tab�L�[�ŃL�[�{�[�h����ƃR���g���[���[����ύX
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isKeyBoard ^= true;
            Debug.Log("�L�[�{�[�h����F" + isKeyBoard);
        }

        // keyboard���R���g���[���[���ڑ�����ĂȂ���
        if (!isKeyBoard && Gamepad.current == null)
            return;

        // ���͏���
        //CheckInput();


        // Enter(����)��������
        if (CheckDecition())
        {
            if (m_SelectObj == null)
                return;

            isSelect = CheckDecition();

            // SE�Đ�
            SoundManager.GetComponent<SoundManager>().PlaySE("���艹_���C");
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
        {// �L�[�{�[�h����
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
        {// �p�b�h����
            var current = Gamepad.current;
            input = current.leftStick.ReadValue();
        }

        // �ړ��ʌv�Z
        Vector3 move = new Vector3(input.x * moveSpeed, input.y * moveSpeed, 0.0f);

        // �ړ�
        transform.position += move;

        // ��ʊO�ɏo�Ȃ��悤�ɂ���
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
    //    Debug.Log("�I��" + collision.gameObject.tag);

    //    // SE�Đ�
    //    SoundManager.GetComponent<SoundManager>().PlaySE("�I����");

    //    // �I�������I�u�W�F�N�g��ێ�
    //    m_SelectObj = collision.gameObject;


    //    collision.gameObject.GetComponent<SelectUIBase>().StartChangeScale();
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // �I�������I�u�W�F�N�g������
    //    m_SelectObj = null;

    //    collision.gameObject.GetComponent<SelectUIBase>().StopChangeScale();
    //}

    private void glassSE()
    {
        Debug.Log("������");
        SoundManager.GetComponent<SoundManager>().PlaySE("���艹");
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
        // SE�Đ�
        SoundManager.GetComponent<SoundManager>().PlaySE("�I����");

        if (m_SelectObj.tag == "TitleUI")
        {
            m_SelectObj.GetComponent<SelectUIBase>().StopChangeScaleTitle();
            // �I�������I�u�W�F�N�g��ێ�
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScaleTitle();
        }
        else if(m_SelectObj.tag == "PauseUI")
        {
            m_SelectObj.GetComponent<SelectUIBase>().StopChangeScalePause();
            // �I�������I�u�W�F�N�g��ێ�
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
        // SE�Đ�
        SoundManager.GetComponent<SoundManager>().PlaySE("�I����");

        if (m_SelectObj.tag == "TitleUI")
        {
            m_SelectObj.gameObject.GetComponent<SelectUIBase>().StopChangeScaleTitle();
            // �I�������I�u�W�F�N�g��ێ�
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScaleTitle();
        }
        else if (m_SelectObj.tag == "PauseUI")
        {
            m_SelectObj.gameObject.GetComponent<SelectUIBase>().StopChangeScalePause();
            // �I�������I�u�W�F�N�g��ێ�
            m_SelectObj = uiObj[selectNum].gameObject;

            m_SelectObj.GetComponent<SelectUIBase>().StartChangeScalePause();
        }
    }
}