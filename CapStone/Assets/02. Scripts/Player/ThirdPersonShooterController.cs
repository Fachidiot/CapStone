using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    GameObject aimVirtualCamera;
    [SerializeField]
    LayerMask aimColliderLayerMask = new LayerMask();
    [HideInInspector] public Transform targetTransform;
    [Range(1, 30)]
    public float normalSensitivity = 3f;
    [Range(1, 10)]
    public float aimSensitivity = 2f;
    public float aimDuration = 0.3f;
    [SerializeField]
    protected ActiveWeapon WeaponManager;
    [SerializeField]
    MultiAimConstraint[] targetList;

    [SerializeField]
    float normalFOV = 40;
    [SerializeField]
    float aimedFOV = 40;
    [SerializeField]
    Vector3 normalOffset = new Vector3(0, 0, 0);
    [SerializeField]
    Vector3 aimedOffset = new Vector3(0.5f, 0, 1);

    protected bool aimed = false;
    Image CrossHair;

    //Animation ID
    int m_animIDZoom;
    int m_animIDHorizontal;
    int m_animIDVertical;

    ThirdPersonController thirdPersonController;
    RigBuilder rigController;
    protected CustomInput input;
    protected Animator animator;
    Vector2 screenCenterPoint;

    void Awake()
    {
        if (targetTransform == null)
        {
            targetTransform = new GameObject().GetComponent<Transform>();
            targetTransform.gameObject.name = "Target";
        }
        foreach (MultiAimConstraint component in targetList)
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, targetTransform);
            component.data.sourceObjects = data;
        }
        RigBuilder rigs = GetComponent<RigBuilder>();
        rigs.Build();

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
        var playerUIManager = FindObjectOfType<PlayerUIManager>();
        CrossHair = playerUIManager.UI_CrossHair;
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
        var temp = GetMousePosition();
        Zoom(temp);
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

    protected virtual void Zoom(Vector3 mousePosition)
    {
        // if Player do zoom
        if (input.mouseR)
        {
            CrossHair.gameObject.SetActive(true);
            // aimVirtualCamera.gameObject.SetActive(true);
            aimVirtualCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView = aimedFOV;
            aimVirtualCamera.GetComponent<CinemachineCameraOffset>().m_Offset = aimedOffset;

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
            CrossHair.gameObject.SetActive(false);
            // aimVirtualCamera.gameObject.SetActive(false);
            aimVirtualCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView = normalFOV;
            aimVirtualCamera.GetComponent<CinemachineCameraOffset>().m_Offset = normalOffset;

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
        if (!WeaponManager.isHasWeapon)
            return;
        if (input.mouseR)
        {
            if (input.reload)
            {
                WeaponManager.Reload();
            }
        }

        input.reload = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {

        }
    }

    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, aimColliderLayerMask))
        {
            //Debug.Log(screenCenterPoint + " | " + raycastHit.point);
            targetTransform.position = raycastHit.point;
            return raycastHit.point;
        }
        return Vector3.zero;
    }

    void OnDestroy()
    {
        Destroy(targetTransform);
    }
}
