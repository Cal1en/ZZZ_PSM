using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : SingleMonoBase<MonoManager>
{
    //Update
    public Action updateAction;

    public void AddUpdateAction(Action action)      //Enter
    {
        updateAction += action;
    }
    public void RemoveUpdateAction(Action action)   //Exit
    {
        updateAction -= action;
    }
    private void Update()
    {
        updateAction?.Invoke();
    }


    //FixedUpdate
    public Action fixedUpdateAction;

    public void AddFixedUpdateAction(Action action)     //Enter
    {
        fixedUpdateAction += action;
    }
    public void RemoveFixedUpdateAction(Action action)   //Exit
    {
        fixedUpdateAction -= action;
    }
    private void FixedUpdate()
    {
        fixedUpdateAction?.Invoke();
    }


    //LateUpdate
    public Action lateUpdateAction;

    public void AddLateUpdateAction(Action action)     //Enter
    {
        lateUpdateAction += action;
    }
    public void RemoveLateUpdateAction(Action action)   //Exit
    {
        lateUpdateAction -= action;
    }
    private void LateUpdate()
    {
        lateUpdateAction?.Invoke();
    }

}
