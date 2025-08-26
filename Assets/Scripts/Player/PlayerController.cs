using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingleMonoBase<PlayerController>
{
    public PlayerModel currentPlayerModel;

    [HideInInspector] public PlayerInputAction input;
    [HideInInspector] public Vector2 inputMoveVec2;      //移动输入

    public float rotationSpeed = 12f;    //转向速度

    [HideInInspector] public bool attackFlag = false;

    public PlayerConfig playerConfig;

    private List<PlayerModel> controllableModels;

    private int currentPlayerModelIndex = 0;

    [Tooltip("敌人标签列表")] public List<string> enemyTagList;

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputAction();
        input.Enable();

        controllableModels = new List<PlayerModel>();

        #region 生成角色模型
        for (int i = 0; i < playerConfig.playerModels.Length; i++)
        {
            GameObject model = Instantiate(playerConfig.playerModels[i], transform);
            controllableModels.Add(model.GetComponent<PlayerModel>());
            //model.GetComponent<PlayerModel>().stateMachine = new StateMachine(model.GetComponent<PlayerModel>());
            model.SetActive(false);

            controllableModels[i].Init(enemyTagList);
        }
        #endregion

        #region 操控配队中的第一个模型
        controllableModels[0].gameObject.SetActive(true);
        currentPlayerModel = controllableModels[0];
        
        #endregion
    }
    private void Start()
    {
        currentPlayerModel.SwitchState(PlayerState.Idle);
        //锁定光标
        LockMouse();
    }

    private void Update()
    {
        //更新移动输入
        inputMoveVec2 = input.Player.Move.ReadValue<Vector2>().normalized;

        //Debug.Log(currentPlayerModel.transform.position);


    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    /// <summary>
    /// 锁定光标
    /// </summary>
    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SwitchModel()
    {
        Vector3 prevPos = currentPlayerModel.transform.position;

        Quaternion prevRot = currentPlayerModel.transform.rotation;

        //刷新状态机
        currentPlayerModel.stateMachine.Stop();

        //角色切换
        currentPlayerModel.ExitModel();

        currentPlayerModelIndex++;

        if (currentPlayerModelIndex >= controllableModels.Count)
        {
            currentPlayerModelIndex = 0;
        }

        PlayerModel model = controllableModels[currentPlayerModelIndex];

        model.gameObject.SetActive(true);

        currentPlayerModel = model;

        currentPlayerModel.EnterModel(prevPos, prevRot);
        
        

        currentPlayerModel.SwitchState(PlayerState.SwitchIn_Normal);
    }

}
