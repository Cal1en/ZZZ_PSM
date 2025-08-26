using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        switch (playerModel.currentState)
        {
            case PlayerState.Idle:
                playerModel.PlayAnimation("Idle");
                break;
            case PlayerState.Idle_AFK:
                playerModel.PlayAnimation("Idle_AFK");
                break;
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

        //¹Ò»ú¼ì²â
        switch (playerModel.currentState)
        {
            case PlayerState.Idle:
                if (statePlayingTime > 3f)
                {
                    //ÇÐ»»µ½¹Ò»ú×´Ì¬
                    playerModel.SwitchState(PlayerState.Idle_AFK);
                }
                break;
            case PlayerState.Idle_AFK:
                if (playerModel.IsAnimationBreak())
                {
                    //ÇÐ»»µ½´ý»ú×´Ì¬
                    playerModel.SwitchState(PlayerState.Idle);
                }
                break;
        }

        
    }
}
