using FischlWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    SwitchIn_Normal,
    Idle, Idle_AFK,
    Walk,
    Run,RunEnd,
    TurnBack,
    Evade_Front, Evade_Back, Evade_Front_End, Evade_Back_End,
    NormalAttack, NormalAttack_End,
    BigSkill_Start, BigSkill, BigSkill_End,
}
public enum ModelFoot
{
    Right,
    Left,
}
public class PlayerModel : MonoBehaviour, IStateMachineOwner, IHurt
{
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public PlayerState lastState;

    [HideInInspector] public Animator animator;

    public StateMachine stateMachine;

    [HideInInspector] public CharacterController characterController;

    public GameObject bigSkillStartShot;     //大招Start镜头
    public GameObject bigSkillShot;     //大招镜头

    //[HideInInspector] public csHomebrewIK footIK;

    public PlayerConfig playerConfig;

    public SkillConfig skillConfig;

    public float gravity = -9.8f;

    private float evadeTimer = 1f;

    //武器列表
    public WeaponController[] weapons;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();

        //footIK = GetComponent<csHomebrewIK>();

        stateMachine = new StateMachine(this);
        Debug.Log("newew");

        //重置攻击段数
        skillConfig.currentNormalAttackIndex = 1;
    }

    private void Start()
    {
        //SwitchState(PlayerState.Idle);
    }
    private void Update()
    {
        if (evadeTimer < 1f)
        {
            evadeTimer += Time.deltaTime;
        }
        else if (evadeTimer > 1f)
        {
            evadeTimer = 1f;
        }

    }
    public void SwitchState(PlayerState playerState)
    {
        currentState = playerState;

        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Idle_AFK:
                stateMachine.EnterState<PlayerIdleState>(true);
                break;
            case PlayerState.Walk:
            case PlayerState.Run:
                stateMachine.EnterState<PlayerRunState>(true);
                break;
            case PlayerState.RunEnd:
                stateMachine.EnterState<PlayerRunEndState>();
                break;
            case PlayerState.TurnBack:
                stateMachine.EnterState<PlayerTurnBackState>();
                break;
            case PlayerState.Evade_Front:
            case PlayerState.Evade_Back:
                if (evadeTimer != 1f) return;     //避免state记录闪避状态
                stateMachine.EnterState<PlayerEvadeState>();
                evadeTimer = 0f;
                break;
            case PlayerState.Evade_Front_End:
            case PlayerState.Evade_Back_End:
                stateMachine.EnterState<PlayerEvadeEndState>();
                break;
            case PlayerState.NormalAttack:
                stateMachine.EnterState<PlayerNormalAttackState>();
                break;
            case PlayerState.NormalAttack_End:
                stateMachine.EnterState<PlayerNormalAttackEndState>();
                break;
            case PlayerState.BigSkill_Start:
                stateMachine.EnterState<PlayerBigSkillStartState>();
                break;
            case PlayerState.BigSkill:
                stateMachine.EnterState<PlayerBigSkillState>();
                break;
            case PlayerState.BigSkill_End:
                stateMachine.EnterState<PlayerBigSkillEndState>();
                break;
            case PlayerState.SwitchIn_Normal:
                stateMachine.EnterState<PlayerSwitchInNormalState>();
                break;
        }

        lastState = playerState;
    }

    #region 动画方法
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <param name="fixedTransitionDuration">过渡时间</param>
    /// <param name="layer">动画层</param>
    /// <param name="fixedTimeOffset">动画起始播放偏移</param>
    public void PlayAnimation(string animationName, float fixedTransitionDuration, int layer, float fixedTimeOffset)
    {
        animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration, layer, fixedTimeOffset);
    }
    public void PlayAnimation(string animationName, float fixedTransitionDuration = 0.25f, int layer = 0)
    {
        animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration, layer);
    }
    public bool IsAnimationBreak()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }
    #endregion


    #region 动画左右脚状态
    [HideInInspector] public ModelFoot foot = ModelFoot.Right;

    /// <summary>
    /// 迈出左脚
    /// </summary>
    [HideInInspector]
    public void SetOutLeftFoot()
    {
        foot = ModelFoot.Left;
    }

    /// <summary>
    /// 迈出右脚
    /// </summary>
    public void SetOutRightFoot()
    {
        foot = ModelFoot.Right;
    }
    #endregion


    public void ResetGravity()
    {
        gravity = -9.8f;
    }

    private void OnDisable()
    {
        //重置普通攻击段数
        skillConfig.currentNormalAttackIndex = 1;
    }

    #region 切换模型方法
    /// <summary>
    /// 进入模型
    /// </summary>
    public void EnterModel(Vector3 position, Quaternion rotation)
    {
        MonoManager.INSTANCE.RemoveUpdateAction(OnExit);

        Vector3 rightDirection = rotation * Vector3.right;
        position += rightDirection * 0.8f;

        Vector3 backDirection = rotation * Vector3.back;
        //position += backDirection * 3f;

        // 临时禁用CharacterController，直接设置位置，然后重新启用
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;

        transform.rotation = rotation;
    }

    /// <summary>
    /// 退出模型
    /// </summary>
    public void ExitModel()
    {
        //characterController.enabled = false;

        animator.CrossFadeInFixedTime("SwitchOut_Normal", 0.1f);

        MonoManager.INSTANCE.AddUpdateAction(OnExit);
    }


    public void OnExit()
    {
        if (IsAnimationBreak())
        {
            gameObject.SetActive(false);
            MonoManager.INSTANCE.RemoveUpdateAction(OnExit);
        }
    }
    #endregion

    #region 受击方法
    public void Init(List<string> enemyTagList)
    {
        foreach (WeaponController weapon in weapons)
        {
            weapon.Init(enemyTagList, OnHit);
        }
    }
    public void OnHit(IHurt enemy)
    {
        Debug.Log(((Component)enemy).name);
    }
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
    }
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }
    #endregion
}
