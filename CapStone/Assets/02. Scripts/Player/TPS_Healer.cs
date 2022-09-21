using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Healer : ThirdPersonController
{
    [Header("Weapon")]
    //public Weapon weapon;

    // animation IDs
    int m_animIDFire;
    int m_animIDZoom;
    int m_animIDHasWeapon;

    void Start()
    {
        AssignAnimationIDs();
    }

    void AssignAnimationIDs()
    {
        m_animIDFire = Animator.StringToHash("Fire");
        m_animIDHasWeapon = Animator.StringToHash("HasWeapon");
        m_animIDZoom = Animator.StringToHash("Zoom");
    }

    protected override void MouseClick()
    {
        if (m_hasWeapon)
        {
            animator.SetFloat(m_animIDZoom, input.mouseR ? 1 : 0);
            animator.SetBool(m_animIDFire, input.mouseL);
        }
        // Zoom
        if (input.mouseR)
        {

        }

        // Attack Combo
        if (input.mouseL)
        {
            animator.SetFloat(m_animIDZoom, 1);
            weapon.Fire();
            // if (m_hasWeapon)
            // {
            // 
            // }
            // else
            // {
            // 
            // }
        }
    }

    protected override void SkillSystem()
    {
        // Dash
        if (input.crouch)
        {
            m_dashVelocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime)));

            //gravity
            m_dashVelocity.y += Gravity * Time.deltaTime;

            //dash ground drags
            m_dashVelocity.x /= 1 + Time.deltaTime * 10;
            m_dashVelocity.y /= 1 + Time.deltaTime * 10;
            m_dashVelocity.z /= 1 + Time.deltaTime * 10;

            controller.Move(m_dashVelocity * Time.deltaTime);
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
}
