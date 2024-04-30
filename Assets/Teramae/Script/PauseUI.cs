using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField, Header("ポーズUI")]
    GameObject pauseUI;
    [SerializeField, Header("カーソルUI")]
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
        //- InputAction終了処理
        input.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //- ポーズフラグ切り替え
        isPause ^= true;
        if(isPause) // ポーズフラグが有効なら
        {
            //- ポーズUI表示
            pauseUI.SetActive(true);
            //- 遷移中フラグを無効化
            GameObject.Find("reticl").GetComponent<TitleCursor>().isFinish = false;
            //- カーソルUI切り替え
            cursorObj[0].SetActive(true);
            cursorObj[1].SetActive(false);
            //- 時間を止める
            Time.timeScale = 0f;
        }
        else
        {
            //- ポーズUI非表示
            pauseUI.SetActive(false);
            //- 時間を進める
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
