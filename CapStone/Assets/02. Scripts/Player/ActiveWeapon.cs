using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public GameObject hitUI;
    public Transform Target;
    public Transform weaponParent;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Animator rigController;
    public bool isHasWeapon
    {
        get { return weapon; }
    }

    RaycastWeapon weapon;

    void Start()
    {
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        //rigController.updateMode = AnimatorUpdateMode.Normal;
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon) {
            Equip(existingWeapon);
        }
    }

    public void WeaponHolster(bool isHolstered)
    {
        rigController.SetBool("holster_weapon", !isHolstered);
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
        if (weapon.isFiring)
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
        weapon.hitUI = hitUI;
        rigController.Play("equip_" + weapon.weaponName);
    }
}
