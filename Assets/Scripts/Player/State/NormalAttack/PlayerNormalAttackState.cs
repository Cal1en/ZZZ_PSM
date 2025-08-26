using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttackState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.attackFlag = false;

        #region 锁定最近敌人
        GameObject targetEnemy = null;  //目标敌人
        float minDistance = Mathf.Infinity;     //初始化敌人的最近距离

        foreach(string tag in playerController.enemyTagList)
        {
            //获取该敌人标签下的敌人数组
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(playerModel.transform.position, enemy.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }

        //如果目标敌人不为空 且敌人距离较近
        if (targetEnemy != null && minDistance < 5f)
        {
            //计算玩家和最近敌人的方向
            Vector3 direction = (targetEnemy.transform.position - playerModel.transform.position).normalized;

            //让玩家模型面朝敌人(只需要面朝xz方向)
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        }
        
        #endregion

        playerModel.PlayAnimation("Attack_Normal_" + playerModel.skillConfig.currentNormalAttackIndex.ToString());
    }

    public override void Update()
    {
        base.Update();

        //移动监测
        AnimatorStateInfo stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
        if( stateInfo.normalizedTime < 0.4f && playerController.input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            #region 处理移动方向
            Vector3 inputMoveVec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);

            float cameraAxisY = Camera.main.transform.rotation.eulerAngles.y;

            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMoveVec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);

            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);

            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, playerController.rotationSpeed * Time.deltaTime);

            #endregion
        }

        //闪避检测
        if (playerController.input.Player.Evade.triggered)
        {
            //重置攻击段数
            playerModel.skillConfig.currentNormalAttackIndex = 1;

            playerModel.SwitchState(PlayerState.Evade_Front);

            return;
        }

        //如果继续攻击
        if (playerController.input.Player.Fire.triggered)
        {
            playerController.attackFlag = true;
        }

        if (playerModel.IsAnimationBreak())
        {
            playerModel.SwitchState(PlayerState.NormalAttack_End);
        }
    }
}
