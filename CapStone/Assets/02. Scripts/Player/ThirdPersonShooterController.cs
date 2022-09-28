using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
    Transform targetTransform;
    [SerializeField]
    List<MultiAimConstraint> multiAimConstraintsList;
    [Range(1, 30)]
    public float normalSensitivity = 3f;
    [Range(1, 10)]
    public float aimSensitivity = 2f;
    public float aimDuration = 0.3f;

    public GameObject PlayerNeck;
    public Vector2 VerticalClamp = new Vector2(-8, 30);
    public Vector2 HorizontalClamp = new Vector2(-30, 30);

    //public GameObject PlayerHand;
    [SerializeField]
    List<Weapon> weaponList;

    //Animation ID
    int m_animIDFire;
    int m_animIDZoom;
    int m_animIDHasWeapon;

    ThirdPersonController thirdPersonController;
    RigBuilder rigController;
    CustomInput input;
    Animator animator;
    Vector2 screenCenterPoint;

    int m_currentWeapon = 0;

    bool m_prevHasWeapon = false;
    bool m_hasWeapon = false;

    void Awake()
    {
        input = GetComponent<CustomInput>();
        animator = GetComponent<Animator>();
        rigController = GetComponent<RigBuilder>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AssignAnimationIDs();
    }

    void Update()
    {
        //m_hasWeapon = weapon != null;
        //if (m_hasWeapon != m_prevHasWeapon)
        //    animator.SetBool(m_animIDHasWeapon, m_hasWeapon);
        //m_prevHasWeapon = m_hasWeapon;
        Zoom(GetMousePosition());
        Fire();
    }

    void AssignAnimationIDs()
    {
        m_animIDFire = Animator.StringToHash("Fire");
        m_animIDZoom = Animator.StringToHash("Zoom");
        m_animIDHasWeapon = Animator.StringToHash("HasWeapon");
    }

    void Zoom(Vector3 mousePosition)
    {
        // if Player do zoom
        if (input.mouseR)
        {
            CrossHair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetFloat(m_animIDZoom, 1f, 0.5f, Time.deltaTime);
            rigController.layers[0].rig.weight += Time.deltaTime / aimDuration;
            rigController.layers[2].rig.weight += Time.deltaTime / aimDuration;

            mousePosition.y = transform.position.y;
            Vector3 aimDirection = (mousePosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            // Player Neck
            //m_neckX = (m_cinemachineTargetYaw < HorizontalClamp.x - 180 || m_cinemachineTargetYaw > HorizontalClamp.y + 180) ? -(m_cinemachineTargetYaw - transform.eulerAngles.y) : m_cinemachineTargetYaw - transform.eulerAngles.y;
            //PlayerNeck.transform.localRotation = Quaternion.Euler(-Mathf.Clamp(m_neckX, HorizontalClamp.x, HorizontalClamp.y), 0.0f, Mathf.Clamp(m_cinemachineTargetPitch, VerticalClamp.x, VerticalClamp.y));
            
            CrossHair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetFloat(m_animIDZoom, 0f, 0.5f, Time.deltaTime);
            rigController.layers[0].rig.weight -= Time.deltaTime / aimDuration;
            rigController.layers[2].rig.weight -= Time.deltaTime / aimDuration;
        }
    }

    void Fire()
    {
        if (input.mouseL)
        {
            animator.SetBool(m_animIDFire, true);
            if (weaponList.Count > 0)
                weaponList[m_currentWeapon].Fire();
        } else
            animator.SetBool(m_animIDFire, false);
    }

    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            targetTransform.position = raycastHit.point;
            return raycastHit.point;
        }
        return Vector3.zero;
    }
}
