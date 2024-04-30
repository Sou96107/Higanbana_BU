using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField, Header("�|�[�YUI")]
    GameObject pauseUI;
    [SerializeField, Header("�J�[�\��UI")]
    private GameObject[] cursorObj;

    private Player playerCS;
    private UIinput input;
    private bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        input = new UIinput();
        input.UI.Pause.performed += OnPause;
        input.Enable();

        playerCS = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //- InputAction�I������
        input.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //- �|�[�Y�t���O�؂�ւ�
        isPause ^= true;
        if(isPause) // �|�[�Y�t���O���L���Ȃ�
        {
            //- �|�[�YUI�\��
            pauseUI.SetActive(true);
            //- �J�ڒ��t���O�𖳌���
            GameObject.Find("reticl").GetComponent<TitleCursor>().isFinish = false;
            //- �J�[�\��UI�؂�ւ�
            cursorObj[0].SetActive(true);
            cursorObj[1].SetActive(false);
            //- ���Ԃ��~�߂�
            Time.timeScale = 0f;
        }
        else
        {
            //- �|�[�YUI��\��
            pauseUI.SetActive(false);
            //- ���Ԃ�i�߂�
            Time.timeScale = 1f;
        }
    }

    public void BackGame()
    {
        isPause = false;
        pauseUI.SetActive(false);
        playerCS.SetPauseFlag(false);
        Time.timeScale = 1f;
    }
}
