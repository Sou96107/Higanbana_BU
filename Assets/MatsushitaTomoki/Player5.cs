using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using DG.Tweening;
public class Player5 : MonoBehaviour
{
    //--- �C���X�y�N�^�[�\�� ---
    [Header("----- PlayerMove.cs -----")]
    public CharacterController characterController;

    [SerializeField, Header("�J�����I�u�W�F�N�g")]
    public GameObject camera;
    [SerializeField, Header("�̗�")]
    public float hp = 100;
    [SerializeField, Header("�U����")]
    public int power = 100;
    [SerializeField, Header("�����e��")]
    public int bulletNum = 17;
    [SerializeField, Header("���G����(���[��)")]
    private float rollInvTime = 1.0f;
    [SerializeField, Header("���G����(�X�e�b�v)")]
    private float stepInvTime = 0.3f;
    [SerializeField, Header("���G����(��e��)")]
    private float damageInvTime = 0.5f;
    [SerializeField, Header("�ړ����x")]
    private float moveSpeed = 10.0f;
    //[SerializeField, Header("�_�b�V�����x")]
    //private float dashSpeed = 10.0f;
    [SerializeField, Header("�X�e�b�v���x�{��")]
    private float stepMltSpd = 2.0f;
    [SerializeField, Header("��𑬓x�{��")]
    private float rollMltSpd = 2.0f;
    [SerializeField, Header("�W���X�g����̎�t����")]
    private float justAcceptTime = 0.5f;
    [SerializeField, Header("�X���[���[�V�����̎���")]
    private float SlowTime = 0.5f;
    [SerializeField, Header("�W���X�g������̓����x(0~255)")]
    private float justAlpha = 150.0f;
    [SerializeField, Header("�e��")]
    private GameObject Gun;
    [SerializeField, Header("�e")]
    private GameObject bullet;
    [SerializeField, Header("�v���C���[�̐���")]
    private GameObject playerForward;
    [SerializeField, Header("�G�l���M�[�o�[")]
    public GameObject EnergyBar;
    [SerializeField, Header("�����[�h�C���[�W")]
    public GameObject reloadImg;
    [SerializeField, Header("�A�^�b�NUI")]
    private GameObject attackUIimg;
    [SerializeField] GameObject attackUIbut;
    [SerializeField] GameObject attackUIback;
    [SerializeField, Header("���[��UI")]
    private GameObject rollUIimg;
    [SerializeField] GameObject rollUIbut;
    [SerializeField] GameObject rollUIback;
    [SerializeField, Header("���b�NUI")]
    private GameObject lockUIimg;
    [SerializeField] GameObject lockUIbut;
    [SerializeField] GameObject lockUIback;
    [SerializeField, Header("�T�E���h�}�l�[�W���[")]
    private GameObject soundManager;
    [SerializeField, Header("�m�Y���t���b�V��")]
    public GameObject muzzleFlash;
    [SerializeField, Header("�}�K�W��UI_CS")]
    public MagazineUI magazineUI;
    [SerializeField, Header("���f��")]
    public GameObject playerModel;
    [SerializeField, Header("�G�t�F�N�g�Đ�")]
    public GameObject effectObj;
    [SerializeField, Header("���f��(�瑩)")]
    private List<GameObject> chisatoModel = new List<GameObject>();
    [SerializeField, Header("�}�e���A��(�瑩)")]
    private Material[] chisatoMat;
    [SerializeField, Header("���f��(�e)")]
    private GameObject gunModel;
    [SerializeField, Header("�}�e���A��(�e)")]
    private Material[] gunMat;
    [SerializeField, Header("�J�����؂�ւ�CS")]
    private ChangeCamera changeCamera_CS;
    [SerializeField, Header("�W���X�g������\��UI")]
    private CanvasGroup justUI;


    //--- �C���X�y�N�^�[��\��
    //[SerializeField, HideInInspector]
    //public bool isDash;        // �_�b�V���t���O
    [SerializeField, HideInInspector]
    public bool isStep;        // �X�e�b�v�t���O
    [SerializeField, HideInInspector]
    public bool isRoll;         // ���[���t���O

    private PlayerInput input;  // InputAction�ݒ�p
    private float curInvTime;   // ���݂̖��G����
    private float curJustTime;  // �W���X�g�����̌��ݎ���
    private float curReloadTime;  // �����[�h�̌��ݎ���
    private float curDamageTime;  // ��e��̌��ݎ���
    private bool isInvRoll;     // ���G�t���O(���)
    private bool isInvStep;     // ���G�t���O(�X�e�b�v)
    private bool isInvJust;     // ���G�t���O(�W���X�g���)
    private bool isKeyBoard;    // �L�[�{�[�h�t���O
    private bool isMove;        // �ړ��t���O
    private bool isReload;        // �ړ��t���O
    private bool isOnce;        // ��x���������t���O
    private Rigidbody rb;       // RigidBody�ݒ�p
    private Vector2 move;       // �ړ���
    private Vector3 velocity;   // ���x�x�N�g���i�[�p
    private GameObject enemyObj;// �G���i�[�p
    private bool isEnemy;       // �G�����t���O
    private GameObject enemyBlt;// �G�̒e���i�[�p
    private bool isEnemyBlt;    // �G�̒e�t���O
    private Vector3 latePos;    // �ߋ��̍��W�i�[�p
    Vector3 bulletDir;          // �e�̃x�N�g���i�[�p
    private bool isJust;        // �W���X�g����t���O
    private EnergyBar eneBarCS; // �G�l���M�[�o�[�X�N���v�g
    private GameObject[] targets; // �X�e�[�W���̂��ׂĂ̓G���i�[�p
    private GameObject nearTarget; // ��ԋ߂��G�̏��i�[�p
    private GameObject autoAtkEnemy; // �����U���Ώۊi�[�p
    private float verInput;     // �O��̓��͊i�[�p(�L�[�{�[�h)
    private float horiInput;    // ���E�̓��͊i�[�p(�L�[�{�[�h)
    private float curShiftTime; // ���݂̃V�t�g�L�[����������
    private int magazineNum;    // �}�K�W����
    private float reloadTime;   // �����[�h����
    private GameObject lockonEnemy; // ���b�N�I���Ώۊi�[�p
    [SerializeField] private bool isLockon;      // ���b�N�I���t���O
    private float curSlowTime;  // ���݂̃X���[���[�V��������
    private bool isJustSlow;    // �X���[���[�V�����t���O
    private bool isJustAccept;  // �W���X�g����󂯕t���t���O
    private bool isHitForward;  // ���ʕ����Փ˃t���O
    private Camera mainCamera;  // ���C���J�����i�[�p
    private Animator playerAnim;// �A�j���[�^�[�i�[�p
    //private float avoidTime;
    //private bool isAvoid;
    private bool isDamage;      // �_���[�W�t���O
    private bool isMatAlpha;    // �}�e���A�����߃t���O
    private float curMatChange; // �}�e���A������ւ��̌��ݎ���
    private bool isDeath;       // ���S�t���O
    private bool isShot;        // �e���˃t���O
    private Color matColor;        // �}�e���A���̐F���i�[�p
    private Color UIColor;        // �}�e���A���̐F���i�[�p

