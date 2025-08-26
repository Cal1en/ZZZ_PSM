using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;

    private void LateUpdate()
    {
        transform.position = parentTransform.position;

        transform.rotation = parentTransform.rotation;
        //transform.rotation = new Quaternion(parentTransform.rotation.x,parentTransform.rotation.y, parentTransform.rotation.z,parentTransform.rotation.w);

    }
}
