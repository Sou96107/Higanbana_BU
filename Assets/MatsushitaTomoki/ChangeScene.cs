using System.Collections;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private void Update()
    {
        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // "TargetSceneName" の部分を遷移したいシーンの名前に置き換える
            string targetSceneName = "SampleScene";

            // Fade オブジェクトがあるか確認し、存在すればフェード処理を実行
            Fade fade = FindObjectOfType<Fade>();
            if (fade != null)
            {
                fade.FadeToScene(targetSceneName);
            }
            else
            {
                // Fade オブジェクトが見つからない場合、直接シーンをロード
                UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}