    // Start is called before the first frame update
    void Start()
    {
        //- InputAction�̐ݒ�
        // InputAction������
        input = new PlayerInput();
        // �e���ڂ̐ݒ�
        input.Player.Move.started += OnMove;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        //input.Player.Roll.performed += OnRoll;
        input.Player.Attack.performed += OnAttack;
        //input.Player.Dash.performed += OnDash;
        input.Player.Step.performed += OnStep;
        input.Player.Lockon.performed += OnLockon;
        // InputAction�L����
        input.Enable();

        //- RigidBody�擾
        rb = GetComponent<Rigidbody>();

        //- �ϐ��̏�����
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
        reloadTime = 1.0f;
        //avoidTime = 0.0f;
        curDamageTime = 0.0f;
        curMatChange = 0.0f;
        mainCamera = Camera.main;
        playerAnim = playerModel.GetComponent<Animator>();
        matColor = chisatoMat[0].color;
        matColor.a = 1.0f;
        chisatoMat[0].color = matColor;
        //for (int i = 0; i < chisatoModel.Count; i++)
        //{
        //    color[i] = chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material.color;
        //}
        Debug.Log("�L�[�{�[�h����F" + isKeyBoard);
    }

    private void OnDestroy()
    {
        //- InputAction�I������
        input.Dispose();
    }

