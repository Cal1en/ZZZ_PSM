using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigSkillState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //¹Ø±Õ½Å²½IK
        //playerModel.footIK.Close();

        //ÇÐ»»¾µÍ·
        //playerModel.bigSkillStartShot.SetActive(false);
        // playerModel.bigSkillShot.SetActive(true);
        playerModel.bigSkillStartShot.GetComponent<CinemachineVirtualCamera>().Priority = -100;
        playerModel.bigSkillShot.GetComponent<CinemachineVirtualCamera>().Priority = 100;


        playerModel.PlayAnimation("BigSkill", 0f);
    }

    public override void Update()
    {
        base.Update();

        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.BigSkill_End);
        }
    }

}
