using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvadeEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        switch (playerModel.currentState)
        {
            case PlayerState.Evade_Front_End:
                playerModel.PlayAnimation("Evade_Front_End");
                break;
            case PlayerState.Evade_Back_End:
                playerModel.PlayAnimation("Evade_Back_End");
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
        }

        //ÉÁ±Ü¼ì²â
        if (playerController.input.Player.Evade.triggered)
        {
            switch(playerModel.currentState)
            {
                case PlayerState.Evade_Front:
                    playerModel.SwitchState(PlayerState.Evade_Front);
                    break;
                case PlayerState.Evade_Back:
                    playerModel.SwitchState(PlayerState.Evade_Back);
                    break;
            }
        }

        #region ÒÆ¶¯¼à²â
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            playerModel.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion

        #region ¶¯»­ÊÇ·ñ²¥·Å½áÊø
        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.Idle);
        }
        #endregion
    }
}
