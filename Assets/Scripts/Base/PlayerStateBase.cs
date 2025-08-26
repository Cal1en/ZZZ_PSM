using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerStateBase : StateBase
{
    protected PlayerController playerController;

    protected PlayerModel playerModel;

    protected float statePlayingTime = 0f;

    public override void Init(IStateMachineOwner owner)
    {
        playerController = PlayerController.INSTANCE;
        playerModel = (PlayerModel)owner; 
        
    }
    public override void Enter()
    {
        MonoManager.INSTANCE.AddUpdateAction(Update);
        MonoManager.INSTANCE.AddFixedUpdateAction(FixedUpdate);
        MonoManager.INSTANCE.AddLateUpdateAction(LateUpdate);

        statePlayingTime = 0f;
    }

    public override void Exit()
    {
        MonoManager.INSTANCE.RemoveUpdateAction(Update);
        MonoManager.INSTANCE.RemoveFixedUpdateAction(FixedUpdate);
        MonoManager.INSTANCE.RemoveLateUpdateAction(LateUpdate);
    }

    public override void FixedUpdate()
    {
    }
    public override void LateUpdate()
    {
    }

    public override void UnInit()
    {
    }

    public override void Update()
    {
        statePlayingTime += Time.deltaTime;

        //÷ÿ¡¶
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.deltaTime, 0));

        //ºÏ≤‚¥Û’–
        if (playerController.input.Player.BigSkill.triggered)
        {
            //÷ÿ÷√π•ª˜∂Œ ˝
            playerModel.skillConfig.currentNormalAttackIndex = 1;

            playerModel.SwitchState(PlayerState.BigSkill_Start);
            return;
        }

        #region ºÏ≤‚Ω«…´«–ªª
        if (playerController.input.Player.Switch.triggered
            && playerModel.currentState != PlayerState.BigSkill_Start
            && playerModel.currentState != PlayerState.BigSkill)
        {
            playerController.SwitchModel();
        }
        #endregion
    }
}
