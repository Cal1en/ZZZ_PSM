using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineOwner { }

public class StateMachine
{
    private StateBase currentState;

    private IStateMachineOwner owner;

    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    #region 状态机初始化
    public StateMachine(IStateMachineOwner owner)
    {
        Init(owner);
    }
    public void Init(IStateMachineOwner owner)
    {
        this.owner = owner;
    }
    #endregion


    /// <summary>
    /// 进入状态
    /// </summary>
    /// <typeparam name="T">状态类型</typeparam>
    /// <param name="reloadState">是否强制刷新状态</param>
    public void EnterState<T>(bool reloadState = false) where T : StateBase, new()
    {
        if (currentState?.GetType() == typeof(T) && !reloadState)
        {
            return;
        }

        currentState?.Exit();
        currentState = LoadState<T>();
        currentState.Enter();
    }

    private StateBase LoadState<T>() where T : StateBase, new()
    {
        Type stateType = typeof(T);

        //如果字典中没有创建过该状态
        if (!stateDic.TryGetValue(stateType, out StateBase state))
        {
            //创建该状态实例并保存到字典
            state = new T();
            state.Init(owner);
            stateDic.Add(stateType, state);
        }

        return state;
    }

    public void Stop()
    {
        currentState?.Exit();

        currentState = null;
        foreach (var state in stateDic.Values)
        {
            state.UnInit();
        }
        stateDic.Clear();
    }
}