    private void FixedUpdate()
    {
        //- RigidBody�̑��x�x�N�g���擾
        velocity = rb.velocity;
        //- ���G�t���O�������Ȃ�
        if (!isInvRoll && !isInvStep && !isJust && !isDamage && !isDeath && isMove/* && !isInvJust*/)
        {

            if (move == Vector2.zero) // �v���C���[�̈ړ����͂�����������
            {
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);//- ���x�x�N�g���X�V
            }
            else // �ړ����͂���������
                rb.velocity = new Vector3(move.x, rb.velocity.y, move.y);//- ���x�x�N�g���X�V
        }
    }

    // Update is called once per frame
    void Update()
    {
        //- ���G���ԏ���
        InvCount();

        isShot = false;

        //- Tab�L�[�ŃL�[�{�[�h����ƃR���g���[���[����ύX
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isKeyBoard ^= true;
            Debug.Log("�L�[�{�[�h����F" + isKeyBoard);
        }

        //if (isDash)
        //{
        //    if (eneBarCS.energy < eneBarCS.SubDash)
        //    {
        //        Debug.Log("�X�^�~�i����");
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
        //            Vector3 forward = transform.position - playerForward.transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
        //            rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // ���x�x�N�g���ύX
        //        }
        //        else if (avoidTime > 0.15f)
        //        {
        //            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f); // ���x�x�N�g���ύX
        //            avoidTime = 0.0f;
        //            isAvoid = false;
        //        }
        //    }
        //    else if (isRoll)
        //    {
        //        avoidTime += Time.deltaTime;
        //    }
        //}

        //- �����[�h�t���O���L���Ȃ�
        if (isReload)
        {
            //- �����[�h�֐����s
            Reload();
        }

        //- �L�[�{�[�h�ݒ�
        if (isKeyBoard)
        {
            KeyboardPlay();
        }

        //- �W���X�g����̃X���[���[�V�����t���O���L���Ȃ�
        if (isJustSlow)
        {
            curJustTime += Time.unscaledDeltaTime; // ���݂̃W���X�g������ԍX�V
            //- ���݂̃W���X�g������Ԃ����X���[���[�V�������Ԃ𒴂�����
            if (curJustTime > SlowTime)
            {
                Time.timeScale = 1.0f; // �^�C���X�P�[�������ɖ߂�
                curJustTime = 0.0f; // ���݂̃W���X�g������ԏ�����
                isJustSlow = false; // �W���X�g����̃X���[���[�V�����t���O������
            }
        }

        //- �W���X�g����t���O���L���Ȃ�
        if (isJust)
        {
            //- �W���X�g������̍U���֐����s
            JustAttack();
        }

        //- ���b�N�I���t���O���L���Ȃ�
        if (isLockon)
        {
            if (lockonEnemy && !isMove && !isStep) // ���b�N�I���Ώۂ�������
            {
                //- ���b�N�I�����Ă���G�̕����Ɍ���
                var direction = lockonEnemy.transform.position - transform.position;
                direction.y = 0.0f;
                var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);
            }
            //else if(!lockonEnemy) // ���b�N�I���Ώۂ����Ȃ�������
            //{
            //    changeCamera_CS.CamChange(); // �J�����ύX
            //    isLockon = false; // ���b�N�I���t���O������
            //    Debug.Log("�J�����ύX");
            //}
        }
        if (isMove && !isStep && !isRoll && !isJust) // �ړ��t���O���L���Ȃ�
        {
            //- �v���C���[�̌����ύX����
            Vector3 diff = transform.position - latePos;    // ���݈ʒu����ߋ��̈ʒu�����Z
            diff.y = 0.0f;  // Y���v�f�𖳂���
            latePos = transform.position;   // �ߋ��̈ʒu�X�V
            //- �x�N�g���̑傫����0.01�ȏ�Ȃ�ύX
            if (diff.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(diff);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //- �L�[�{�[�h�t���O�������Ȃ�
        if (!isKeyBoard)
        {
            //- �ړ��t���O��L����
            isMove = true;
            if (!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
            //- ���X�e�B�b�N�̓��͗ʂ��擾
            var inputKey = context.ReadValue<Vector2>();
            Debug.Log(inputKey);
            float deltaTime = Time.deltaTime;
            // ���͕���
            float inputDir = Mathf.Atan2(inputKey.x, inputKey.y) * Mathf.Rad2Deg;

            // �J��������
            float cameraDir = camera.transform.eulerAngles.y;

            // ���[���h�ɕϊ������ړ�����
            float moveDir = inputDir + cameraDir;

            // �⊮��]
            Vector3 nowEulerAngle = camera.transform.eulerAngles;
            //transform.eulerAngles = nowEulerAngle;

            // �ړ��������v�Z
            Vector3 moveDirVector = new Vector3(Mathf.Sin(moveDir * Mathf.Deg2Rad), 0.0f, Mathf.Cos(moveDir * Mathf.Deg2Rad));

            move = new Vector2(moveDirVector.x * moveSpeed * deltaTime, moveDirVector.z * moveSpeed * deltaTime);
            //- ���͂������Ȃ�����
            if (context.phase == InputActionPhase.Canceled)
            {
                rb.velocity = Vector3.zero;
                isMove = false; // �ړ��t���O������
                move = Vector2.zero;
                Debug.Log("�ړ��I��");
                if (playerAnim.GetBool("isMove"))
                    playerAnim.SetBool("isMove", false);
            }


        }
    }

    //private void OnRoll(InputAction.CallbackContext context)
    //{
    //    if (!isInvStep && !isInvRoll && !isDamage && !isDeath && eneBarCS.energy >= eneBarCS.SubStep)
    //    {
    //        // UI�̐F��ς���
    //        //rollUI.GetComponent<PushUI>().Push();
    //        //- �T�E���h�Đ�
    //        soundManager.GetComponent<SoundManager>().PlaySE("���[�����2");

    //        isMove = false;     // �ړ��t���O������
    //        isRoll = true;      // ���[���t���O�L����
    //        isInvRoll = true;   // ���G�t���O(���)�L����
    //        isJustAccept = true;// �W���X�g����󂯕t���t���O�L����
    //        Vector3 forward = playerForward.transform.position - transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
    //        rb.velocity = new Vector3(forward.normalized.x * moveSpeed * rollMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * rollMltSpd); // ���x�x�N�g���ύX
    //        Debug.Log("���G(���)");
    //    }
    //}

    private void OnAttack(InputAction.CallbackContext context)
    {
        //- �e���G�t���O������ ���� �c�e�����c���Ă����
        if (!isInvRoll && !isInvStep && !isInvJust && !isDamage && !isDeath && bulletNum > 0)
        {
            isShot = true;
            //- UI�̐F��ς���
            attackUIimg.GetComponent<PushUI>().ShotPush();
            attackUIbut.GetComponent<PushUI>().ShotPush();
            attackUIback.GetComponent<PushUI>().ShotPush();
            //- �T�E���h�Đ�
            soundManager.GetComponent<SoundManager>().PlaySE("���C��");
            //- �m�Y���t���b�V���Đ�
            StartCoroutine(EffectDrawTime());
            //- �U���A�j���[�V�����Đ�
            playerAnim.Play("Shot", 0, 0.35f);

            //- �e��������
            GameObject Bullet = Instantiate(bullet, new Vector3(Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z), Quaternion.Euler(0, 0, 0));
            bulletNum--;
            if (enemyObj) // �G��������
            {
                //Debug.Log("�G����");
                //- �e�̔��˃x�N�g����G�Ɍ������悤�v�Z
                bulletDir = enemyObj.transform.position - Gun.transform.position;
                //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                Bullet.GetComponent<PlayerBullet>().SetBullet(
                    bulletDir.normalized.x + Random.Range(-0.15f, 0.15f),
                    bulletDir.normalized.y + Random.Range(-0.15f, 0.15f),
                    bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                //playerAnim.SetBool("isShot", false);
            }
            else if (lockonEnemy)
            {
                //- �e���Ɍ������x�N�g���v�Z
                bulletDir = Gun.transform.position - playerForward.transform.position;
                //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
            }
            else if (!enemyObj) // �G�����Ȃ�������
            {
                //Debug.Log("�G���Ȃ�");
                if (move == Vector2.zero) // �v���C���[���ړ����ĂȂ�������
                {
                    //- �e���Ɍ������x�N�g���v�Z
                    bulletDir = Gun.transform.position - playerForward.transform.position;
                    //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //playerAnim.SetBool("isShot", false);
                }
                else // �ړ����Ă�����
                {
                    //- �ړ������̃x�N�g����e�Ɋi�[(�����΂炯����)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(move.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), move.normalized.y + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    //playerAnim.SetBool("isShot", false);
                }
            }
            //- �c�e���������Ȃ��
            if (bulletNum <= 0)
                isReload = true; // �����[�h�t���O�L����
        }
    }

    //private void OnDash(InputAction.CallbackContext context)
    //{
    //    if (eneBarCS.energy >= eneBarCS.SubDash)
    //    {
    //        Debug.Log("�_�b�V��");
    //        isDash = true;  // �_�b�V���t���O�L����
    //        moveSpeed = dashSpeed; // �ړ����x�ύX
    //    }
    //}

    private void OnStep(InputAction.CallbackContext context)
    {
        //- �e���G�t���O������ ���� �X�^�~�i���c���Ă�����
        if (/*!isDash && */!isInvRoll && !isInvStep && !isDamage && !isDeath && eneBarCS.energy >= eneBarCS.SubStep)
        {
            Debug.Log("�X�e�b�v");
            //- UI�̐F��ς���
            rollUIimg.GetComponent<PushUI>().Push();
            rollUIbut.GetComponent<PushUI>().Push();
            rollUIback.GetComponent<PushUI>().Push();
            //- �T�E���h�Đ�
            soundManager.GetComponent<SoundManager>().PlaySE("�X�e�b�v���");
            //- ���(���[��)�A�j���[�V�����Đ�
            playerAnim.Play("Roll", 0, 0.15f);

            isRoll = true;  // �X�e�b�v�t���O�L����
            isInvRoll = true; // �X�e�b�v���G�t���O�L����
            isJustAccept = true; // �W���X�g����󂯕t���t���O�L����
            Vector3 forward = playerForward.transform.position - transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
            //Vector3 forward = transform.position - playerForward.transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
            rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // ���x�x�N�g���ύX
            Debug.Log("���G(�X�e�b�v)");
        }
        //else if (isDash) // �_�b�V���t���O���L���Ȃ�
        //{
        //    isDash = false;   // �_�b�V���t���O������
        //    moveSpeed = 5.0f; // �ړ����x�ύX
        //}
    }

    private void OnLockon(InputAction.CallbackContext context)
    {
        //isLockon = changeCamera_CS.GetLockonFlag(); // ���b�N�I���t���O��؂�ւ�
        Debug.Log("���b�N�I���t���O�F" + isLockon);
        //isLockon ^= true; // ���b�N�I���t���O��؂�ւ�
        if (isLockon)
        {
            //- UI�̐F��ς���
            lockUIimg.GetComponent<PushUI>().Push();
            lockUIbut.GetComponent<PushUI>().Push();
            lockUIback.GetComponent<PushUI>().Push();
        }
        else
        {
            //- UI�̐F��ς���
            lockUIimg.GetComponent<PushUI>().Release();
            lockUIbut.GetComponent<PushUI>().Release();
            lockUIback.GetComponent<PushUI>().Release();
        }
    }

    /// <summary>
    /// ���G�͈͓��̓G�i�[
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(GameObject enemy)
    {
        //- �G�l�~�[���i�[
        enemyObj = enemy;
    }

    /// <summary>
    /// �W���X�g���Ώۂ̒e�i�[
    /// </summary>
    /// <param name="bullet"></param>
    public void SetEnemyBullet(GameObject bullet)
    {
        //- �G�̒e���i�[
        enemyBlt = bullet;
    }

    /// <summary>
    /// �����U���Ώۂ̊i�[
    /// </summary>
    /// <param name="enemy"></param>
    public void SetAutAtkEnemy(GameObject enemy)
    {
        //- �����U���Ώۂ̏��i�[
        autoAtkEnemy = enemy;
    }

    /// <summary>
    /// ���b�N�I���Ώۂ̊i�[
    /// </summary>
    /// <param name="enemy"></param>
    public void SetLockonEnemy(GameObject enemy, bool lockonFlg)
    {
        //- �����U���Ώۂ̏��i�[
        lockonEnemy = enemy;
        //- ���b�N�I���t���O�L����
        isLockon = lockonFlg;
    }

    /// <summary>
    /// �}�K�W���擾
    /// </summary>
    public bool SetMagazine()
    {
        //- �}�K�W������3�����Ȃ�
        if (magazineNum < 3)
        {
            magazineUI.DrawMagazine(magazineNum); // �}�K�W��UI�̕\���ύX
            magazineNum++; // �}�K�W��������
            return true;
        }
        return false;
    }

    /// <summary>
    /// �v���C���[�̗̑͂����炷
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        //- �e���G�t���O�������Ȃ�
        if (!isInvRoll && !isInvStep && !isInvJust && !isDamage && !isDeath)
        {
            soundManager.GetComponent<SoundManager>().PlaySE("��e��");// ���Đ�
            hp -= damage; // �̗͂���_���[�W�����炷
                          //- �̗͂�0�ȉ��ɂȂ�����

            //- �G�t�F�N�g�Đ�
            effectObj.GetComponent<EffectSample>().PlayEffect("hit 1");

            // �_���[�W�A�j���[�V�����Đ�
            playerAnim.Play("Damage", 0, 0.24f);

            //- �̗͂�0�ȉ��ɂȂ�����
            if (hp <= 0)
            {
                // ���S�A�j���[�V�����Đ�
                playerAnim.SetBool("isDeath", true);
                // �ړ��ʖ�����
                rb.velocity = new Vector3(0.0f, velocity.y, 0.0f);
                // ���S�t���O�L����
                isDeath = true;
                //Destroy(gameObject); // �v���C���[������(���S)
            }
            else
            {
                // �_���[�W�t���O�L����
                isDamage = true;
            }
        }
    }

    /// <summary>
    /// �c�e�����擾
    /// </summary>
    public int GetBulletNum()
    {
        return bulletNum;
    }

    /// <summary>
    /// ���݂̗̑͂��擾
    /// </summary>
    public float GetHP()
    {
        return hp;
    }

    /// <summary>
    /// �}�K�W�������擾
    /// </summary>
    public int GetMagazineNum()
    {
        return magazineNum;
    }

    /// <summary>
    /// �U���t���O���擾
    /// </summary>
    public bool IsShot()
    {
        return isShot;
    }

    private void InvCount()
    {
        //- ����̖��G�t���O���L���Ȃ�
        if (isInvRoll)
        {
            //- ���G�̌��ݎ��ԍX�V
            curInvTime += Time.deltaTime;
            //- ���G�̌��ݎ��Ԃ����G���Ԃ𒴂�����
            if (curInvTime > rollInvTime)
            {
                isRoll = false;     // ���[���t���O�𖳌���
                EnergyBar.GetComponent<EnergyBar>().isRoll = false;
                isInvRoll = false;  // ���G�t���O�𖳌���
                curInvTime = 0.0f;  // ���G�̌��ݎ��ԏ�����
                rollUIimg.GetComponent<PushUI>().Release(); //- UI�̐F��߂�
                rollUIbut.GetComponent<PushUI>().Release(); //- UI�̐F��߂�
                rollUIback.GetComponent<PushUI>().Release(); //- UI�̐F��߂�
                Debug.Log("���G����(���)");
            }
        }
        //- �X�e�b�v�̖��G�t���O���L���Ȃ�
        if (isInvStep)
        {
            //- ���G�̌��ݎ��ԍX�V
            curInvTime += Time.deltaTime;
            //- ���G�̌��ݎ��Ԃ����G���Ԃ𒴂�����
            if (curInvTime > stepInvTime)
            {
                isStep = false;     // �X�e�b�v�t���O�𖳌���
                EnergyBar.GetComponent<EnergyBar>().isStep = false;
                isInvStep = false;  // ���G�t���O�𖳌���
                curInvTime = 0.0f;  // ���G�̌��ݎ��ԏ�����
                rollUIimg.GetComponent<PushUI>().Release(); //- UI�̐F��߂�
                rollUIbut.GetComponent<PushUI>().Release(); //- UI�̐F��߂�
                rollUIback.GetComponent<PushUI>().Release(); //- UI�̐F��߂�

                Debug.Log("���G����(�X�e�b�v)");
            }
        }
        //- �W���X�g����󂯕t���t���O���L���Ȃ�
        if (isJustAccept)
        {
            curJustTime += Time.deltaTime; // ���݂̎󂯕t�����ԍX�V
            if (curJustTime <= justAcceptTime) // ���݂̎󂯕t�����Ԃ��ő�󂯕t�����Ԉȉ��Ȃ�
            {
                //- �G�̒e���߂��ɗ��Ă���
                if (lockonEnemy && enemyBlt)
                {
                    if (lockonEnemy.name == enemyBlt.GetComponent<EnemyBullet>().GetEnemyName())
                    {
                        // �W���X�g����A�j���[�V�����Đ�
                        playerAnim.Play("Step", 0, 0.1f);

                        isRoll = false; // ���(���[��)�t���O������
                        isInvRoll = false; // ���G(���[��)�t���O������
                        isStep = true; // ���(�X�e�b�v)�t���O�L����
                        isInvStep = true; // ���G(�X�e�b�v)�t���O�L����
                        curInvTime = 0.0f; // ���݂̖��G���ԏ�����
                        isJust = true; // �W���X�g����t���O�L����
                        isInvJust = true; // �W���X�g����̖��G�t���O�L����
                        isJustSlow = true; // �W���X�g����̃X���[���[�V�����t���O�L����
                        isJustAccept = false; // �W���X�g����󂯕t���t���O������
                        curJustTime = 0.0f; // ���݂̎󂯕t�����ԏ�����
                        Time.timeScale = 0.5f; // �X���[���[�V�����ɂ��邽�߃^�C���X�P�[���ύX
                        Debug.Log("�W���X�g��𔭓�");
                        soundManager.GetComponent<SoundManager>().PlaySE("�W���X�g���"); // �W���X�g���SE�Đ�
                        soundManager.GetComponent<SoundManager>().PlaySE("�e�����"); // �W���X�g���SE�Đ�
                        justUI.DOFade(1.0f, stepInvTime);
                        //- ���f�����̃}�e���A���̓����x��ύX
                        matColor.a = justAlpha / 255.0f;   // �l��0~1�ɂ���
                        chisatoMat[0].DOColor(matColor, stepInvTime); // �}�e���A���̓����x���X�e�b�v�̖��G���ԕ������ĕύX
                        camera.GetComponent<Offset>().SetFollowOffset(); //�W���X�g����̉��o            
                        //- �v���C���[�̌����ύX����
                        Vector3 camRight = camera.transform.right;
                        transform.rotation = Quaternion.LookRotation(-camRight);
                        rb.velocity = new Vector3(camRight.normalized.x * 4, rb.velocity.y, camRight.normalized.z * 4); // ���x�x�N�g���ύX
                    }
                }
                //Debug.Log("�W���X�g�󂯓���");
            }
            else if (curJustTime > justAcceptTime) // ���݂̎󂯕t�����Ԃ��ő�󂯕t�����Ԃ𒴂�����
            {
                curJustTime = 0.0f; // ���݂̎󂯕t�����ԏ�����
                isJustAccept = false; // �W���X�g����󂯕t���t���O������
            }
        }
        //- �_���[�W�t���O���L���Ȃ�
        if (isDamage)
        {
            curDamageTime += Time.deltaTime; // ��e��̌��ݎ��ԍX�V
            curMatChange += Time.deltaTime;  // �}�e���A������ւ��̌��ݎ��ԍX�V
            rb.velocity = new Vector3(0.0f, velocity.y, 0.0f); // �ړ��ʖ�����
            //- �}�e���A������ւ��̌��ݎ��Ԃ��w�莞�Ԃ𒴂�����
            if (curMatChange >= damageInvTime / 7.0f)
            {
                isMatAlpha ^= true; // �}�e���A�����߃t���O�؂�ւ�
                curMatChange = 0.0f; // �}�e���A������ւ��̌��ݎ��Ԃ�������
                if (isMatAlpha) // �}�e���A�����߃t���O���L���Ȃ�
                {
                    //- ���f�����̃}�e���A���𓧉߃}�e���A���ɕύX
                    for (int i = 0; i < chisatoModel.Count; i++)
                    {
                        chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[1];
                    }
                    gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[1];
                }
                else // �}�e���A�����߃t���O�������Ȃ�
                {
                    //- ���f�����̃}�e���A����ʏ�ɕύX
                    for (int i = 0; i < chisatoModel.Count; i++)
                    {
                        chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[0];
                    }
                    gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[0];
                }
            }
            //- ��e��̌��ݎ��Ԃ��_���[�W���G���Ԃ𒴂�����
            if (curDamageTime > damageInvTime)
            {
                isDamage = false;       // �_���[�W�t���O������
                curDamageTime = 0.0f;   // ��e��̌��ݎ��ԏ�����
                curMatChange = 0.0f;    // �}�e���A������ւ��̌��ݎ��ԏ�����
                //- ���f�����̃}�e���A����ʏ�ɕύX
                for (int i = 0; i < chisatoModel.Count; i++)
                {
                    chisatoModel[i].GetComponent<SkinnedMeshRenderer>().material = chisatoMat[0];
                }
                gunModel.GetComponent<SkinnedMeshRenderer>().material = gunMat[0];
            }
        }
    }

    private void NearLockOn()
    {
        //- ��x���������t���O�������Ȃ�
        if (!isOnce)
        {
            //- �t�B�[���h��ɂ���S�G�l�~�[�̏����擾
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            float minDistance = float.MaxValue;
            nearTarget = null;
            //- ��ԋ߂��G������
            foreach (GameObject t in targets)
            {
                float distance = Vector3.Distance(transform.position, t.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearTarget = t;
                }
            }
            isOnce = true; // ��x���������t���O�L����

            playerAnim.SetTrigger("JustStep");
        }
        //- ��ԋ߂��G�̕����Ɍ���
        if (nearTarget)
        {
            var direction = nearTarget.transform.position - transform.position;
            direction.y = 0.0f;
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);
            ////- ��ԋ߂��G�Ɍ������Ĉړ�
            //Vector3 forward = playerForward.transform.position - transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
            //float distance = Vector3.Distance(nearTarget.transform.position, transform.position);
            //Debug.Log("�G�Ƃ̋����F" + distance);
            //rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd * 2.0f, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd * 2.0f); // ���x�x�N�g���ύX
        }
    }

    private void JustAttack()
    {
        //- �e����t���O�������Ȃ�
        if (!isRoll && !isStep)
        {
            //- �߂��̓G��W�I�ɂ���֐����s
            //NearLockOn();
            //- ��x���������t���O�������Ȃ�
            if (!isOnce)
            {
                isOnce = true; // ��x���������t���O�L����
                playerAnim.SetTrigger("JustStep"); // �W���X�g����̃A�j���[�V�������s
                rb.velocity = Vector3.zero; // �ړ��ʂ𖳂���
                Vector3 diff = lockonEnemy.transform.position - transform.position;    // ���݈ʒu����ߋ��̈ʒu�����Z
                diff.y = 0.0f;  // Y���v�f�𖳂���
                transform.rotation = Quaternion.LookRotation(diff);
                //- ��ԋ߂��G�Ɍ������Ĉړ�
                Vector3 moveForward = lockonEnemy.transform.position - transform.position; // �G�Ɍ������x�N�g�����v�Z
                //- �G�Ǝ����̋������v�Z����3��������(�W���X�g�����3���̂���)
                float magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 3;
                if (transform.forward.z > 0) // �v���C���[��Z���̐������������Ă�����
                {
                    //- �W���X�g����̍ŏ��̈���ڂ̍��W���v�Z
                    Vector3 destination = transform.position + new Vector3(transform.forward.x + 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                    //- �v�Z�������W��0.1�b�����Ĉړ�
                    transform.DOMove(destination, 0.1f).OnComplete(() =>
                    {// �ړ����I�������
                        //- �������ɂ����悤���W���v�Z
                        destination = transform.position + transform.forward * -1.1f;
                        //- �񓧖��ɂ���
                        matColor.a = 1.0f;
                        chisatoMat[0].color = matColor;
                        justUI.DOFade(0.0f, 0.5f);
                        //- �v�Z�������W��0.1�b�����Ĉړ�
                        transform.DOMove(destination, 0.1f).OnComplete(() =>
                        {// �ړ����I�������
                            //- �������ɂ��炵�����ɂ���čŏI�I�ȍ��W�������̂łQ���ڂ̍��W���Čv�Z
                            magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 2.0f;
                            destination = transform.position + new Vector3(transform.forward.x - 0.4f, transform.forward.y, transform.forward.z) * magnitude;
                            //- �L�����̓����x��ύX
                            matColor.a = justAlpha / 255.0f; // 0~1�ɂȂ�悤�v�Z
                            chisatoMat[0].DOColor(matColor, 0.3f); // 0.3�b�����ĐF�ύX
                            lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                            soundManager.GetComponent<SoundManager>().PlaySE("�e�����"); // �W���X�g���SE�Đ�
                            //- 0.2�b�҂��Ă���A�Čv�Z�������W��0.1�b�����Ĉړ�
                            transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                            {// �ړ����I�������
                                //- �������ɂ����悤���W���v�Z
                                destination = transform.position + transform.forward * -1.1f;
                                //- �񓧖��ɂ���
                                matColor.a = 1.0f;
                                chisatoMat[0].color = matColor;
                                //- �v�Z�������W��0.1�b�����Ĉړ�
                                transform.DOMove(destination, 0.1f).OnComplete(() =>
                                {// �ړ����I�������
                                    //- �������ɂ��炵�����ɂ���čŏI�I�ȍ��W�������̂�3���ڂ̍��W���Čv�Z
                                    magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position);
                                    destination = transform.position + new Vector3(transform.forward.x + 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                                    //- �L�����̓����x��ύX
                                    matColor.a = justAlpha / 255.0f; // 0~1�ɂȂ�悤�v�Z
                                    chisatoMat[0].DOColor(matColor, 0.3f); // 0.3�b�����ĐF�ύX
                                    lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                                    soundManager.GetComponent<SoundManager>().PlaySE("�e�����"); // �W���X�g���SE�Đ�
                                    //- 0.2�b�҂��Ă���A�Čv�Z�������W��0.1�b�����Ĉړ�
                                    transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                                    {
                                        //- �񓧖��ɂ���
                                        matColor.a = 1.0f;
                                        chisatoMat[0].color = matColor;
                                        Debug.Log("�e����");
                                        //- UI�̐F��ς���
                                        attackUIimg.GetComponent<PushUI>().ShotPush();
                                        attackUIback.GetComponent<PushUI>().ShotPush();
                                        attackUIbut.GetComponent<PushUI>().ShotPush();
                                        //- �T�E���h�Đ�
                                        soundManager.GetComponent<SoundManager>().PlaySE("���C��");
                                        //- �m�Y���t���b�V���Đ�
                                        StartCoroutine(EffectDrawTime());
                                        camera.GetComponent<Offset>().ResetFollowOffset(); //�W���X�g����̉��o�I��
                                        //- �U���A�j���[�V�����Đ�
                                        playerAnim.Play("Shot", 0, 0.35f);
                                        //- �e��������
                                        GameObject Bullet = Instantiate(bullet, new Vector3(
                                            Gun.transform.position.x,
                                            Gun.transform.position.y,
                                            Gun.transform.position.z),
                                            Quaternion.Euler(0, 0, 0));
                                        //- �e�̔��˃x�N�g����G�Ɍ������悤�v�Z
                                        bulletDir = lockonEnemy.transform.position - Gun.transform.position;
                                        //- �v�Z�����x�N�g����e�Ɋi�[
                                        Bullet.GetComponent<PlayerBullet>().SetBullet(
                                            bulletDir.normalized.x,
                                            bulletDir.normalized.y,
                                            bulletDir.normalized.z);
                                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(lockonEnemy.transform.position);
                                        isJust = false; // �W���X�g����t���O������
                                        isInvJust = false; // �W���X�g����̖��G�t���O������
                                        isJustAccept = false; // �W���X�g����󂯕t���t���O������
                                        isInvStep = false;  // �X�e�b�v�̖��G�t���O������
                                        isOnce = false; // ��x���������t���O������
                                        curJustTime = 0.0f; // ���݂̎󂯕t�����ԏ�����
                                        move = Vector2.zero; // �ړ��ʏ�����
                                        Debug.Log("�W���X�g�U������");
                                    });
                                });
                            });
                        });
                    });
                }
                else // �v���C���[��Z���̕������������Ă�����
                {
                    //- �W���X�g����̍ŏ��̈���ڂ̍��W���v�Z
                    Vector3 destination = transform.position + new Vector3(transform.forward.x - 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                    //- �v�Z�������W��0.1�b�����Ĉړ�
                    transform.DOMove(destination, 0.1f).OnComplete(() =>
                    {// �ړ����I�������
                        //- �������ɂ����悤���W���v�Z
                        destination = transform.position + transform.forward * -1.1f;
                        //- �񓧖��ɂ���
                        matColor.a = 1.0f;
                        chisatoMat[0].color = matColor;
                        justUI.DOFade(0.0f, 0.5f);
                        //- �v�Z�������W��0.1�b�����Ĉړ�
                        transform.DOMove(destination, 0.1f).OnComplete(() =>
                        {// �ړ����I�������
                            //- �������ɂ��炵�����ɂ���čŏI�I�ȍ��W�������̂łQ���ڂ̍��W���Čv�Z
                            magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position) / 2.0f;
                            destination = transform.position + new Vector3(transform.forward.x + 0.4f, transform.forward.y, transform.forward.z) * magnitude;
                            //- �L�����̓����x��ύX
                            matColor.a = justAlpha / 255.0f; // 0~1�ɂȂ�悤�v�Z
                            chisatoMat[0].DOColor(matColor, 0.3f); // 0.3�b�����ĐF�ύX
                            lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                            soundManager.GetComponent<SoundManager>().PlaySE("�e�����"); // �W���X�g���SE�Đ�
                            //- 0.2�b�҂��Ă���A�Čv�Z�������W��0.1�b�����Ĉړ�
                            transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                            {// �ړ����I�������
                                //- �������ɂ����悤���W���v�Z
                                destination = transform.position + transform.forward * -1.1f;
                                //- �񓧖��ɂ���
                                matColor.a = 1.0f;
                                chisatoMat[0].color = matColor;
                                //- �v�Z�������W��0.1�b�����Ĉړ�
                                transform.DOMove(destination, 0.1f).OnComplete(() =>
                                {// �ړ����I�������
                                    //- �������ɂ��炵�����ɂ���čŏI�I�ȍ��W�������̂�3���ڂ̍��W���Čv�Z
                                    magnitude = Vector3.Distance(lockonEnemy.transform.position, transform.position);
                                    destination = transform.position + new Vector3(transform.forward.x - 0.2f, transform.forward.y, transform.forward.z) * magnitude;
                                    //- �L�����̓����x��ύX
                                    matColor.a = justAlpha / 255.0f; // 0~1�ɂȂ�悤�v�Z
                                    chisatoMat[0].DOColor(matColor, 0.3f); // 0.3�b�����ĐF�ύX
                                    lockonEnemy.GetComponent<EnemyState_Attack>().PerformanceShot();
                                    soundManager.GetComponent<SoundManager>().PlaySE("�e�����"); // �W���X�g���SE�Đ�
                                    //- 0.2�b�҂��Ă���A�Čv�Z�������W��0.1�b�����Ĉړ�
                                    transform.DOMove(destination, 0.1f).SetDelay(0.2f).OnComplete(() =>
                                    {
                                        //- �񓧖��ɂ���
                                        matColor.a = 1.0f;
                                        chisatoMat[0].color = matColor;
                                        Debug.Log("�e����");
                                        //- UI�̐F��ς���
                                        attackUIimg.GetComponent<PushUI>().ShotPush();
                                        attackUIback.GetComponent<PushUI>().ShotPush();
                                        attackUIbut.GetComponent<PushUI>().ShotPush();
                                        //- �T�E���h�Đ�
                                        soundManager.GetComponent<SoundManager>().PlaySE("���C��");
                                        //- �m�Y���t���b�V���Đ�
                                        StartCoroutine(EffectDrawTime());
                                        camera.GetComponent<Offset>().ResetFollowOffset(); //�W���X�g����̉��o�I��
                                        //- �U���A�j���[�V�����Đ�
                                        playerAnim.Play("Shot", 0, 0.35f);
                                        //- �e��������
                                        GameObject Bullet = Instantiate(bullet, new Vector3(
                                            Gun.transform.position.x,
                                            Gun.transform.position.y,
                                            Gun.transform.position.z),
                                            Quaternion.Euler(0, 0, 0));
                                        //- �e�̔��˃x�N�g����G�Ɍ������悤�v�Z
                                        bulletDir = lockonEnemy.transform.position - Gun.transform.position;
                                        //- �v�Z�����x�N�g����e�Ɋi�[
                                        Bullet.GetComponent<PlayerBullet>().SetBullet(
                                            bulletDir.normalized.x,
                                            bulletDir.normalized.y,
                                            bulletDir.normalized.z);
                                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(lockonEnemy.transform.position);
                                        isJust = false; // �W���X�g����t���O������
                                        isInvJust = false; // �W���X�g����̖��G�t���O������
                                        isJustAccept = false; // �W���X�g����󂯕t���t���O������
                                        isInvStep = false;  // �X�e�b�v�̖��G�t���O������
                                        isOnce = false; // ��x���������t���O������
                                        curJustTime = 0.0f; // ���݂̎󂯕t�����ԏ�����
                                        move = Vector2.zero; // �ړ��ʏ�����
                                        Debug.Log("�W���X�g�U������");
                                    });
                                });
                            });
                        });
                    });
                }
            }
            //Debug.Log("�؂�ւ��t���O�F" + isCutBack);
            if (CheckForwardObj())
            {
                isJust = false; // �W���X�g����t���O������
                isInvJust = false; // �W���X�g����̖��G�t���O������
                isJustSlow = false; // �W���X�g����̃X���[���[�V�����t���O������
                isJustAccept = false; // �W���X�g����󂯕t���t���O������
                isHitForward = true; // �O���Փ˃t���O�L����
                isOnce = false; // ��x���������t���O������
                Time.timeScale = 1.0f; // �^�C���X�P�[�������ɖ߂�
            }
        }
    }

    private void Reload()
    {
        //- �}�K�W����������Ȃ�
        if (magazineNum > 0)
        {
            // �����[�h�A�j���[�V����
            if (!playerAnim.GetBool("isReroad"))
            {
                playerAnim.SetBool("isReload", true);
                playerAnim.Play("Reload");
            }

            reloadImg.GetComponent<ReloadImage>().SetReload(reloadTime); // �����[�hUI�̏������s
            curReloadTime += Time.deltaTime; // ���݂̃����[�h���ԍX�V
            if (curReloadTime > reloadTime) // ���݂̃����[�h���Ԃ��������[�h���Ԃ𒴂�����
            {
                // ���Đ�
                soundManager.GetComponent<SoundManager>().PlaySE("�����[�h");
                playerAnim.SetBool("isReload", false);
                bulletNum = 17; // �c�e������
                magazineNum--; // �}�K�W��������
                magazineUI.HiddenMagazine(magazineNum); // �}�K�W��UI�̕\���ύX
                curReloadTime = 0.0f; // ���݂̃����[�h���ԏ�����
                isReload = false; // �����[�h�t���O������
            }
        }
    }

    private bool CheckForwardObj()
    {
        Ray ray = new Ray(new Vector3(transform.position.x + 0.3f, transform.position.y + 0.1f, transform.position.z + 0.5f), transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy" && hit.collider.tag != "EnemyBullet")
                return true;
        }
        else
        {
            isHitForward = false;
            ray = new Ray(new Vector3(transform.position.x - 0.3f, transform.position.y + 0.1f, transform.position.z + 0.5f), transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.1f))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x + 0.4f, transform.position.y + 1.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.1f))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x - 0.4f, transform.position.y + 1.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.1f))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x + 0.3f, transform.position.y + 2.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.1f))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                    return true;
            }
            else
            {
                isHitForward = false;
                ray = new Ray(new Vector3(transform.position.x - 0.3f, transform.position.y + 2.0f, transform.position.z + 0.5f), transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 100.0f);
            }
        }
        if (!isHitForward)
        {
            if (Physics.Raycast(ray, out hit, 0.1f))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
                    return true;
            }
            else
                isHitForward = false;
        }
        return false;
    }

    private void KeyboardPlay()
    {
        //- �ړ�
        if (Input.GetKey(KeyCode.W))
        {
            verInput = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verInput = -1.0f;
        }
        else
        {
            verInput = 0.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horiInput = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horiInput = 1.0f;
        }
        else
        {
            horiInput = 0.0f;
        }

        Vector3 camForward = camera.transform.forward;
        Vector3 camRight = camera.transform.right;

        Vector3 moveDir = (camForward * verInput + camRight * horiInput).normalized;

        move = new Vector2(moveDir.x * moveSpeed, moveDir.z * moveSpeed);

        //- �U��
        if (Input.GetMouseButtonDown(0))
        {
            if (!isInvRoll && !isInvStep && !isInvJust && bulletNum > 0)
            {
                //- UI�̐F��ς���
                //attackUI.GetComponent<PushUI>().ShotPush();
                //- �T�E���h�Đ�
                soundManager.GetComponent<SoundManager>().PlaySE("���C��");
                //- �m�Y���t���b�V���Đ�
                StartCoroutine(EffectDrawTime());

                //- �e��������
                GameObject Bullet = Instantiate(bullet, new Vector3(Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z), Quaternion.Euler(0, 0, 0));
                bulletNum--;
                if (enemyObj) // �G��������
                {
                    Debug.Log("�G����");
                    //- �e�̔��˃x�N�g����G�Ɍ������悤�v�Z
                    bulletDir = enemyObj.transform.position - Gun.transform.position;
                    //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(
                        bulletDir.normalized.x + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.y + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                }
                else if (lockonEnemy)
                {
                    //- �e�̔��˃x�N�g����G�Ɍ������悤�v�Z
                    bulletDir = lockonEnemy.transform.position - Gun.transform.position;
                    //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                    Bullet.GetComponent<PlayerBullet>().SetBullet(
                        bulletDir.normalized.x + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.y + Random.Range(-0.15f, 0.15f),
                        bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                    Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                }
                else if (!enemyObj) // �G�����Ȃ�������
                {
                    Debug.Log("�G���Ȃ�");
                    if (move == Vector2.zero) // �v���C���[���ړ����ĂȂ�������
                    {
                        //- �e���Ɍ������x�N�g���v�Z
                        bulletDir = Gun.transform.position - playerForward.transform.position;
                        //- �v�Z�����x�N�g����e�Ɋi�[(�����΂炯����)
                        Bullet.GetComponent<PlayerBullet>().SetBullet(bulletDir.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), bulletDir.normalized.z + Random.Range(-0.15f, 0.15f));
                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    }
                    else // �ړ����Ă�����
                    {
                        //- �ړ������̃x�N�g����e�Ɋi�[(�����΂炯����)
                        Bullet.GetComponent<PlayerBullet>().SetBullet(move.normalized.x + Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f), move.normalized.y + Random.Range(-0.15f, 0.15f));
                        Bullet.GetComponent<PlayerBullet>().SetPlayerPos(transform.position);
                    }
                }
            }
        }

        // ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isInvStep && eneBarCS.energy >= eneBarCS.SubStep)
            {
                isMove = false;     // �ړ��t���O������
                isRoll = true;      // ���[���t���O�L����
                isInvRoll = true;   // ���G�t���O(���)�L����
                isJustAccept = true;// �W���X�g����󂯕t���t���O�L����
                //- UI�̐F��ς���
                //stepUI.GetComponent<PushUI>().Push();
                //steptext.GetComponent<PushUI>().PushTextUI();
                //- �T�E���h�Đ�
                soundManager.GetComponent<SoundManager>().PlaySE("�X�e�b�v���");
                Vector3 forward = playerForward.transform.position - transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
                rb.velocity = new Vector3(forward.normalized.x * moveSpeed * rollMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * rollMltSpd); // ���x�x�N�g���ύX
                Debug.Log("���G(���)");
            }
        }

        //- �_�b�V��
        //if(Input.GetKey(KeyCode.LeftShift))
        //{
        //    curShiftTime += Time.deltaTime;
        //    if (curShiftTime > 0.2f)
        //    {
        //        if (eneBarCS.energy >= eneBarCS.SubDash)
        //        {
        //            Debug.Log("�_�b�V��");
        //            isDash = true;  // �_�b�V���t���O�L����
        //            moveSpeed = dashSpeed; // �ړ����x�ύX
        //        }
        //    }
        //}

        //-�@�X�e�b�v
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (/*!isDash && */!isInvRoll && eneBarCS.energy >= eneBarCS.SubStep) // �_�b�V���t���O�������Ȃ�
            {
                Debug.Log("�X�e�b�v");
                isStep = true;  // �X�e�b�v�t���O�L����
                isInvStep = true; // �X�e�b�v���G�t���O�L����
                isJustAccept = true; // �W���X�g����󂯕t���t���O�L����
                //curShiftTime = 0.0f;
                // UI�̐F��ς���
                //rollUI.GetComponent<PushUI>().Push();
                //- �T�E���h�Đ�
                soundManager.GetComponent<SoundManager>().PlaySE("���[�����2");
                Vector3 forward = playerForward.transform.position - transform.position; // �v���C���[�̐��ʕ����̃x�N�g�����v�Z
                rb.velocity = new Vector3(forward.normalized.x * moveSpeed * stepMltSpd, rb.velocity.y, forward.normalized.z * moveSpeed * stepMltSpd); // ���x�x�N�g���ύX
                Debug.Log("���G(�X�e�b�v)");
            }
            //else if (isDash) // �_�b�V���t���O���L���Ȃ�
            //{
            //    isDash = false;   // �_�b�V���t���O������
            //    moveSpeed = 5.0f; // �ړ����x�ύX
            //    curShiftTime = 0.0f;
            //}
        }

        //- ���b�N�I��
        if (Input.GetMouseButtonDown(2))
        {
            //- �t�B�[���h��ɂ���S�G�l�~�[�̏����擾
            targets = GameObject.FindGameObjectsWithTag("Enemy");
            float minDistance = float.MaxValue;
            GameObject nearTarget = null;
            isLockon ^= true;
            if (targets[0])
            {
                //- ��ԋ߂��G������
                foreach (GameObject t in targets)
                {
                    // �G�l�~�[�̈ʒu�����[���h���W����r���[�|�[�g���W�ɕϊ�
                    Vector3 viewportPoint = mainCamera.WorldToViewportPoint(t.transform.position);

                    if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                    {
                        float distance = Vector3.Distance(transform.position, t.transform.position);

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearTarget = t;
                        }
                    }
                }
                //- ��ԋ߂��G�̕����Ɍ���
                if (nearTarget)
                {
                    var direction = nearTarget.transform.position - transform.position;
                    direction.y = 0.0f;
                    var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);
                    lockonEnemy = nearTarget;
                }
            }
        }
    }

    IEnumerator EffectDrawTime()
    {
        // �G�t�F�N�g�̕\���A��\��
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.SetActive(false);
    }
}
