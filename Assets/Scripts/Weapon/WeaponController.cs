using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //敌人标签列表
    private List<string> enemyTagList;

    //单次攻击的敌人受击列表
    private List<IHurt> enemyHurtList = new List<IHurt>();

    [SerializeField] private Collider hitCollider;

    //命中事件
    private Action<IHurt> onHitAction;

    public void Init(List<string> enemyTagList, Action<IHurt> onHitAction) 
    {
        hitCollider.enabled = false;

        this.enemyTagList = enemyTagList;
        this.onHitAction = onHitAction;
    }

    public void StartHit()
    {
        hitCollider.enabled = true;
    }
    public void StopHit()
    {
        hitCollider.enabled = false;
        enemyHurtList.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        //检测对象是否为敌人
        if (enemyTagList.Contains(other.tag))
        {
            IHurt enemy = other.GetComponent<IHurt>();

            if (enemy != null && !enemyHurtList.Contains(enemy))
            {
                //记录攻击到的敌人
                enemyHurtList.Add(enemy);

                #region 让敌人受击

                //触发命中事件
                onHitAction?.Invoke(enemy);

                #endregion
            }
            else if (enemy == null) 
            {
                //没有挂载IHurt
                Debug.LogError($"该受击对象{other.name}不包含受击接口");
            }
        }
    }
}
