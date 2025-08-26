using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigSkillEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //¹Ø±Õ½Å²½IK
        //playerModel.footIK.Close();

        //ÇÐ»»¾µÍ·
        CameraManager.INSTANCE.cm_Brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
        //playerModel.bigSkillShot.SetActive(false);
        //CameraManager.INSTANCE.freelookCamera.SetActive(true);
        playerModel.bigSkillShot.GetComponent<CinemachineVirtualCamera>().Priority = -100;
        CameraManager.INSTANCE.freelookCamera.GetComponent<CinemachineFreeLook>().Priority = 100;

        //ÖØÖÃ¾µÍ·
        //CameraManager.INSTANCE.ResetFreeLookCamera();


        if (playerController.currentPlayerModel.name.StartsWith("AnBi"))
        {
            playerModel.PlayAnimation("BigSkill_End", 1f);
        }
        else
        {
            playerModel.PlayAnimation("BigSkill_End", 0f);
        }
            
    }

    public override void Update()
    {
        base.Update();

        //¼ì²â¹¥»÷
        if (playerController.input.Player.Fire.triggered)
        {
            playerModel.SwitchState(PlayerState.NormalAttack);
            return;
        }

        //ÉÁ±Ü¼ì²â
        if (playerController.input.Player.Evade.triggered)
        {
            playerModel.SwitchState(PlayerState.Evade_Back);
            return;
        }

        //ÒÆ¶¯¼à²â
        if (playerController.inputMoveVec2.magnitude != 0)
        {
            playerModel.SwitchState(PlayerState.Walk);
            return;
        }

        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        CameraManager.INSTANCE.freeLook.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        //¿ªÆô½Å²½IK
        //playerModel.footIK.Open();
    }
}
