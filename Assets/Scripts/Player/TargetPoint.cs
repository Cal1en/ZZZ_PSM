using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    //跟踪点距地高度
    private float height;

    //角色位置
    private Vector3 playerPos;

    private void Awake()
    {
        height = transform.position.y;
    }

    private void LateUpdate()
    {
        playerPos = PlayerController.INSTANCE.currentPlayerModel.transform.position;

        transform.position = new Vector3(playerPos.x, playerPos.y + height, playerPos.z);
    }
}
