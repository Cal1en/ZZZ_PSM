using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerRunState : PlayerStateBase 
{
    private Camera mainCamera;

    private float inputZero_timer = 0f;

    public override void Enter()
    {
        base.Enter();

        mainCamera = Camera.main;

        //ÅÐ¶ÏÒÆ¶¯×´Ì¬
        switch (playerModel.currentState)
        {
            case PlayerState.Walk:
                #region ×óÓÒ½ÅÅÐ¶Ï
                switch (playerModel.foot)
                {
                    case ModelFoot.Left:
                        playerModel.PlayAnimation("Walk", 0.25f, 0, 0.6f);
                        playerModel.foot = ModelFoot.Right;
                        break;
                    case ModelFoot.Right:
                        playerModel.PlayAnimation("Walk", 0.25f, 0, 0);
                        playerModel.foot = ModelFoot.Left;
                        break;
                }
                #endregion
                break;
            case PlayerState.Run:
                #region ×óÓÒ½ÅÅÐ¶Ï
                switch (playerModel.foot)
                {
                    case ModelFoot.Left:
                        playerModel.PlayAnimation("Run", 0.25f, 0, 0.5f);
                        playerModel.foot = ModelFoot.Right;
                        break;
                    case ModelFoot.Right:
                        playerModel.PlayAnimation("Run", 0.25f, 0, 0);
                        playerModel.foot = ModelFoot.Left;
                        break;
                }
                #endregion
                break;
        }
        //¹Ø±Õ½Å²½IK
        //playerModel.footIK.Close();
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

        //¾²Ö¹¼ì²â
        if (playerController.inputMoveVec2 == Vector2.zero)
        {
            inputZero_timer += Time.deltaTime;
            if (inputZero_timer > 0.1f)
            {
                Debug.Log("11111111111");
                playerModel.SwitchState(PlayerState.RunEnd);
                return;
            }
        }
        else
        {
            inputZero_timer = 0f;

            #region ´¦ÀíÒÆ¶¯·½Ïò
            Vector3 inputMoveVec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);

            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;

            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMoveVec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);

            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);

            //180×ªÉíÅÐ¶Ï 
            if (angles > 160 && angles < 200)
            {
                Debug.Log(playerModel.currentState);
                playerModel.lastState = playerModel.currentState;
                playerModel.SwitchState(PlayerState.TurnBack);

                return;
            }
            else
            {
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, playerController.rotationSpeed * Time.deltaTime);
            }    
            #endregion
        }

        //¼²ÅÜÇÐ»»
        if (playerModel.currentState == PlayerState.Walk && statePlayingTime > 3f)
        {
            playerModel.SwitchState(PlayerState.Run);

            return;
        }
    }

    public override void Exit()
    {
        base.Exit();

        //¿ªÆô½Å²½IK
        //playerModel.footIK.Open();
    }
}
