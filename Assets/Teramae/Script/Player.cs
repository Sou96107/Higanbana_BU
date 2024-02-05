using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;
using Cinemachine;
public class Player : MonoBehaviour
{
    public float stepStart = 0.1f;
    //--- インスペクター表示 ---
    [SerializeField, Header("カメラオブジェクト")]
    public GameObject camera;
    [SerializeField, Header("体力")]
    public float hp = 100;
    [SerializeField, Header("攻撃力")]
    public int power = 100;
    [SerializeField, Header("初期弾数")]
    public int bulletNum = 17;
    [SerializeField, Header("無敵時間(ロール)")]
    private float rollInvTime = 1.0f;
    [SerializeField, Header("無敵時間(ステップ)")]
    private float stepInvTime = 0.3f;
    [SerializeField, Header("無敵時間(被弾時)")]
    private float damageInvTime = 0.5f;
    [SerializeField, Header("移動速度")]
    private float moveSpeed = 10.0f;
    //[SerializeField, Header("ダッシュ速度")]
    //private float dashSpeed = 10.0f;
    [SerializeField, Header("ステップ速度倍率")]
    private float stepMltSpd = 2.0f;
    [SerializeField, Header("回避速度倍率")]
    private float rollMltSpd = 2.0f;
    [SerializeField, Header("ジャスト回避の受付時間")]
    private float justAcceptTime = 0.5f;
    [SerializeField, Header("スローモーションの時間")]
    private float SlowTime = 0.5f;
    [SerializeField, Header("ジャスト回避時の透明度(0~255)")]
    private float justAlpha = 150.0f;
    [SerializeField, Header("銃口")]
    private GameObject Gun;
    [SerializeField, Header("弾")]
    private GameObject bullet;
    [SerializeField, Header("プレイヤーの正面")]
    private GameObject playerForward;
    [SerializeField, Header("エネルギーバー")]
    public GameObject EnergyBar;
    [SerializeField, Header("リロードイメージ")]
    public GameObject reloadImg;
    [SerializeField, Header("アタックUI")]
    private GameObject attackUIimg;
    [SerializeField] GameObject attackUIbut;
    [SerializeField] GameObject attackUIback;
    [SerializeField, Header("リロードUI")]
    private GameObject reloadUIimg;
    [SerializeField] GameObject reloadUIbut;
    [SerializeField] GameObject reloadUIback;
    [SerializeField, Header("ロールUI")]
    private GameObject rollUIimg;
    [SerializeField] GameObject rollUIbut;
    [SerializeField] GameObject rollUIback;
    [SerializeField, Header("ロックUI")]
    private GameObject lockUIimg;
    [SerializeField] GameObject lockUIbut;
    [SerializeField] GameObject lockUIback;
    //[SerializeField, Header("サウンドマネージャー")]
    //private GameObject soundManager;
    [SerializeField, Header("ノズルフラッシュ")]
    public GameObject muzzleFlash;
    [SerializeField, Header("マガジンUI_CS")]
    public MagazineUI magazineUI;
    [SerializeField, Header("モデル")]
    public GameObject playerModel;
    [SerializeField, Header("エフェクト再生")]
    public GameObject effectObj;
    [SerializeField, Header("カットイン")]
    public GameObject cutIn;
    [SerializeField, Header("モデル(千束)")]
    private List<GameObject> chisatoModel = new List<GameObject>();
    [SerializeField, Header("マテリアル(千束)")]
    private Material[] chisatoMat;
    [SerializeField, Header("モデル(銃)")]
    private GameObject gunModel;
    [SerializeField, Header("マテリアル(銃)")]
    private Material[] gunMat;
    [SerializeField, Header("ジャスト回避時表示UI")]
    private CanvasGroup justUI;
    [SerializeField, Header("ジャスト回避時表示エフェクト")]
    private GameObject justEffect;

    //--- インスペクター非表示
    //[SerializeField, HideInInspector]
    //public bool isDash;        // ダッシュフラグ
    [SerializeField, HideInInspector]
    public bool isStep;        // ステップフラグ
    [SerializeField, HideInInspector]
    public bool isRoll;         // ロールフラグ
    [SerializeField, HideInInspector]
    public bool isBlow;

    private PlayerInput input;  // InputAction設定用
    private float curInvTime;   // 現在の無敵時間
    private float curJustTime;  // ジャスト回避後の現在時間
    private float curReloadTime;  // リロードの現在時間
    private float curDamageTime;  // 被弾後の現在時間
    private bool isInvRoll;     // 無敵フラグ(回避)
    private bool isInvStep;     // 無敵フラグ(ステップ)
    private bool isInvJust;     // 無敵フラグ(ジャスト回避)
    private bool isKeyBoard;    // キーボードフラグ
    private bool isMove;        // 移動フラグ
    private bool isReload;        // 移動フラグ
    private bool isOnce;        // 一度だけ処理フラグ
    private Rigidbody rb;       // RigidBody設定用
    private Vector2 move;       // 移動量
    private Vector3 velocity;   // 速度ベクトル格納用
    private GameObject enemyObj;// 敵情報格納用
    private bool isEnemy;       // 敵発見フラグ
    private GameObject enemyBlt;// 敵の弾情報格納用
    private bool isEnemyBlt;    // 敵の弾フラグ
    private Vector3 latePos;    // 過去の座標格納用
    Vector3 bulletDir;          // 弾のベクトル格納用
    private bool isJust;        // ジャスト回避フラグ
    private EnergyBar eneBarCS; // エネルギーバースクリプト
    private GameObject[] targets; // ステージ内のすべての敵情報格納用
    private GameObject nearTarget; // 一番近い敵の情報格納用
    private GameObject autoAtkEnemy; // 自動攻撃対象格納用
    private float verInput;     // 前後の入力格納用(キーボード)
    private float horiInput;    // 左右の入力格納用(キーボード)
    private float curShiftTime; // 現在のシフトキー長押し時間
    private int magazineNum;    // マガジン数
    private float reloadTime;   // リロード時間
    private GameObject lockonEnemy; // ロックオン対象格納用
    [SerializeField] private bool isLockon;      // ロックオンフラグ
    private float curSlowTime;  // 現在のスローモーション時間
    private bool isJustSlow;    // スローモーションフラグ
    private bool isJustAccept;  // ジャスト回避受け付けフラグ
    private bool isHitForward;  // 正面方向衝突フラグ
    private Camera mainCamera;  // メインカメラ格納用
    private Animator playerAnim;// アニメーター格納用
    //private float avoidTime;
    //private bool isAvoid;
    private bool isDamage;      // ダメージフラグ
    private bool isMatAlpha;    // マテリアル透過フラグ
    private float curMatChange; // マテリアル入れ替えの現在時間
    private bool isDeath;       // 死亡フラグ
    private bool isShot;        // 弾発射フラグ
    private Color matColor;     // マテリアルの色情報格納用
    private float curShotTime;  // 射撃後の現在時間
    private GameObject soundManager;
    private GameObject particle;
    private Vector2 inputKey;
    private Transform TrBeforAvoidance;
    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        //- InputActionの設定
        // InputAction初期化
        input = new PlayerInput();
        // 各項目の設定
        input.Player.Move.started += OnMove;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        //input.Player.Roll.performed += OnRoll;
        input.Player.Attack.performed += OnAttack;
        //input.Player.Dash.performed += OnDash;
        input.Player.Step.performed += OnStep;
        input.Player.Lockon.performed += OnLockon;
        input.Player.Reload.performed += OnReload;
        // InputAction有効化
        input.Enable();

        //- RigidBody取得
        rb = GetComponent<Rigidbody>();

