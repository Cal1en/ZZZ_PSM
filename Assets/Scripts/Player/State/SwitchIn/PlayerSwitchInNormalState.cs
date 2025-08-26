using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchInNormalState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerModel.PlayAnimation("SwitchIn_Normal", 0f);
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
            return;
        }
    }
}
