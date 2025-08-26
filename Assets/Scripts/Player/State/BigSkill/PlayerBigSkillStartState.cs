using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigSkillStartState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //关闭脚步IK
        //playerModel.footIK.Close();

        playerModel.gravity = 0f;

        //切换镜头
        CameraManager.INSTANCE.cm_Brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        //CameraManager.INSTANCE.freelookCamera.SetActive(false);
        //playerModel.bigSkillStartShot.SetActive(true);
        CameraManager.INSTANCE.freelookCamera.GetComponent<CinemachineFreeLook>().Priority = -100;
        playerModel.bigSkillStartShot.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        //重置镜头
        CameraManager.INSTANCE.ResetFreeLookCamera();

        //播放动画
        playerModel.PlayAnimation("BigSkill_Start", 0f);
    }

    public override void Update()
    {
        base.Update();

        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.BigSkill);
        }
    }
}
