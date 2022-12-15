using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        None = -1,
        Primary = 0,
        Secondary = 1
    }

    public Transform[] weaponSlots;

    public Transform leftHand;

    public Animator rigController;
    public WeaponAnimationEvents animationEvents;

    public Cinemachine.CinemachineFreeLook playerCamera;
    public WeaponSlot currentWeapon = WeaponSlot.None;

    public bool isNeedReload = false;

    [HideInInspector]
    public bool isHasWeapon
    {
        get { return equipped_weapons[0] || equipped_weapons[1]; }
    }

    RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    int activeWeaponIndex;
    GameObject magazineHand;

    // UIs
    Transform target;
    Image hitUI;
    TMP_Text ammoCountUI;
    TMP_Text maxAmmoCountUI;

    void Start()
    {
        target = GetComponent<ThirdPersonShooterController>().targetTransform;
        var playerUIManager = FindObjectOfType<PlayerUIManager>();
        hitUI = playerUIManager.UI_Hit;
        ammoCountUI = playerUIManager.UI_AmmoCount;
        maxAmmoCountUI = playerUIManager.UI_MaxAmmoCount;

        currentWeapon = WeaponSlot.None;
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        //rigController.updateMode = AnimatorUpdateMode.Normal;

        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    RaycastWeapon GetCurrentWeapon()
    {
        // Debug.Log(activeWeaponIndex);
        return GetWeapon(activeWeaponIndex);
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
        if (weapon.isReload)
            return;

        weapon.StartFiring();
        if (weapon.isFiring)
            weapon.UpdateFiring(Time.deltaTime);
        UpdateUI();
        weapon.UpdateBullets(Time.deltaTime);
    }

    void UpdateUI()
    {
        var weapon = GetCurrentWeapon();
        if (weapon == null)
            return;

        if (weapon.ammoCount < weapon.maxAmmoCount / 5)
            isNeedReload = true;
        else
            isNeedReload = false;

        ammoCountUI.GetComponent<TextMeshProUGUI>().text = weapon.ammoCount.ToString();
        maxAmmoCountUI.GetComponent<TextMeshProUGUI>().text = weapon.maxAmmoCount.ToString();
    }

    public void StopFire()
    {
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

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        currentWeapon = newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
            Destroy(weapon.gameObject);

        weapon = newWeapon;
        weapon.raycastDestination = target;
        weapon.recoil.playerCamera = playerCamera;
        weapon.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        weapon.hitUI = hitUI;

        equipped_weapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);
        currentWeapon = newWeapon.weaponSlot;
        UpdateUI();
    }

    public void SwitchWeapon(int weaponSlot)
    {
        if (!isHasWeapon)
            return;
        switch (weaponSlot)
        {
            case 0:
                currentWeapon = WeaponSlot.Primary;
                SetActiveWeapon(currentWeapon);
                return;
            case 1:
                currentWeapon = WeaponSlot.Secondary;
                SetActiveWeapon(currentWeapon);
                return;
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;
        activeWeaponIndex = (int)weaponSlot;

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
            UpdateUI();
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    public void OnAnimationEvent(string eventName)
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attatch_magazine":
                AttatchMagazine();
                break;
            case "goodto_fire":
                GoodToFire();
                break;
        }
    }

    private void DetachMagazine()
    {
        RaycastWeapon weapon = GetCurrentWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }

    private void DropMagazine()
    {
        GameObject dropMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        dropMagazine.AddComponent<Rigidbody>();
        dropMagazine.AddComponent<BoxCollider>();
        Destroy(dropMagazine, 10f);
        magazineHand.SetActive(false);
    }

    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }

    private void AttatchMagazine()
    {
        RaycastWeapon weapon = GetCurrentWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        rigController.SetBool("holster_weapon", true);
    }

    private void GoodToFire()
    {
        RaycastWeapon weapon = GetCurrentWeapon();
        weapon.ammoCount = weapon.maxAmmoCount;
        weapon.isReload = false;
        UpdateUI();
    }
}
