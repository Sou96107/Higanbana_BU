using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    private PlayerInput input;  // InputAction設定用
    private float rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        input = new PlayerInput();
        input.Player.Look.started += OnLook;
        input.Player.Look.performed += OnLook;
        input.Player.Look.canceled += OnLook;
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {

        // 回転の更新を毎フレーム行う
        Vector3 eulerAngle = transform.eulerAngles;
        eulerAngle.y += rotationY * Time.deltaTime;


        transform.eulerAngles = eulerAngle;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        var stickMove = context.ReadValue<Vector2>();
        rotationY = stickMove.x * cameraSpeed;
    }
}
