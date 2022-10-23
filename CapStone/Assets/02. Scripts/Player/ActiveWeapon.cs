using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }
    public GameObject hitUI;
    public GameObject ammoCountUI;
    public Transform Target;
    public Transform[] weaponSlots;
    public Cinemachine.CinemachineFreeLook playerCamera;

    public Animator rigController;
    public bool isHasWeapon
    {
        get { return equipped_weapons[0] || equipped_weapons[1]; }
    }

    RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    int activeWeaponIndex;

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

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_weapons.Length)
            return null;
        return equipped_weapons[index];
    }

    public void WeaponHolster(bool isHolstered)
    {
        rigController.SetBool("holster_weapon", !isHolstered);
    }

    public void Fire()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        if (weapon) {
            weapon.StartFiring();
            if (weapon.isFiring) {
                weapon.UpdateFiring(Time.deltaTime);
            }
            weapon.UpdateBullets(Time.deltaTime);
        }
    }

    public void StopFire() {
        var weapon = GetWeapon(activeWeaponIndex);
        if (!weapon)
            return;
        if (weapon.isFiring)
            weapon.StopFiring();
    }

    public void Reload()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        if (!weapon)
            return;
        weapon.Reload();
    }

    public void Equip(RaycastWeapon newWeapon) {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
            Destroy(weapon.gameObject);

        weapon = newWeapon;
        weapon.raycastDestination = Target;
        weapon.recoil.playerCamera = playerCamera;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        weapon.hitUI = hitUI;
        weapon.ammoCountUI = ammoCountUI;

        equipped_weapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);
    }

    public void SwitchWeapon(int weaponSlot)
    {
        switch (weaponSlot)
        {
            case 0:
                SetActiveWeapon(WeaponSlot.Primary);
                return;
            case 1:
                SetActiveWeapon(WeaponSlot.Secondary);
                return;
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if (holsterIndex == activateIndex)
            holsterIndex = -1;

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
}