        //- 変数の初期化
        curInvTime = 0.0f;
        curJustTime = 0.0f;
        isInvRoll = false;
        isInvStep = false;
        isKeyBoard = false;
        isMove = false;
        isEnemy = false;
        //isDash = false;
        isStep = false;
        isJust = false;
        isOnce = false;
        isLockon = false;
        isJustSlow = false;
        isJustAccept = false;
        isHitForward = false;
        //isAvoid = false;
        isDamage = false;
        isMatAlpha = false;
        isDeath = false;
        isShot = false;
        isBlow = false;
        isStart = false;
        move = Vector2.zero;
        velocity = Vector2.zero;
        latePos = transform.position;
        eneBarCS = EnergyBar.GetComponent<EnergyBar>();
        horiInput = 0.0f;
        verInput = 0.0f;
        curShiftTime = 0.0f;
        curReloadTime = 0.0f;
        curSlowTime = 0.0f;
        magazineNum = 3;
        reloadTime = 1.4f;
        //avoidTime = 0.0f;
        curDamageTime = 0.0f;
        curMatChange = 0.0f;
        curShotTime = 0.0f;
        mainCamera = Camera.main;
        playerAnim = playerModel.GetComponent<Animator>();
        matColor = chisatoMat[0].color;
        matColor.a = 1.0f;
        chisatoMat[0].color = matColor;
        soundManager = GameObject.Find("SoundManager");
        particle = GameObject.Find("Particle");
        //- UIの色を変える
        reloadUIimg.GetComponent<PushUI>().Push();
        reloadUIbut.GetComponent<PushUI>().Push();
        reloadUIback.GetComponent<PushUI>().Push();
        //Time.timeScale = 0.0f;
        Debug.Log("キーボード操作：" + isKeyBoard);
    }

    private void OnDestroy()
    {
        //- InputAction終了処理
        input.Disable();
    }

    private void FixedUpdate()
    {
        if (cutIn.activeSelf)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        //- RigidBodyの速度ベクトル取得
        velocity = rb.velocity;
        //- 無敵フラグが無効なら
        if (!isInvRoll && !isInvStep && !isJust && !isDamage && !isDeath && !isShot && !isReload && !isBlow)
        {
            if (move == Vector2.zero) // プレイヤーの移動入力が無かったら
            {
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);//- 速度ベクトル更新
                if (playerAnim.GetBool("isMove"))
                    playerAnim.SetBool("isMove", false);
            }
            else // 移動入力があったら
            {
                rb.velocity = new Vector3(move.x, rb.velocity.y, move.y);//- 速度ベクトル更新
                if (!playerAnim.GetBool("isMove"))
                    playerAnim.SetBool("isMove", true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cutIn.activeSelf)
            return;
        //- 無敵時間処理
        InvCount();

        //- Tabキーでキーボード操作とコントローラー操作変更
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isKeyBoard ^= true;
            Debug.Log("キーボード操作：" + isKeyBoard);
        }

        //if (isDash)
        //{
        //    if (eneBarCS.energy < eneBarCS.SubDash)
        //    {
        //        Debug.Log("スタミナ無い");
        //        moveSpeed = 5.0f;
        //        isDash = false;
        //    }
        //}
        //
        //if (isAvoid)
        //{
        //    if (isStep)
        //    {
        //        avoidTime += Time.deltaTime;
        //        if (avoidTime >= 0.05f && avoidTime <= 0.15)
        //        {
        //            Vector3 forward = transform.position - playerForward.transform.position; // プレイヤーの正面方向のベクトルを計算
        //            rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // 速度ベクトル変更
        //        }
        //        else if (avoidTime > 0.15f)
        //        {
        //            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f); // 速度ベクトル変更
        //            avoidTime = 0.0f;
        //            isAvoid = false;
        //        }
        //    }
        //    else if (isRoll)
        //    {
        //        avoidTime += Time.deltaTime;
        //    }
        //}

        //- リロードフラグが有効なら
        if (isReload)
        {
            //- リロード関数実行
            Reload();
        }

        //- キーボード設定
        if (isKeyBoard)
        {
            KeyboardPlay();
        }

        //- ジャスト回避のスローモーションフラグが有効なら
        if (isJustSlow)
        {
            curJustTime += Time.unscaledDeltaTime; // 現在のジャスト回避時間更新
            //- 現在のジャスト回避時間が総スローモーション時間を超えたら
            if (curJustTime > SlowTime)
            {
                Time.timeScale = 1.0f; // タイムスケールを元に戻す
                curJustTime = 0.0f; // 現在のジャスト回避時間初期化
                isJustSlow = false; // ジャスト回避のスローモーションフラグ無効化
            }
        }

        //- ジャスト回避フラグが有効なら
        if (isJust)
        {
            //- ジャスト回避時の攻撃関数実行
            JustAttack();
        }

        //- ロックオンフラグが有効なら
        if (isLockon)
        {
            if (lockonEnemy && !isMove && !isStep && !isRoll && !isJust && !isReload && !isShot && !isDeath && !isDamage && !isBlow) // ロックオン対象がいたら
            {
                //- ロックオンしている敵の方向に向く
                var direction = lockonEnemy.transform.position - transform.position;
                direction.y = 0.0f;
                var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);
            }
            //else if(!lockonEnemy) // ロックオン対象がいなかったら
            //{
            //    changeCamera_CS.CamChange(); // カメラ変更
            //    isLockon = false; // ロックオンフラグ無効化
            //    Debug.Log("カメラ変更");
            //}
        }
        if (isMove && !isKeyBoard && !isStep && !isRoll && !isJust && !isReload && !isShot && !isDeath && !isDamage && !isBlow) // 移動フラグが有効なら
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputKey.y + Camera.main.transform.right * inputKey.x;
            // 入力方向
            float inputDir = Mathf.Atan2(inputKey.x, inputKey.y) * Mathf.Rad2Deg;

            // カメラ向き
            float cameraDir = camera.transform.eulerAngles.y;

            // ワールドに変換した移動方向
            float moveDir = inputDir + cameraDir;

            // 移動方向を計算
            Vector3 moveDirVector = new Vector3(Mathf.Sin(moveDir * Mathf.Deg2Rad), 0.0f, Mathf.Cos(moveDir * Mathf.Deg2Rad));
            move = new Vector2(moveDirVector.x * moveSpeed, moveDirVector.z * moveSpeed);

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }

            //- プレイヤーの向き変更処理
            //Vector3 diff = transform.position - latePos;    // 現在位置から過去の位置を減算
            //diff.y = 0.0f;  // Y軸要素を無くす
            //latePos = transform.position;   // 過去の位置更新
            //Debug.Log(diff.magnitude);
            //if (!CheckForwardObj())
            //{
            //    //- ベクトルの大きさが0.01以上なら変更
            //    if (diff.magnitude > 0.01f)
            //    {
            //        transform.rotation = Quaternion.LookRotation(diff);
            //    }
            //}
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //- キーボードフラグが無効なら
        if (!isKeyBoard/* && !isStep && !isRoll && !isJust*/)
        {
            if(!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            //- 移動フラグを有効化
            isMove = true;
            //if (!playerAnim.GetBool("isMove"))
            //    playerAnim.SetBool("isMove", true);
            //- 左スティックの入力量を取得
            inputKey = context.ReadValue<Vector2>();
            //// 入力方向
            //float inputDir = Mathf.Atan2(inputKey.x, inputKey.y) * Mathf.Rad2Deg;

            //// カメラ向き
            //float cameraDir = camera.transform.eulerAngles.y;

            //// ワールドに変換した移動方向
            //float moveDir = inputDir + cameraDir;

            //// 移動方向を計算
            //Vector3 moveDirVector = new Vector3(Mathf.Sin(moveDir * Mathf.Deg2Rad), 0.0f, Mathf.Cos(moveDir * Mathf.Deg2Rad));
            //move = new Vector2(moveDirVector.x * moveSpeed, moveDirVector.z * moveSpeed);


        }
        //- 入力が無くなったら
        if (context.phase == InputActionPhase.Canceled)
        {
            isMove = false; // 移動フラグ無効化
            move = Vector2.zero;
            Debug.Log("移動終了");
            if (playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", false);
        }
    }

    //private void OnRoll(InputAction.CallbackContext context)
    //{
    //    if (!isInvStep && !isInvRoll && !isDamage && !isDeath && eneBarCS.energy >= eneBarCS.SubStep)
    //    {
    //        // UIの色を変える
    //        //rollUI.GetComponent<PushUI>().Push();
    //        //- サウンド再生
    //        soundManager.GetComponent<SoundManager>().PlaySE("ロール回避2");

    //        isMove = false;     // 移動フラグ無効化
    //        isRoll = true;      // ロールフラグ有効化
    //        isInvRoll = true;   // 無敵フラグ(回避)有効化
    //        isJustAccept = true;// ジャスト回避受け付けフラグ有効化
    //        Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
    //        rb.velocity = new Vector3(forward.normalized.x * moveSpeed * rollMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * rollMltSpd); // 速度ベクトル変更
    //        Debug.Log("無敵(回避)");
    //    }
    //}

    private void OnAttack(InputAction.CallbackContext context)
    {
        //- 各無敵フラグが無効 かつ 残弾数が残っていれば
        if (!isInvRoll && !isInvStep && !isInvJust && !isDamage && !isDeath && !isReload && !isBlow && bulletNum > 0)
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            isShot = true;
            rb.velocity = Vector3.zero;
            curShotTime = 0.0f;
            //- UIの色を変える
            attackUIimg.GetComponent<PushUI>().ShotPush();
            attackUIbut.GetComponent<PushUI>().ShotPush();
            attackUIback.GetComponent<PushUI>().ShotPush();
            //- サウンド再生
            soundManager.GetComponent<SoundManager>().PlaySE("発砲音");
            //- ノズルフラッシュ再生
            StartCoroutine(EffectDrawTime());
            //- 攻撃アニメーション再生
            playerAnim.Play("Shot", 0, 0.35f);
            //- 弾生成処理
            GameObject Bullet = Instantiate(bullet, new Vector3(Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z), Quaternion.Euler(0, 0, 0));
            bulletNum--;
            if (bulletNum == 16 && magazineNum > 0)
            {
                //- UIの色を変える
                reloadUIimg.GetComponent<PushUI>().Release();
                reloadUIbut.GetComponent<PushUI>().Release();
                reloadUIback.GetComponent<PushUI>().Release();
            }
            if (enemyObj && !lockonEnemy) // 敵がいたら
            {
                //Debug.Log("敵いる");
                //- 弾の発射ベクトルを敵に向かうよう計算
                bulletDir = enemyObj.transform.position - Gun.transform.position;
                //- 計算したベクトルを弾に格納(少しばらけさす)
                Bullet.GetComponent<PlayerBullet>().SetBullet(
                    bulletDir.normalized.x + Random.Range(-0.15f, 0.15f),
                    bulletDir.normalized.y + Random.Range(-0.15f, 0.15f),
                    bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                //playerAnim.SetBool("isShot", false);
            }
            else if (lockonEnemy)
            {
                //- 銃口に向かうベクトル計算
                bulletDir = Gun.transform.position - playerForward.transform.position;
                //- 計算したベクトルを弾に格納(少しばらけさす)
                Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
            }
            else if (!enemyObj) // 敵がいなかったら
            {
                //Debug.Log("敵いない");
                //- 銃口に向かうベクトル計算
                bulletDir = Gun.transform.position - playerForward.transform.position;
                //- 計算したベクトルを弾に格納(少しばらけさす)
                Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                //if (move == Vector2.zero) // プレイヤーが移動してなかったら
                //{
                //    //- 銃口に向かうベクトル計算
                //    bulletDir = Gun.transform.position - playerForward.transform.position;
                //    //- 計算したベクトルを弾に格納(少しばらけさす)
                //    Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                //    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                //    //playerAnim.SetBool("isShot", false);
                //}
                //else // 移動していたら
                //{
                //    //- 移動方向のベクトルを弾に格納(少しばらけさす)
                //    Bullet.GetComponent<PlayerBullet>().SetBullet(move.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), move.normalized.y + Random.Range(-0.15f, 0.15f));
                //    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                //    //playerAnim.SetBool("isShot", false);
                //}
            }
        }
    }

    //private void OnDash(InputAction.CallbackContext context)
    //{
    //    if (eneBarCS.energy >= eneBarCS.SubDash)
    //    {
    //        Debug.Log("ダッシュ");
    //        isDash = true;  // ダッシュフラグ有効化
    //        moveSpeed = dashSpeed; // 移動速度変更
    //    }
    //}

    private void OnStep(InputAction.CallbackContext context)
    {
        //- 各無敵フラグが無効 かつ スタミナが残っていたら
        if (/*!isDash && */!isInvRoll && !isInvStep && !isDamage && !isDeath && !isJust && !isBlow && eneBarCS.energy >= eneBarCS.SubStep)
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            if (isShot)
            {
                if(curShotTime >= stepStart)
                {
                    Debug.Log("ステップ");
                    TrBeforAvoidance = playerModel.transform;    // 現在位置を保存
                    //- UIの色を変える
                    rollUIimg.GetComponent<PushUI>().Push();
                    rollUIbut.GetComponent<PushUI>().Push();
                    rollUIback.GetComponent<PushUI>().Push();
                    //- サウンド再生
                    soundManager.GetComponent<SoundManager>().PlaySE("ステップ回避");
                    //- 回避(ロール)アニメーション再生
                    playerAnim.Play("Roll", 0, 0.15f);

                    particle.SetActive(false);
                    isRoll = true;  // ステップフラグ有効化
                    isInvRoll = true; // ステップ無敵フラグ有効化
                    isJustAccept = true; // ジャスト回避受け付けフラグ有効化
                    isReload = false;
                    curReloadTime = 0.0f;
                    reloadImg.GetComponent<ReloadImage>().SetReload(0.0f, true); // リロードUIの処理実行
                    if (bulletNum != 17 && magazineNum > 0)
                    {
                        //- UIの色を変える
                        reloadUIimg.GetComponent<PushUI>().Release();
                        reloadUIbut.GetComponent<PushUI>().Release();
                        reloadUIback.GetComponent<PushUI>().Release();
                    }
                    Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
                                                                                             //Vector3 forward = transform.position - playerForward.transform.position; // プレイヤーの正面方向のベクトルを計算
                    rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // 速度ベクトル変更
                    Debug.Log("無敵(ステップ)");
                }
            }
            else
            {
                Debug.Log("ステップ");
                //- UIの色を変える
                TrBeforAvoidance = playerModel.transform;    // 現在位置を保存
                rollUIimg.GetComponent<PushUI>().Push();
                rollUIbut.GetComponent<PushUI>().Push();
                rollUIback.GetComponent<PushUI>().Push();
                //- サウンド再生
                soundManager.GetComponent<SoundManager>().PlaySE("ステップ回避");
                //- 回避(ロール)アニメーション再生
                playerAnim.Play("Roll", 0, 0.15f);

                particle.SetActive(false);
                isRoll = true;  // ステップフラグ有効化
                isInvRoll = true; // ステップ無敵フラグ有効化
                isJustAccept = true; // ジャスト回避受け付けフラグ有効化
                isReload = false;
                curReloadTime = 0.0f;
                reloadImg.GetComponent<ReloadImage>().SetReload(0.0f, true); // リロードUIの処理実行
                if (bulletNum != 17 && magazineNum > 0)
                {
                    //- UIの色を変える
                    reloadUIimg.GetComponent<PushUI>().Release();
                    reloadUIbut.GetComponent<PushUI>().Release();
                    reloadUIback.GetComponent<PushUI>().Release();
                }
                Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
                                                                                         //Vector3 forward = transform.position - playerForward.transform.position; // プレイヤーの正面方向のベクトルを計算
                rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // 速度ベクトル変更
                Debug.Log("無敵(ステップ)");
            }
        }
        //else if (isDash) // ダッシュフラグが有効なら
        //{
        //    isDash = false;   // ダッシュフラグ無効化
        //    moveSpeed = 5.0f; // 移動速度変更
        //}
    }

    private void OnLockon(InputAction.CallbackContext context)
    {
        //if (!isJust && !isDeath)
        //{
        //    //isLockon = changeCamera_CS.GetLockonFlag(); // ロックオンフラグを切り替え
        //    Debug.Log("ロックオンフラグ：" + isLockon);
        //    //isLockon ^= true; // ロックオンフラグを切り替え
        //    if (isLockon)
        //    {
        //        //- UIの色を変える
        //        lockUIimg.GetComponent<PushUI>().Push();
        //        lockUIbut.GetComponent<PushUI>().Push();
        //        lockUIback.GetComponent<PushUI>().Push();
        //    }
        //    else
        //    {
        //        //- UIの色を変える
        //        lockUIimg.GetComponent<PushUI>().Release();
        //        lockUIbut.GetComponent<PushUI>().Release();
        //        lockUIback.GetComponent<PushUI>().Release();
        //    }
        //}
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if(!isInvRoll && !isInvStep && !isDamage && !isDeath && !isJust && !isBlow && !isBlow && bulletNum != 17 && magazineNum > 0)
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            // リロードアニメーション
            if (!playerAnim.GetBool("isReroad"))
            {
                playerAnim.SetBool("isReload", true);
                playerAnim.Play("Reload");
            }
            isReload = true;
            rb.velocity = Vector3.zero;
            //- UIの色を変える
            reloadUIimg.GetComponent<PushUI>().Push();
            reloadUIbut.GetComponent<PushUI>().Push();
            reloadUIback.GetComponent<PushUI>().Push();
        }
    }

    /// <summary>
    /// 索敵範囲内の敵格納
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(GameObject enemy)
    {
        //- エネミー情報格納
        enemyObj = enemy;
    }

    /// <summary>
    /// ジャスト回避対象の弾格納
    /// </summary>
    /// <param name="bullet"></param>
    public void SetEnemyBullet(GameObject bullet)
    {
        //- 敵の弾情報格納
        enemyBlt = bullet;
    }

    /// <summary>
    /// 自動攻撃対象の格納
    /// </summary>
    /// <param name="enemy"></param>
    public void SetAutAtkEnemy(GameObject enemy)
    {
        //- 自動攻撃対象の情報格納
        autoAtkEnemy = enemy;
    }

    /// <summary>
    /// ロックオン対象の格納
    /// </summary>
    /// <param name="enemy"></param>
    public void SetLockonEnemy(GameObject enemy, bool lockonFlg)
    {
        //- 自動攻撃対象の情報格納
        lockonEnemy = enemy;
        //- ロックオンフラグ有効化
        isLockon = lockonFlg;
        if (!isLockon)
        {
            //- UIの色を変える
            lockUIimg.GetComponent<PushUI>().Push();
            lockUIbut.GetComponent<PushUI>().Push();
            lockUIback.GetComponent<PushUI>().Push();
        }
        else
        {
            //- UIの色を変える
            lockUIimg.GetComponent<PushUI>().Release();
            lockUIbut.GetComponent<PushUI>().Release();
            lockUIback.GetComponent<PushUI>().Release();
        }
    }

    /// <summary>
    /// マガジン取得
    /// </summary>
    public bool SetMagazine()
    {
        //- マガジン数が3未満なら
        if (magazineNum < 3)
        {
            magazineUI.DrawMagazine(magazineNum); // マガジンUIの表示変更
            magazineNum++; // マガジン数増加
            return true;
        }
        return false;
    }

    /// <summary>
    /// プレイヤーの体力を減らす
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        //- 各無敵フラグが無効なら
        if ((!isInvRoll && !isInvStep && !isInvJust && !isDamage && !isDeath) || isBlow)
        {
            soundManager.GetComponent<SoundManager>().PlaySE("被弾音");// 音再生
            var impulseSource = GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulse();
            hp -= damage; // 体力からダメージ分減らす
            isReload = false;
            curReloadTime = 0.0f;
            reloadImg.GetComponent<ReloadImage>().SetReload(0.0f, true); // リロードUIの処理実行
            if (bulletNum != 17 && magazineNum > 0)
            {
                //- UIの色を変える
                reloadUIimg.GetComponent<PushUI>().Release();
                reloadUIbut.GetComponent<PushUI>().Release();
                reloadUIback.GetComponent<PushUI>().Release();
            }

            //- エフェクト再生
            effectObj.GetComponent<EffectSample>().PlayEffect("hit 1");

            // ダメージアニメーション再生
            playerAnim.Play("Damage", 0, 0.24f);

            //- 体力が0以下になったら
            if (hp <= 0)
            {
                // 死亡アニメーション再生
                playerAnim.SetBool("isDeath", true);
                // 移動量無くす
                rb.velocity = new Vector3(0.0f, velocity.y, 0.0f);
                // 死亡フラグ有効化
                isDeath = true;
                //Destroy(gameObject); // プレイヤーを消す(死亡)
            }
            else
            {
                // ダメージフラグ有効化
                isDamage = true;
            }
        }
    }

    // 吹き飛び
    public void Blow(Vector3 AddV , float damage)
    {
        StartCoroutine(EnumBlow(AddV , damage));
    }

    /// <summary>
    /// 残弾数を取得
    /// </summary>
    public int GetBulletNum()
    {
        return bulletNum;
    }

    /// <summary>
    /// 現在の体力を取得
    /// </summary>
    public float GetHP()
    {
        return hp;
    }

    /// <summary>
    /// マガジン数を取得
    /// </summary>
    public int GetMagazineNum()
    {
        return magazineNum;
    }

    /// <summary>
    /// 攻撃フラグを取得
    /// </summary>
    public bool IsShot()
    {
        return isShot;
    }

    /// <summary>
    /// ジャスト回避フラグを取得
    /// </summary>
    public bool IsJust()
    {
        return isJust;
    }

    private void InvCount()
    {
        //- 回避の無敵フラグが有効なら
        if (isInvRoll)
        {
            //- 無敵の現在時間更新
            curInvTime += Time.deltaTime;
            //- 無敵の現在時間が無敵時間を超えたら
            if (curInvTime > rollInvTime)
            {
                isRoll = false;     // ロールフラグを無効化
                EnergyBar.GetComponent<EnergyBar>().isRoll = false;
                isInvRoll = false;  // 無敵フラグを無効化
                curInvTime = 0.0f;  // 無敵の現在時間初期化
                rollUIimg.GetComponent<PushUI>().Release(); //- UIの色を戻す
                rollUIbut.GetComponent<PushUI>().Release(); //- UIの色を戻す
                rollUIback.GetComponent<PushUI>().Release(); //- UIの色を戻す
                particle.SetActive(true);
                Debug.Log("無敵解除(回避)");
            }
        }
        //- ステップの無敵フラグが有効なら
        if (isInvStep)
        {
            //- 無敵の現在時間更新
            curInvTime += Time.deltaTime;
            //- 無敵の現在時間が無敵時間を超えたら
            if (curInvTime > stepInvTime)
            {
                isStep = false;     // ステップフラグを無効化
                EnergyBar.GetComponent<EnergyBar>().isStep = false;
                isInvStep = false;  // 無敵フラグを無効化
                curInvTime = 0.0f;  // 無敵の現在時間初期化
                rollUIimg.GetComponent<PushUI>().Release(); //- UIの色を戻す
                rollUIbut.GetComponent<PushUI>().Release(); //- UIの色を戻す
                rollUIback.GetComponent<PushUI>().Release(); //- UIの色を戻す

                Debug.Log("無敵解除(ステップ)");
            }
        }
        //- ジャスト回避受け付けフラグが有効なら
        if (isJustAccept)
        {
            curJustTime += Time.deltaTime; // 現在の受け付け時間更新
            if (curJustTime <= justAcceptTime) // 現在の受け付け時間が最大受け付け時間以下なら
            {
                //- 敵の弾が近くに来てたら
                if (lockonEnemy && enemyBlt)
                {
                    if (lockonEnemy.transform.parent.name == enemyBlt.GetComponent<EnemyBullet>().GetEnemyName())
                    {
                        // 残像表示(回避前
                        GetComponent<BakeMesh>().AfterImagef(TrBeforAvoidance);
                        // ジャスト回避アニメーション再生
                        playerAnim.Play("Step", 0, 0.1f);
                        // エフェクト再生
                        GameObject Obj = Instantiate(justEffect, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                        Obj.transform.parent = this.transform;

                        isRoll = false; // 回避(ロール)フラグ無効化
                        isInvRoll = false; // 無敵(ロール)フラグ無効化
                        isStep = true; // 回避(ステップ)フラグ有効化
                        isInvStep = true; // 無敵(ステップ)フラグ有効化
                        curInvTime = 0.0f; // 現在の無敵時間初期化
                        isJust = true; // ジャスト回避フラグ有効化
                        cutIn.SetActive(true);
                        isInvJust = true; // ジャスト回避の無敵フラグ有効化
                        isJustSlow = true; // ジャスト回避のスローモーションフラグ有効化
                        isJustAccept = false; // ジャスト回避受け付けフラグ無効化
                        curJustTime = 0.0f; // 現在の受け付け時間初期化
                        Time.timeScale = 0.5f; // スローモーションにするためタイムスケール変更
                        Debug.Log("ジャスト回避発動");
                        soundManager.GetComponent<SoundManager>().PlaySE("ジャスト回避"); // ジャスト回避SE再生
                        soundManager.GetComponent<SoundManager>().PlaySE("弾回避音"); // ジャスト回避SE再生
                        justUI.DOFade(1.0f, stepInvTime);
                        //- モデル内のマテリアルの透明度を変更
                        matColor.a = justAlpha / 255.0f;   // 値を0~1にする
                        chisatoMat[0].DOColor(matColor, stepInvTime); // マテリアルの透明度をステップの無敵時間分かけて変更
                        camera.GetComponent<Offset>().SetFollowOffset(); //ジャスト回避の演出            
                        //- プレイヤーの向き変更処理
                        Vector3 camRight = camera.transform.right;
                        transform.rotation = Quaternion.LookRotation(-camRight);
                        rb.velocity = new Vector3(camRight.normalized.x * 4, rb.velocity.y, camRight.normalized.z * 4); // 速度ベクトル変更
                    }
                }
                //Debug.Log("ジャスト受け入れ");
            }
            else if (curJustTime > justAcceptTime) // 現在の受け付け時間が最大受け付け時間を超えたら
            {
                curJustTime = 0.0f; // 現在の受け付け時間初期化
                isJustAccept = false; // ジャスト回避受け付けフラグ無効化
            }
        }
        //- ダメージフラグが有効なら
        if (isDamage)
        {
            curDamageTime += Time.deltaTime; // 被弾後の現在時間更新
            curMatChange += Time.deltaTime;  // マテリアル入れ替えの現在時間更新
            if (!isBlow)
            {
                rb.velocity = new Vector3(0.0f, velocity.y, 0.0f); // 移動量無くす
            }
            
            //- マテリアル入れ替えの現在時間が指定時間を超えたら
            if (curMatChange >= damageInvTime / 7.0f)
            {
                isMatAlpha ^= true; // マテリアル透過フラグ切り替え
                curMatChange = 0.0f; // マテリアル入れ替えの現在時間を初期化
                if (isMatAlpha) // マテリアル透過フラグが有効なら
                {
                    //- モデル内のマテリアルを透過マテリアルに変更
                    for (int i = 0; i < chisatoModel.Count; i++)
                    {
                        chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[1];
                    }
                    gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[1];
                }
                else // マテリアル透過フラグが無効なら
                {
                    //- モデル内のマテリアルを通常に変更
                    for (int i = 0; i < chisatoModel.Count; i++)
                    {
                        chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[0];
                    }
                    gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[0];
                }
            }
            //- 被弾後の現在時間がダメージ無敵時間を超えたら
            if (curDamageTime > damageInvTime)
            {
                isDamage = false;       // ダメージフラグ無効化
                curDamageTime = 0.0f;   // 被弾後の現在時間初期化
                curMatChange = 0.0f;    // マテリアル入れ替えの現在時間初期化
                //- モデル内のマテリアルを通常に変更
                for (int i = 0; i < chisatoModel.Count; i++)
                {
                    chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[0];
                }
                gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[0];
            }
        }
        if(isShot)
        {
            curShotTime += Time.deltaTime;
            if(curShotTime >= 0.3f)
            {
                isShot = false;
                curShotTime = 0.0f;
            }
        }
    }

    //private void NearLockOn()
    //{
    //    //- 一度だけ処理フラグが無効なら
    //    if (!isOnce)
    //    {
    //        //- フィールド上にいる全エネミーの情報を取得
    //        targets = GameObject.FindGameObjectsWithTag("Enemy");
    //        float minDistance = float.MaxValue;
    //        nearTarget = null;
    //        //- 一番近い敵を検索
    //        foreach (GameObject t in targets)
    //        {
    //            float distance = Vector3.Distance(transform.position, t.transform.position);

    //            if (distance < minDistance)
    //            {
    //                minDistance = distance;
    //                nearTarget = t;
    //            }
    //        }
    //        isOnce = true; // 一度だけ処理フラグ有効化

    //        playerAnim.SetTrigger("JustStep");
    //    }
    //    //- 一番近い敵の方向に向く
    //    if (nearTarget)
    //    {
    //        var direction = nearTarget.transform.position - transform.position;
    //        direction.y = 0.0f;
    //        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);
    //        //- 一番近い敵に向かって移動
    //        Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
    //        float distance = Vector3.Distance(nearTarget.transform.position, transform.position);
    //        Debug.Log("敵との距離：" + distance);
    //        rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd * 2.0f, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd * 2.0f); // 速度ベクトル変更
    //    }
    //}

    private void JustAttack()
    {
        //- 各回避フラグが無効なら
        if (!isRoll && !isStep)
        {
            //- 近くの敵を標的にする関数実行
            //NearLockOn();
            //- 一度だけ処理フラグが無効なら
            if (!isOnce)
            {
                isOnce = true; // 一度だけ処理フラグ有効化
                playerAnim.SetTrigger("JustStep"); // ジャスト回避のアニメーション実行
                rb.velocity = Vector3.zero; // 移動量を無くす
                Vector3 diff = lockonEnemy.transform.position - transform.position;    // 現在位置から過去の位置を減算
                diff.y = 0.0f;  // Y軸要素を無くす
                transform.rotation = Quaternion.LookRotation(diff);
                //- 一番近い敵に向かって移動
                Vector3 moveForward = lockonEnemy.transform.position - transform.position; // 敵に向かうベクトルを計算
                //- 敵と自分の距離を計算して3分割する(ジャスト回避が3歩のため)
                float magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 3; 
                if (transform.forward.z > 0) // プレイヤーがZ軸の正方向を向いていたら
                {
                    //- ジャスト回避の最初の一歩目の座標を計算
                    Vector3 destination = transform.position + new Vector3(transform.forward.x + 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                    //- 計算した座標に0.1秒かけて移動
                    transform.DOMove(destination, 0.1f).OnComplete(() =>
                    {// 移動し終わったら
                        //- 少し後ろにずれるよう座標を計算
                        destination = transform.position + transform.forward * -1.1f;
                        //- 非透明にする
                        matColor.a = 1.0f; 
                        chisatoMat[0].color = matColor;
                        justUI.DOFade(0.0f, 0.5f);
                        //- 座標取得
                        TrBeforAvoidance = playerModel.transform;
                        //- 計算した座標に0.1秒かけて移動
                        transform.DOMove(destination, 0.1f).OnComplete(() =>
                        {// 移動し終わったら
                            //- 残像表示(回避
                            GetComponent<BakeMesh>().AfterImages(TrBeforAvoidance, 0.15f);
                            //- 少し後ろにずらした事によって最終的な座標がずれるので２歩目の座標を再計算
                            magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 2.0f;
                            destination = transform.position + new Vector3(transform.forward.x - 0.4f, transform.forward.y, transform.forward.z) * magnitude;
                            //- キャラの透明度を変更
                            matColor.a = justAlpha / 255.0f; // 0~1になるよう計算
                            chisatoMat[0].DOColor(matColor, 0.3f); // 0.3秒かけて色変更
                            lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                            soundManager.GetComponent<SoundManager>().PlaySE("弾回避音"); // ジャスト回避SE再生
                            //- 座標取得
                            TrBeforAvoidance = playerModel.transform;
                            //- 0.2秒待ってから、再計算した座標へ0.1秒かけて移動
                            transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                            {// 移動し終わったら
                                //- 残像表示(回避
                                GetComponent<BakeMesh>().AfterImages(TrBeforAvoidance, 0.5f);
                                //- 少し後ろにずれるよう座標を計算
                                destination = transform.position + transform.forward * -1.1f;
                                //- 非透明にする
                                matColor.a = 1.0f;
                                chisatoMat[0].color = matColor;
                                //- 計算した座標に0.1秒かけて移動
                                transform.DOMove(destination, 0.1f).OnComplete(() =>
                                {// 移動し終わったら
                                    //- 少し後ろにずらした事によって最終的な座標がずれるので3歩目の座標を再計算
                                    magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) - Vector3.Distance(lockonEnemy.transform.position, transform.position) / 4;
                                    destination = transform.position + new Vector3(transform.forward.x + 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                                    //- キャラの透明度を変更
                                    matColor.a = justAlpha / 255.0f; // 0~1になるよう計算
                                    chisatoMat[0].DOColor(matColor, 0.3f); // 0.3秒かけて色変更
                                    lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                                    soundManager.GetComponent<SoundManager>().PlaySE("弾回避音"); // ジャスト回避SE再生
                                    //- 0.2秒待ってから、再計算した座標へ0.1秒かけて移動
                                    transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                                    {
                                        //- 残像削除
                                        GetComponent<BakeMesh>().DestroyBakeMesh();
                                        //- 非透明にする
                                        matColor.a = 1.0f;
                                        chisatoMat[0].color = matColor;
                                        Debug.Log("弾発射");
                                        //- UIの色を変える
                                        attackUIimg.GetComponent<PushUI>().ShotPush();
                                        attackUIback.GetComponent<PushUI>().ShotPush();
                                        attackUIbut.GetComponent<PushUI>().ShotPush();
                                        //- サウンド再生
                                        soundManager.GetComponent<SoundManager>().PlaySE("発砲音");
                                        //- ノズルフラッシュ再生
                                        StartCoroutine(EffectDrawTime());
                                        camera.GetComponent<Offset>().ResetFollowOffset(); //ジャスト回避の演出終了
                                        //- 攻撃アニメーション再生
                                        playerAnim.Play("Shot", 0, 0.35f);
                                        //- 弾生成処理
                                        GameObject Bullet = Instantiate(bullet, new Vector3(
                                            Gun.transform.position.x,
                                            Gun.transform.position.y,
                                            Gun.transform.position.z),
                                            Quaternion.Euler(0, 0, 0));
                                        //- 弾の発射ベクトルを敵に向かうよう計算
                                        bulletDir = lockonEnemy.transform.position - Gun.transform.position;
                                        //- 計算したベクトルを弾に格納
                                        Bullet.GetComponent<PlayerBullet>().SetBullet(
                                            bulletDir.normalized.x,
                                            bulletDir.normalized.y,
                                            bulletDir.normalized.z);
                                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(lockonEnemy.transform.parent.position);
                                        Bullet.GetComponent<PlayerBullet>().SetJustBullet(lockonEnemy);
                                        isJust = false; // ジャスト回避フラグ無効化
                                        isInvJust = false; // ジャスト回避の無敵フラグ無効化
                                        isJustAccept = false; // ジャスト回避受け付けフラグ無効化
                                        isInvStep = false;  // ステップの無敵フラグ無効化
                                        isOnce = false; // 一度だけ処理フラグ無効化
                                        curJustTime = 0.0f; // 現在の受け付け時間初期化
                                        move = Vector2.zero; // 移動量初期化
                                        particle.SetActive(true);
                                        Debug.Log("ジャスト攻撃発動");
                                    });
                                });
                            });
                        });
                    });
                }
                else // プレイヤーがZ軸の負方向を向いていたら
                {
                    //- ジャスト回避の最初の一歩目の座標を計算
                    Vector3 destination = transform.position + new Vector3(transform.forward.x - 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                    //- 計算した座標に0.1秒かけて移動
                    transform.DOMove(destination, 0.1f).OnComplete(() =>
                    {// 移動し終わったら
                        //- 少し後ろにずれるよう座標を計算
                        destination = transform.position + transform.forward * -1.1f;
                        //- 非透明にする
                        matColor.a = 1.0f;
                        chisatoMat[0].color = matColor;
                        justUI.DOFade(0.0f, 0.5f);
                        //- 座標取得
                        TrBeforAvoidance = playerModel.transform;
                        //- 計算した座標に0.1秒かけて移動
                        transform.DOMove(destination, 0.1f).OnComplete(() =>
                        {// 移動し終わったら
                            //- 残像表示(回避
                            GetComponent<BakeMesh>().AfterImages(TrBeforAvoidance, 0.15f);
                            //- 少し後ろにずらした事によって最終的な座標がずれるので２歩目の座標を再計算
                            magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 2.0f;
                            destination = transform.position + new Vector3(transform.forward.x + 0.4f, transform.forward.y, transform.forward.z) * magnitude;
                            //- キャラの透明度を変更
                            matColor.a = justAlpha / 255.0f; // 0~1になるよう計算
                            chisatoMat[0].DOColor(matColor, 0.3f); // 0.3秒かけて色変更
                            lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                            soundManager.GetComponent<SoundManager>().PlaySE("弾回避音"); // ジャスト回避SE再生
                            //- 座標取得
                            TrBeforAvoidance = playerModel.transform;
                            //- 0.2秒待ってから、再計算した座標へ0.1秒かけて移動
                            transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                            {// 移動し終わったら
                                //- 残像表示(回避
                                GetComponent<BakeMesh>().AfterImages(TrBeforAvoidance, 0.5f);
                                //- 少し後ろにずれるよう座標を計算
                                destination = transform.position + transform.forward * -1.1f;
                                //- 非透明にする
                                matColor.a = 1.0f;
                                chisatoMat[0].color = matColor;
                                //- 計算した座標に0.1秒かけて移動
                                transform.DOMove(destination, 0.1f).OnComplete(() =>
                                {// 移動し終わったら
                                    //- 少し後ろにずらした事によって最終的な座標がずれるので3歩目の座標を再計算
                                    magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) - Vector3.Distance(lockonEnemy.transform.position, transform.position) / 4;
                                    destination = transform.position + new Vector3(transform.forward.x - 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                                    //- キャラの透明度を変更
                                    matColor.a = justAlpha / 255.0f; // 0~1になるよう計算
                                    chisatoMat[0].DOColor(matColor, 0.3f); // 0.3秒かけて色変更
                                    lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                                    soundManager.GetComponent<SoundManager>().PlaySE("弾回避音"); // ジャスト回避SE再生
                                    //- 0.2秒待ってから、再計算した座標へ0.1秒かけて移動
                                    transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                                    {
                                        //- 残像削除
                                        GetComponent<BakeMesh>().DestroyBakeMesh();
                                        //- 非透明にする
                                        matColor.a = 1.0f;
                                        chisatoMat[0].color = matColor;
                                        Debug.Log("弾発射");
                                        //- UIの色を変える
                                        attackUIimg.GetComponent<PushUI>().ShotPush();
                                        attackUIback.GetComponent<PushUI>().ShotPush();
                                        attackUIbut.GetComponent<PushUI>().ShotPush();
                                        //- サウンド再生
                                        soundManager.GetComponent<SoundManager>().PlaySE("発砲音");
                                        //- ノズルフラッシュ再生
                                        StartCoroutine(EffectDrawTime());
                                        camera.GetComponent<Offset>().ResetFollowOffset(); //ジャスト回避の演出終了
                                        //- 攻撃アニメーション再生
                                        playerAnim.Play("Shot", 0, 0.35f);
                                        //- 弾生成処理
                                        GameObject Bullet = Instantiate(bullet, new Vector3(
                                            Gun.transform.position.x,
                                            Gun.transform.position.y,
                                            Gun.transform.position.z),
                                            Quaternion.Euler(0, 0, 0));
                                        //- 弾の発射ベクトルを敵に向かうよう計算
                                        bulletDir = lockonEnemy.transform.position - Gun.transform.position;
                                        //- 計算したベクトルを弾に格納
                                        Bullet.GetComponent<PlayerBullet>().SetBullet(
                                            bulletDir.normalized.x,
                                            bulletDir.normalized.y,
                                            bulletDir.normalized.z);
                                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(lockonEnemy.transform.parent.position);
                                        Bullet.GetComponent<PlayerBullet>().SetJustBullet(lockonEnemy);
                                        isJust = false; // ジャスト回避フラグ無効化
                                        isInvJust = false; // ジャスト回避の無敵フラグ無効化
                                        isJustAccept = false; // ジャスト回避受け付けフラグ無効化
                                        isInvStep = false;  // ステップの無敵フラグ無効化
                                        isOnce = false; // 一度だけ処理フラグ無効化
                                        curJustTime = 0.0f; // 現在の受け付け時間初期化
                                        move = Vector2.zero; // 移動量初期化
                                        particle.SetActive(true);
                                        Debug.Log("ジャスト攻撃発動");
                                    });
                                });
                            });
                        });
                    });
                }
            }
            //Debug.Log("切り替えフラグ：" + isCutBack);
            //if (CheckForwardObj())
            //{
            //    isJust = false; // ジャスト回避フラグ無効化
            //    isInvJust = false; // ジャスト回避の無敵フラグ無効化
            //    isJustSlow = false; // ジャスト回避のスローモーションフラグ無効化
            //    isJustAccept = false; // ジャスト回避受け付けフラグ無効化
            //    isHitForward = true; // 前方衝突フラグ有効化
            //    isOnce = false; // 一度だけ処理フラグ無効化
            //    Time.timeScale = 1.0f; // タイムスケールを元に戻す
            //}
        }
    }

    private void Reload()
    {
        reloadImg.GetComponent<ReloadImage>().SetReload(reloadTime); // リロードUIの処理実行
        curReloadTime += Time.deltaTime; // 現在のリロード時間更新
        if (curReloadTime > reloadTime) // 現在のリロード時間が総リロード時間を超えたら
        {
            // 音再生
            soundManager.GetComponent<SoundManager>().PlaySE("リロード");
            playerAnim.SetBool("isReload", false);
            bulletNum = 17; // 残弾数増加
            magazineNum--; // マガジン数減少
            magazineUI.HiddenMagazine(magazineNum); // マガジンUIの表示変更
            isReload = false;
            curReloadTime = 0.0f;
            //- UIの色を変える
            reloadUIimg.GetComponent<PushUI>().Push();
            reloadUIbut.GetComponent<PushUI>().Push();
            reloadUIback.GetComponent<PushUI>().Push();
        }
    }

    private bool CheckForwardObj()
    {
        Ray ray = new Ray(new Vector3(transform.position.x + 0.3f, transform.position.y + 0.1f, transform.position.z + 0.5f), transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
        if (Physics.Raycast(ray, out hit, 0.3f))
        {
            if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                return true;
        }
        else
        {
            isHitForward = false;
            ray = new Ray(new Vector3(transform.position.x - 0.3f, transform.position.y + 0.1f, transform.position.z + 0.5f), transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.3f))
            {
                if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x + 0.4f, transform.position.y + 1.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.3f))
            {
                if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x - 0.4f, transform.position.y + 1.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.3f))
            {
                if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x + 0.3f, transform.position.y + 2.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.3f))
            {
                if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x - 0.3f, transform.position.y + 2.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.3f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.3f))
            {
                if (hit.collider.tag != "Player" /*&& hit.collider.tag != "Enemy"*/ && hit.collider.tag != "EnemyBullet")
                    return true;
            }
            else
                isHitForward = false;
        }
        return false;
    }

    private void KeyboardPlay()
    {
        Vector2 keyMove = Vector2.zero;
        //- 移動
        if (Input.GetKey(KeyCode.W))
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            keyMove.y = 1.0f;
            isMove = true;
            //- 移動フラグを有効化
            if (!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            keyMove.y = -1.0f;
            isMove = true;
            //- 移動フラグを有効化
            if (!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            keyMove.x = -1.0f;
            isMove = true;
            //- 移動フラグを有効化
            if (!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isStart)
            {
                isStart = true;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                }
            }
            keyMove.x = 1.0f;
            isMove = true;
            //- 移動フラグを有効化
            if (!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
        }
        if (keyMove == Vector2.zero && !isRoll)
        {
            isMove = false;
            rb.velocity = Vector3.zero;
            move = Vector2.zero;
            //- 移動フラグを有効化
            if (playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", false);
        }
        if (isMove && !isStep && !isRoll && !isJust && !isReload && !isShot && !isDeath && !isDamage && !isBlow)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * keyMove.y + Camera.main.transform.right * keyMove.x;
            // 入力方向
            float inputDir = Mathf.Atan2(keyMove.x, keyMove.y) * Mathf.Rad2Deg;

            // カメラ向き
            float cameraDir = camera.transform.eulerAngles.y;

            // ワールドに変換した移動方向
            float moveDir = inputDir + cameraDir;

            // 移動方向を計算
            Vector3 moveDirVector = new Vector3(Mathf.Sin(moveDir * Mathf.Deg2Rad), 0.0f, Mathf.Cos(moveDir * Mathf.Deg2Rad));
            move = new Vector2(moveDirVector.x * moveSpeed, moveDirVector.z * moveSpeed);
            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }

        //- 攻撃
        if (Input.GetMouseButtonDown(0))
        {
            //- 各無敵フラグが無効 かつ 残弾数が残っていれば
            if (!isInvRoll && !isInvStep && !isInvJust && !isDamage && !isDeath && !isReload && !isBlow && bulletNum > 0)
            {
                if (!isStart)
                {
                    isStart = true;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                    }
                }
                isShot = true;
                rb.velocity = Vector3.zero;
                curShotTime = 0.0f;
                //- UIの色を変える
                attackUIimg.GetComponent<PushUI>().ShotPush();
                attackUIbut.GetComponent<PushUI>().ShotPush();
                attackUIback.GetComponent<PushUI>().ShotPush();
                //- サウンド再生
                soundManager.GetComponent<SoundManager>().PlaySE("発砲音");
                //- ノズルフラッシュ再生
                StartCoroutine(EffectDrawTime());
                //- 攻撃アニメーション再生
                playerAnim.Play("Shot", 0, 0.35f);
                //- 弾生成処理
                GameObject Bullet = Instantiate(bullet, new Vector3(Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z), Quaternion.Euler(0, 0, 0));
                bulletNum--;
                if (bulletNum == 16 && magazineNum > 0)
                {
                    //- UIの色を変える
                    reloadUIimg.GetComponent<PushUI>().Release();
                    reloadUIbut.GetComponent<PushUI>().Release();
                    reloadUIback.GetComponent<PushUI>().Release();
                }
                if (enemyObj && !lockonEnemy) // 敵がいたら
                {
                    //Debug.Log("敵いる");
                    //- 弾の発射ベクトルを敵に向かうよう計算
                    bulletDir = enemyObj.transform.position - Gun.transform.position;
                    //- 計算したベクトルを弾に格納(少しばらけさす)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(
                        bulletDir.normalized.x + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.y + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //playerAnim.SetBool("isShot", false);
                }
                else if (lockonEnemy)
                {
                    //- 銃口に向かうベクトル計算
                    bulletDir = Gun.transform.position - playerForward.transform.position;
                    //- 計算したベクトルを弾に格納(少しばらけさす)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                }
                else if (!enemyObj) // 敵がいなかったら
                {
                    //Debug.Log("敵いない");
                    //- 銃口に向かうベクトル計算
                    bulletDir = Gun.transform.position - playerForward.transform.position;
                    //- 計算したベクトルを弾に格納(少しばらけさす)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //if (move == Vector2.zero) // プレイヤーが移動してなかったら
                    //{
                    //    //- 銃口に向かうベクトル計算
                    //    bulletDir = Gun.transform.position - playerForward.transform.position;
                    //    //- 計算したベクトルを弾に格納(少しばらけさす)
                    //    Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    //    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //    //playerAnim.SetBool("isShot", false);
                    //}
                    //else // 移動していたら
                    //{
                    //    //- 移動方向のベクトルを弾に格納(少しばらけさす)
                    //    Bullet.GetComponent<PlayerBullet>().SetBullet(move.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), move.normalized.y + Random.Range(-0.15f, 0.15f));
                    //    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //    //playerAnim.SetBool("isShot", false);
                    //}
                }
            }
        }

        // 回避
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (!isInvStep && eneBarCS.energy >= eneBarCS.SubStep)
        //    {
        //        isMove = false;     // 移動フラグ無効化
        //        isRoll = true;      // ロールフラグ有効化
        //        isInvRoll = true;   // 無敵フラグ(回避)有効化
        //        isJustAccept = true;// ジャスト回避受け付けフラグ有効化
        //        //- UIの色を変える
        //        //stepUI.GetComponent<PushUI>().Push();
        //        //steptext.GetComponent<PushUI>().PushTextUI();
        //        //- サウンド再生
        //        soundManager.GetComponent<SoundManager>().PlaySE("ステップ回避");
        //        Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
        //        rb.velocity = new Vector3(forward.normalized.x * moveSpeed * rollMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * rollMltSpd); // 速度ベクトル変更
        //        Debug.Log("無敵(回避)");
        //    }
        //}

        //- ダッシュ
        //if(Input.GetKey(KeyCode.LeftShift))
        //{
        //    curShiftTime += Time.deltaTime;
        //    if (curShiftTime > 0.2f)
        //    {
        //        if (eneBarCS.energy >= eneBarCS.SubDash)
        //        {
        //            Debug.Log("ダッシュ");
        //            isDash = true;  // ダッシュフラグ有効化
        //            moveSpeed = dashSpeed; // 移動速度変更
        //        }
        //    }
        //}

        //-　ステップ
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //- 各無敵フラグが無効 かつ スタミナが残っていたら
            if (/*!isDash && */!isInvRoll && !isInvStep && !isDamage && !isDeath && !isJust && !isBlow && eneBarCS.energy >= eneBarCS.SubStep)
            {
                if (!isStart)
                {
                    isStart = true;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                    }
                }
                if (isShot)
                {
                    if (curShotTime >= stepStart)
                    {
                        Debug.Log("ステップ");
                        TrBeforAvoidance = playerModel.transform;    // 現在位置を保存
                                                                     //- UIの色を変える
                        rollUIimg.GetComponent<PushUI>().Push();
                        rollUIbut.GetComponent<PushUI>().Push();
                        rollUIback.GetComponent<PushUI>().Push();
                        //- サウンド再生
                        soundManager.GetComponent<SoundManager>().PlaySE("ステップ回避");
                        //- 回避(ロール)アニメーション再生
                        playerAnim.Play("Roll", 0, 0.15f);

                        particle.SetActive(false);
                        isRoll = true;  // ステップフラグ有効化
                        isInvRoll = true; // ステップ無敵フラグ有効化
                        isJustAccept = true; // ジャスト回避受け付けフラグ有効化
                        isReload = false;
                        curReloadTime = 0.0f;
                        reloadImg.GetComponent<ReloadImage>().SetReload(0.0f, true); // リロードUIの処理実行
                        if (bulletNum != 17 && magazineNum > 0)
                        {
                            //- UIの色を変える
                            reloadUIimg.GetComponent<PushUI>().Release();
                            reloadUIbut.GetComponent<PushUI>().Release();
                            reloadUIback.GetComponent<PushUI>().Release();
                        }
                        Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
                                                                                                 //Vector3 forward = transform.position - playerForward.transform.position; // プレイヤーの正面方向のベクトルを計算
                        rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // 速度ベクトル変更
                        Debug.Log("無敵(ステップ)");
                    }
                }
                else
                {
                    Debug.Log("ステップ");
                    //- UIの色を変える
                    TrBeforAvoidance = playerModel.transform;    // 現在位置を保存
                    rollUIimg.GetComponent<PushUI>().Push();
                    rollUIbut.GetComponent<PushUI>().Push();
                    rollUIback.GetComponent<PushUI>().Push();
                    //- サウンド再生
                    soundManager.GetComponent<SoundManager>().PlaySE("ステップ回避");
                    //- 回避(ロール)アニメーション再生
                    playerAnim.Play("Roll", 0, 0.15f);

                    particle.SetActive(false);
                    isRoll = true;  // ステップフラグ有効化
                    isInvRoll = true; // ステップ無敵フラグ有効化
                    isJustAccept = true; // ジャスト回避受け付けフラグ有効化
                    isReload = false;
                    curReloadTime = 0.0f;
                    reloadImg.GetComponent<ReloadImage>().SetReload(0.0f, true); // リロードUIの処理実行
                    if (bulletNum != 17 && magazineNum > 0)
                    {
                        //- UIの色を変える
                        reloadUIimg.GetComponent<PushUI>().Release();
                        reloadUIbut.GetComponent<PushUI>().Release();
                        reloadUIback.GetComponent<PushUI>().Release();
                    }
                    Vector3 forward = playerForward.transform.position - transform.position; // プレイヤーの正面方向のベクトルを計算
                                                                                             //Vector3 forward = transform.position - playerForward.transform.position; // プレイヤーの正面方向のベクトルを計算
                    rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // 速度ベクトル変更
                    Debug.Log("無敵(ステップ)");
                }
            }
        }

        //- ロックオン
        //if (Input.GetMouseButtonDown(2))
        //{
        //    if (!isJust && !isDeath)
        //    {
        //        //isLockon = changeCamera_CS.GetLockonFlag(); // ロックオンフラグを切り替え
        //        Debug.Log("ロックオンフラグ：" + isLockon);
        //        //isLockon ^= true; // ロックオンフラグを切り替え
        //        if (isLockon)
        //        {
        //            //- UIの色を変える
        //            lockUIimg.GetComponent<PushUI>().Push();
        //            lockUIbut.GetComponent<PushUI>().Push();
        //            lockUIback.GetComponent<PushUI>().Push();
        //        }
        //        else
        //        {
        //            //- UIの色を変える
        //            lockUIimg.GetComponent<PushUI>().Release();
        //            lockUIbut.GetComponent<PushUI>().Release();
        //            lockUIback.GetComponent<PushUI>().Release();
        //        }
        //    }
        //}
        //- リロード
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isInvRoll && !isInvStep && !isDamage && !isDeath && !isJust && !isBlow && !isBlow && bulletNum != 17 && magazineNum > 0)
            {
                if (!isStart)
                {
                    isStart = true;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.transform.GetChild(3).GetComponent<SearchPlayer>().IsStart();
                    }
                }
                // リロードアニメーション
                if (!playerAnim.GetBool("isReroad"))
                {
                    playerAnim.SetBool("isReload", true);
                    playerAnim.Play("Reload");
                }
                isReload = true;
                rb.velocity = Vector3.zero;
                //- UIの色を変える
                reloadUIimg.GetComponent<PushUI>().Push();
                reloadUIbut.GetComponent<PushUI>().Push();
                reloadUIback.GetComponent<PushUI>().Push();
            }
        }
    }

    IEnumerator EffectDrawTime()
    {
        // エフェクトの表示、非表示
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator EnumBlow(Vector3 AddV, float damage)
    {
        isBlow = true;
        // ダメージアニメーション再生
        Damage((int)damage);
        Vector3 v = new Vector3(-transform.forward.x, 0.0f, -transform.forward.z).normalized;
        Rigidbody Rig = GetComponent<Rigidbody>();
        Rig.AddForce(v * 10.0f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        isBlow = false;
        Rig.velocity = Vector3.zero;
        Debug.Log(Rig.velocity);
    }
}
