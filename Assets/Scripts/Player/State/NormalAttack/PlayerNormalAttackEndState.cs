using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttackEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        if (playerController.attackFlag && playerModel.skillConfig.currentNormalAttackIndex < playerModel.skillConfig.normalAttackDamageMultiple.Length)
        {
            //累加攻击段数
            playerModel.skillConfig.currentNormalAttackIndex++;

            //进入下一段普攻
            playerModel.SwitchState(PlayerState.NormalAttack);
            return;
        }
        playerModel.PlayAnimation("Attack_Normal_" + playerModel.skillConfig.currentNormalAttackIndex.ToString() + "_End");

        //累加攻击段数
        playerModel.skillConfig.currentNormalAttackIndex++;
    }

    public override void Update()
    {
        base.Update();

        //如果超出攻击段数上限
        if (playerModel.skillConfig.currentNormalAttackIndex > playerModel.skillConfig.normalAttackDamageMultiple.Length)
        {
            //重置攻击段数
            playerModel.skillConfig.currentNormalAttackIndex = 1;
        }

        //闪避检测
        if (playerController.input.Player.Evade.triggered)
        {
            //重置攻击段数
            playerModel.skillConfig.currentNormalAttackIndex = 1;

            playerModel.SwitchState(PlayerState.Evade_Back);

            return;
        }

        #region 移动监测
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //重置攻击段数
            playerModel.skillConfig.currentNormalAttackIndex = 1;

            playerModel.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion

        //检测动画是否播放结束
        if (playerModel.IsAnimationBreak())
        {
            //重置攻击段数
            playerModel.skillConfig.currentNormalAttackIndex = 1;

            playerModel.SwitchState(PlayerState.Idle);
        }
    }
}
