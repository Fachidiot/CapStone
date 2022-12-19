using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] MultiAimConstraint[] targetList;

    [Header("Status")]
    public bool hasWeapon = true;
    public bool Grounded = true;
    
    [Header("AI Grounded")]
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Space(10)]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [SerializeField] Animator animator;
    NavMeshAgent agent;

    private void GroundedCheck()
    {
        // 발밑에 생성할 구체 위치값
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        animator.SetBool("Grounded", Grounded);
        // 만약 플레이어가 무한점프를 한다면 플레이어의 Layer와 GroundLayer 충돌 확인
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetFloat("MotionSpeed", 1f);

        foreach (var component in targetList)
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, GameObject.FindGameObjectWithTag("Player").transform);
            component.data.sourceObjects = data;
        }
    }

    void FixedUpdate()
    {
        GroundedCheck();
        animator.SetFloat("Speed", agent.velocity.magnitude * 5);
    }

    void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(transform.position), FootstepAudioVolume);
            }
        }
    }

    void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(transform.position), FootstepAudioVolume);
        }
    }
}
