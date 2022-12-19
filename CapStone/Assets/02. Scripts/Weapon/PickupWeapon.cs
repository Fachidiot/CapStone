using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public RaycastWeapon WeaponPrefab;
    public Transform DisplayWeapon;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon WeaponManager = other.gameObject.GetComponent<ActiveWeapon>();
        if (WeaponManager) {
            RaycastWeapon newWeapon = Instantiate(WeaponPrefab);
            WeaponManager.Equip(newWeapon);
            Destroy(gameObject);
        }
        
        HitBox hitBox = other.gameObject.GetComponent<HitBox>();
        if (hitBox) {
            AIWeapons weapons = hitBox.health.GetComponent<AIWeapons>();
            RaycastWeapon newWeapon = Instantiate(WeaponPrefab);
            weapons.Equip(newWeapon);
            Destroy(gameObject);
        }
    }
}
