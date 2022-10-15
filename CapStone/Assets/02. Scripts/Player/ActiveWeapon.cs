using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public Transform Target;
    public Transform weaponParent;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    Animator animator;
    public bool isHasWeapon
    {
        get { return weapon; }
    }

    RaycastWeapon weapon;
    AnimatorOverrideController overrideController;

    void Start()
    {
        animator = GetComponent<Animator>();
        overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon) {
            Equip(existingWeapon);
        }
    }

    void Update()
    {
        if (!weapon)
        {
            handIK.weight = 0.0f;
            animator.SetLayerWeight(1, 0.0f);
        }
    }

    public void Fire()
    {
        if (weapon) {
            weapon.StartFiring();
            if (weapon.isFiring) {
                weapon.UpdateFiring(Time.deltaTime);
            }
            weapon.UpdateBullets(Time.deltaTime);
        }
    }

    public void StopFire() {
        weapon.StopFiring();
    }

    public void Equip(RaycastWeapon newWeapon) {
        if (weapon)
            Destroy(weapon.gameObject);

        weapon = newWeapon;
        weapon.raycastDestination = Target;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        handIK.weight = 1.0f;
        animator.SetLayerWeight(1, 1.0f);
        Invoke(nameof(SetAnimationDelayed), 0.001f);
    }

    void SetAnimationDelayed()
    {
        overrideController["weapon_anim_Empty"] = weapon.WeaponAnimation;
    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.WeaponAnimation);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}
