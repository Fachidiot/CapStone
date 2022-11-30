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

    new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<NetworkTPController>();
    }

    protected override void Zoom(Vector3 mousePosition)
    {
        // if Player do zoom
        if (input.mouseR)
        {
            CrossHair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            aimed = true;

            mousePosition.y = transform.position.y;
            Vector3 aimDirection = (mousePosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            WeaponManager.WeaponHolster(false);
        }
        else
        {
            CrossHair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            aimed = false;
            WeaponManager.WeaponHolster(true);
        }
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
