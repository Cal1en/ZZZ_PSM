using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingleMonoBase<CameraManager>
{
    public CinemachineBrain cm_Brain;

    [Tooltip("自由相机")] public GameObject freelookCamera;

    [HideInInspector] public CinemachineFreeLook freeLook;

    protected override void Awake()
    {
        base.Awake();

        freeLook = freelookCamera.transform.GetComponent<CinemachineFreeLook>();
    }

    /// <summary>
    /// 重置镜头
    /// </summary>
    public void ResetFreeLookCamera()
    {
        freeLook.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
        freeLook.m_Heading.m_Definition = CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward;
        freeLook.m_Heading.m_Bias = PlayerController.INSTANCE.currentPlayerModel.transform.eulerAngles.y;
        //freeLook.m_XAxis.Value = PlayerController.INSTANCE.currentPlayerModel.transform.eulerAngles.y;
        //Debug.Log(freeLook.m_XAxis.Value);
        freeLook.m_YAxis.Value = 0.5f;
    }
}
