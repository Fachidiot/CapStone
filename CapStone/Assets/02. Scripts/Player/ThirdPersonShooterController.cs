using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(ThirdPersonController))]
public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    GameObject CrossHair;
    [SerializeField]
    LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField]
    Transform debugTransform;
    [Range(1, 30)]
    public float normalSensitivity = 3f;
    [Range(1, 10)]
    public float aimSensitivity = 2f;

    //Vector2 
    float m_prevSensitivity;
    ThirdPersonController thirdPersonController;
    CustomInput input;

    void Awake()
    {
        input = GetComponent<CustomInput>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        Zoom();
        Cursor();
    }

    void Zoom()
    {
        if (input.mouseR)
        {
            CrossHair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
        }
        else
        {
            CrossHair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
        }
    }

    Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    void Cursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
        }
    }
}
