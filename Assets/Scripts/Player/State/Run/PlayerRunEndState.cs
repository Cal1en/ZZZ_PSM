using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region ÅÐ¶Ï×óÓÒ½Å
        switch (playerModel.foot)
        {
            case ModelFoot.Right:
                playerModel.PlayAnimation("Run_End_R", 0.1f);
                break;
            case ModelFoot.Left:
                playerModel.PlayAnimation("Run_End_L", 0.1f);
                break;
        }
        #endregion
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
            playerModel.SwitchState(PlayerState.Evade_Front);
            return;
        }

        #region ÒÆ¶¯¼à²â
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            Debug.Log("22222222");

            playerModel.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion

        #region ¶¯»­ÊÇ·ñ²¥·Å½áÊø
        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
