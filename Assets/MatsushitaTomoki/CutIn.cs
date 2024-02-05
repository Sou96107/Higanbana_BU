using UnityEngine;
using UnityEngine.UI;

public class CutIn : MonoBehaviour
{
    public float countdownTime = 60.0f; // カウントダウンする秒数

    [SerializeField] private float currentTime;

    private void Start()
    {
        currentTime = countdownTime;
    }

    private void Update()
    {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                // タイマーが0になったときの処理をここに追加する（例：ゲームオーバー処理など）
                ResetTimer();
                gameObject.SetActive(false);
            }
    }


    public void ResetTimer()
    {
        currentTime = countdownTime;
    }
}
