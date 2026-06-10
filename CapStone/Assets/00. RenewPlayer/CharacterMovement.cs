using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterState
{
    None = 0,
    Stand,
    Crouch,
    Walk,
    Sprint,
    Jump,
    Roll,
    InAir
}

public class CharacterMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform directionOrienter;

    [Header("Collider values")]
    [SerializeField] private float crouchColliderHeight = 1f;
    private float normalColliderHeight;
    [SerializeField] private float groundCheckDistance;

    private bool _isGrounded;
    public bool isGrounded
    {
        get => _isGrounded;
        set
        {
            if (value == _isGrounded)
                return;
            _isGrounded = value;
            if (!_isGrounded && currentState != CharacterState.InAir)
                currentState = CharacterState.InAir;

            animator.SetBool("isGrounded", value);
            OnGroundedValueChange.Invoke(value);
        }
    }
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private delegate void isGroundedChange(bool changed);
    [SerializeField] private event isGroundedChange OnGroundedValueChange;
    [SerializeField] private float edgeFallMoveForce = 1f;
    [SerializeField] private float nonSlipDistance = 0.1f;

    [Header("Move value")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float walkSpeed = 2;
    [SerializeField] private float sprintSpeed = 3;
    [SerializeField] private float crouchSpeed = 1;
    [SerializeField] private float jumpHeight = 1;

    // Velocity values
    private Vector3 moveVelocity;
    private Vector3 velocity;
    private Vector3 rollVelocity;
    private Vector3 edgeSlipVelocity;

    private CharacterState currentState;

    // animator ids
    private int horizontalInputID;
    private int verticalInputID;
    private int walkID;
    private int sprintID;
    private int crouchID;
    private int isGroundID;
    private int rollID;

    IEnumerator colliderSizeChangeCor;

    private void Awake()
    {
        AssignAnimatorIDs();
        colliderSizeChangeCor = ColliderSizeChangeSmooth(false);
        characterController = GetComponent<CharacterController>();
        normalColliderHeight = characterController.height;
    }

    private void Start()
    {
        currentState = CharacterState.Stand;
    }

    private void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        RaycastHit hitInfo;

        if (velocity.y <= 0 && Physics.SphereCast(transform.position + characterController.center, characterController.radius + characterController.skinWidth, Vector3.down, out hitInfo, groundCheckDistance, groundCheckMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            Vector3 relativeHitPoint = hitInfo.point - (transform.position + Vector3.right * characterController.center.x + Vector3.forward * characterController.center.z);

            Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.up * 0.1f + Vector3.down * 0.3f, Color.red);

            if (characterController.velocity.y < 0 && relativeHitPoint.magnitude > nonSlipDistance && !Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f, groundCheckMask))
            {
                Vector3 edgeFallMovement = transform.position - hitInfo.point;
                edgeFallMovement.y = 0;
                edgeSlipVelocity += (edgeFallMovement * Time.deltaTime * edgeFallMoveForce);
            }
            else
            {
                edgeSlipVelocity = Vector3.zero;
            }
        }
        else
        {
            isGrounded = false;
            edgeSlipVelocity = Vector3.zero;
        }
    }

    // Crouch or Stand상태의 Collider 변환
    IEnumerator ColliderSizeChangeSmooth(bool reduce)
    {
        var startSize = characterController.height;
        var finalSize = reduce ? crouchColliderHeight : normalColliderHeight;
        var startCenter = characterController.center.y;
        var finalCenter = finalSize / 2f;
        float reduceTime = 0;

        while (reduceTime < 0.3f)
        {
            characterController.height = Mathf.Lerp(startSize, finalSize, reduceTime / 0.3f);
            characterController.center = new Vector3(characterController.center.x, Mathf.Lerp(startCenter, finalCenter, reduceTime / 0.3f), characterController.center.z);
            reduceTime += Time.deltaTime;
            yield return null;
        }
        characterController.height = finalSize;
        yield break;
    }

    private void AssignAnimatorIDs()
    {
        horizontalInputID = Animator.StringToHash("x");
        verticalInputID = Animator.StringToHash("y");
        isGroundID = Animator.StringToHash("isGround");
        walkID = Animator.StringToHash("walk");
        sprintID = Animator.StringToHash("sprint");
        rollID = Animator.StringToHash("roll");
        crouchID = Animator.StringToHash("crouch");
    }
}