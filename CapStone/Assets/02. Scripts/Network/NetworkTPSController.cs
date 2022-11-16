using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using System;

[RequireComponent(typeof(NetworkTPController))]
public class NetworkTPSController : ThirdPersonShooterController
{
    NetworkTPController thirdPersonController;

    void Start()
    {
        thirdPersonController = GetComponent<NetworkTPController>();
    }

    internal void SetUIAll(GameObject crossHairUI, GameObject hitUI, Transform target)
    {
        this.CrossHair = crossHairUI;
        this.targetTransform = target;
    }

    internal void SetCamera(CinemachineVirtualCamera aimCamera)
    {
        this.aimVirtualCamera = aimCamera.gameObject;
    }

    internal void SetAimLayer(LayerMask aimColliderLayerMask)
    {
        this.aimColliderLayerMask = aimColliderLayerMask;
    }
}
