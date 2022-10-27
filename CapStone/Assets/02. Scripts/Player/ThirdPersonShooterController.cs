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
    [SerializeField]
    ActiveWeapon WeaponManager;
    [HideInInspector] public float recoilCameraXRotation;
    [HideInInspector] public float recoilCameraYRotation;
    float m_currentCameraXRotation;
    float m_currentCameraYRotation;

    bool aimed = false;

    //Animation ID
    int m_animIDZoom;
    int m_animIDHorizontal;
    int m_animIDVertical;

    ThirdPersonController thirdPersonController;
    RigBuilder rigController;
    CustomInput input;
    Animator animator;
    Vector2 screenCenterPoint;

    void Awake()
    {
        input = GetComponent<CustomInput>();
        animator = GetComponent<Animator>();
        rigController = GetComponent<RigBuilder>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void AssignAnimationIDs()
    {
        //m_animIDFire = Animator.StringToHash("Fire");
        //m_animIDHasWeapon = Animator.StringToHash("HasWeapon");
        m_animIDZoom = Animator.StringToHash("Aimed");
        m_animIDHorizontal = Animator.StringToHash("Horizontal");
        m_animIDVertical = Animator.StringToHash("Vertical");
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AssignAnimationIDs();
    }

    void FixedUpdate()
    {
        MoveAnimation();
    }

    void Update()
    {
        SwitchWeapon();
        Reload();
    }

    private void LateUpdate()
    {
        Zoom(GetMousePosition());
        Fire();
    }

    void MoveAnimation()
    {
        animator.SetBool(m_animIDZoom, aimed);
        if (!aimed)
            return;
        animator.SetFloat(m_animIDHorizontal, input.move.x, 0.1f, Time.deltaTime);
        animator.SetFloat(m_animIDVertical, input.move.y, 0.1f, Time.deltaTime);
    }

    void Zoom(Vector3 mousePosition)
    {
        // Do Recoil
        // m_currentCameraYRotation = Mathf.SmoothDamp(m_currentCameraYRotation, recoilCameraYRotation, ref rotationYVelocity, yRotationSpeed);
        // m_currentCameraXRotation = Mathf.SmoothDamp(m_currentCameraXRotation, recoilCameraXRotation, ref rotationXVelocity, xRotationSpeed);

        // transform.rotation = Quaternion.Euler(0, m_currentCameraYRotation, 0);
        // camera.localRotation = Quaternion.Euler(m_currentCameraXRotation, 0, zRotation);

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

    void Fire()
    {
        if (!WeaponManager.isHasWeapon)
            return;
        if (input.mouseL)
        {
            if (!input.mouseR)
                return;
            WeaponManager.Fire();
        }
        else
        {
            WeaponManager.StopFire();
        }
    }

    void Reload()
    {
        if (input.reload)
        {
            WeaponManager.Reload();
            input.reload = false;
        }
    }

    void SwitchWeapon()
    {
        if (input.alpha1)
        {
            WeaponManager.SwitchWeapon(0);
            input.alpha1 = false;
        }
        if (input.alpha2)
        {
            WeaponManager.SwitchWeapon(1);
            input.alpha2 = false;
        }
    }

    void Recoiling()
    {
        if (!WeaponManager.isHasWeapon)
            return;
    }

    void SkillSystem()
    {
        // Dash
        if (input.crouch)
        {
            //m_dashVelocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime)));

            //gravity
            // m_dashVelocity.y += Gravity * Time.deltaTime;

            // //dash ground drags
            // m_dashVelocity.x /= 1 + Time.deltaTime * 10;
            // m_dashVelocity.y /= 1 + Time.deltaTime * 10;
            // m_dashVelocity.z /= 1 + Time.deltaTime * 10;

            // controller.Move(m_dashVelocity * Time.deltaTime);
            input.crouch = false;
        }

        // Skill1
        if (input.skill1)
        {
            // 스킬 스크립트
            // 스킬 생성
            input.skill1 = false;
        }

        if (input.skill2)
        {
            // 스킬 스크립트
            // 스킬 생성
            input.skill2 = false;
        }

        if (input.ultimate)
        {
            // 궁극기 스크립트
            // 궁극기 생성
            input.ultimate = false;
        }

        if (input.reload)
        {
            // 장전 스크립트
            // 해당 총기 장전
            input.reload = false;
        }
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
