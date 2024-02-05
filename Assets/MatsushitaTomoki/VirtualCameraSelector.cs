using UnityEngine;
using Cinemachine;

/// <summary>
/// バーチャルカメラを切り替えるサンプル
/// </summary>
public class VirtualCameraSelector : MonoBehaviour
{
    string name = "LButton"; //登録した名前

    // バーチャルカメラ一覧
    [SerializeField] private CinemachineVirtualCameraBase[] _virtualCameraList;

    // 非選択時のバーチャルカメラの優先度
    [SerializeField] private int _unselectedPriority = 0;

    // 選択時のバーチャルカメラの優先度
    [SerializeField] private int _selectedPriority = 10;

    // 選択中のバーチャルカメラのインデックス
    private int _currentCamera = 0;

    // バーチャルカメラの優先度初期化
    private void Awake()
    {
        // バーチャルカメラが設定されていなければ、何もしない
        if (_virtualCameraList == null || _virtualCameraList.Length <= 0)
            return;

        // バーチャルカメラの優先度を初期化
        for (var i = 0; i < _virtualCameraList.Length; ++i)
        {
            _virtualCameraList[i].Priority =
                (i == _currentCamera ? _selectedPriority : _unselectedPriority);
        }
    }

    // フレーム更新
    private void Update()
    {
        // バーチャルカメラが設定されていなければ、何もしない
        if (_virtualCameraList == null || _virtualCameraList.Length <= 0)
            return;

        // Lボタンが押されたら
        if (Input.GetButtonDown(name))
        {
            // 以前のバーチャルカメラを非選択
            var vCamPrev = _virtualCameraList[_currentCamera];
            vCamPrev.Priority = _unselectedPriority;

            // 追従対象を順番に切り替え
            if (++_currentCamera >= _virtualCameraList.Length)
                _currentCamera = 0;

            // 次のバーチャルカメラを選択
            var vCamCurrent = _virtualCameraList[_currentCamera];
            vCamCurrent.Priority = _selectedPriority;
        }
    }
}