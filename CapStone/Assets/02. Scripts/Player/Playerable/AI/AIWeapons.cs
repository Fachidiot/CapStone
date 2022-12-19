using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    Animator rigManager;
    public Transform[] weaponSlots;
    public Transform leftHand;
    public WeaponAnimationEvents animationEvents;
    Transform currentTarget;
    public Vector3 targetOffset;
    public bool isNeedReload = false;
    public float inaccuracy = 0.0f;

    [SerializeField] RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    int activeWeaponIndex;
    GameObject magazineHand;
    // AIAgent agent;
    bool weaponActive = false;
    WeaponIK weaponIk;
    MeshSockets sockets;

    [HideInInspector]
    public bool isHasWeapon
    {
        get { return equipped_weapons[0] || equipped_weapons[1]; }
    }
    // public ActiveWeapon.WeaponSlot currentWeapon = ActiveWeapon.WeaponSlot.None;

    void Awake() {
        rigManager = GetComponentInChildren<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIk = GetComponent<WeaponIK>();
    }

    void Update() {
        if (currentTarget && currentWeapon && weaponActive) {
            Vector3 target = currentTarget.position + targetOffset;
            target += UnityEngine.Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enabled) {
        if (enabled) {
            currentWeapon.StartFiring();
        } else {
            currentWeapon.StopFiring();
        }
    }

    public void Equip(RaycastWeapon weapon) {
        currentWeapon = weapon;
        currentWeapon.transform.SetParent(transform, false);
        // sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine);
    }

    internal void ActivateWeapon()
    {
        StartCoroutine(EquipWeapon());
    }

    IEnumerator EquipWeapon() {
        rigManager.SetInteger("WeaponSlot", (int)currentWeapon.weaponSlot);
        rigManager.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while(rigManager.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f) {
            yield return null;
        }

        weaponIk.SetAimTransform(currentWeapon.raycastOrigin);
        weaponActive = true;
    }

    public void DropWeapon() {
        if (currentWeapon) {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<MeshCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void OnAnimationEvent(string eventName) {
        if (eventName == "equipWeapon") {

        }
    }

    public void SetTarget(Transform target)
    {
        weaponIk.SetTargetTransform(target);
        currentTarget = target;
    }

    // void Start() {
    //     agent = GetComponent<AIAgent>();
    //     rigManager = GetComponentInChildren<Animator>();
    //     currentRaycastWeapon = equipped_weapons[0];
    //     currentTarget = agent.playerTransform;
    // }

    // RaycastWeapon GetCurrentWeapon()
    // {
    //     // Debug.Log(activeWeaponIndex);
    //     return GetWeapon(activeWeaponIndex);
    // }

    // RaycastWeapon GetWeapon(int index)
    // {
    //     if (index < 0 || index >= equipped_weapons.Length)
    //         return null;
    //     return equipped_weapons[index];
    // }

    // public void WeaponHolster(bool isHolstered)
    // {
    //     rigManager.SetBool("holster_weapon", !isHolstered);
    // }

    // public void Fire()
    // {
    //     var weapon = GetWeapon(activeWeaponIndex);
    //     if (weapon.isReload)
    //         return;

    //     weapon.StartFiring();
    //     if (weapon.isFiring)
    //         weapon.UpdateFiring(Time.deltaTime, currentTarget.position);
    //     weapon.UpdateBullets(Time.deltaTime);
    // }

    // public void StopFire()
    // {
    //     var weapon = GetWeapon(activeWeaponIndex);
    //     if (!weapon)
    //         return;
    //     if (weapon.isFiring)
    //         weapon.StopFiring();
    // }

    // public void Reload()
    // {
    //     var weapon = GetWeapon(activeWeaponIndex);
    //     if (!weapon)
    //         return;
    //     weapon.Reload();
    // }

    // public void SwitchWeapon(int weaponSlot)
    // {
    //     if (!isHasWeapon)
    //         return;
    //     switch (weaponSlot)
    //     {
    //         case 0:
    //             currentWeapon = ActiveWeapon.WeaponSlot.Primary;
    //             SetActiveWeapon(currentWeapon);
    //             return;
    //         case 1:
    //             currentWeapon = ActiveWeapon.WeaponSlot.Secondary;
    //             SetActiveWeapon(currentWeapon);
    //             return;
    //     }
    // }

    // void SetActiveWeapon(ActiveWeapon.WeaponSlot weaponSlot)
    // {
    //     int holsterIndex = activeWeaponIndex;
    //     int activateIndex = (int)weaponSlot;
    //     activeWeaponIndex = (int)weaponSlot;

    //     if (holsterIndex == activateIndex)
    //         holsterIndex = -1;

    //     StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    // }

    // IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    // {
    //     yield return StartCoroutine(HolsterWeapon(holsterIndex));
    //     yield return StartCoroutine(ActivateWeapon(activateIndex));
    //     activeWeaponIndex = activateIndex;
    // }

    // IEnumerator HolsterWeapon(int index)
    // {
    //     var weapon = GetWeapon(index);
    //     if (weapon)
    //     {
    //         rigManager.SetBool("holster_weapon", true);
    //         do
    //         {
    //             yield return new WaitForEndOfFrame();
    //         } while (rigManager.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
    //     }
    // }

    // IEnumerator ActivateWeapon(int index)
    // {
    //     var weapon = GetWeapon(index);
    //     if (weapon)
    //     {
    //         rigManager.SetBool("holster_weapon", false);
    //         rigManager.Play("equip_" + weapon.weaponName);
    //         do
    //         {
    //             yield return new WaitForEndOfFrame();
    //         } while (rigManager.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
    //     }
    // }

    // public void OnAnimationEvent(string eventName)
    // {
    //     // Debug.Log(eventName);
    //     switch (eventName)
    //     {
    //         case "detach_magazine":
    //             DetachMagazine();
    //             break;
    //         case "drop_magazine":
    //             DropMagazine();
    //             break;
    //         case "refill_magazine":
    //             RefillMagazine();
    //             break;
    //         case "attatch_magazine":
    //             AttatchMagazine();
    //             break;
    //     }
    // }

    // private void DetachMagazine()
    // {
    //     RaycastWeapon weapon = GetCurrentWeapon();
    //     magazineHand = Instantiate(weapon.magazine, leftHand, true);
    //     weapon.magazine.SetActive(false);
    // }

    // private void DropMagazine()
    // {
    //     GameObject dropMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
    //     dropMagazine.AddComponent<Rigidbody>();
    //     dropMagazine.AddComponent<BoxCollider>();
    //     Destroy(dropMagazine, 10f);
    //     magazineHand.SetActive(false);
    // }

    // private void RefillMagazine()
    // {
    //     magazineHand.SetActive(true);
    // }

    // private void AttatchMagazine()
    // {
    //     RaycastWeapon weapon = GetCurrentWeapon();
    //     weapon.magazine.SetActive(true);
    //     Destroy(magazineHand);
    //     rigManager.SetBool("holster_weapon", true);
    // }

    // public bool HasWeapon() {
    //     return currentRaycastWeapon != null;
    // }
}
