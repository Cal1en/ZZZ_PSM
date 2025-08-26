using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBackState : PlayerStateBase
{
    private PlayerState realLastState;


    public override void Enter()
    {
        base.Enter();

        realLastState = playerModel.lastState;

        playerModel.PlayAnimation("TurnBack", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if (playerModel.IsAnimationBreak())
        {
            switch (realLastState)
            {
                case PlayerState.Walk:
                    playerModel.SwitchState(PlayerState.Walk);
                    break;
                case PlayerState.Run:
                    playerModel.SwitchState(PlayerState.Run);
                    break;
            }     
        }
    }
}
