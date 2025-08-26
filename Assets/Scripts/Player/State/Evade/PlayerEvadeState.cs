using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvadeState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        //Debug.Log(playerModel.state);
        switch(playerModel.currentState)
        {
            case PlayerState.Evade_Front:
                playerModel.PlayAnimation("Evade_Front");
                break;
            case PlayerState.Evade_Back:
                playerModel.PlayAnimation("Evade_Back");
                break;
        }
    }
    public override void Update()
    {
        base.Update();

        if (playerModel.IsAnimationBreak())
        {
            switch (playerModel.currentState)
            {
                case PlayerState.Evade_Front:
                    if (playerController.input.Player.Evade.IsPressed())
                    {
                        playerModel.SwitchState(PlayerState.Run);

                        return;
                    }  
                    playerModel.SwitchState(PlayerState.Evade_Front_End);
                    break;
                case PlayerState.Evade_Back:
                    playerModel.SwitchState(PlayerState.Evade_Back_End);
                    break;
            }
        }
    }
}
