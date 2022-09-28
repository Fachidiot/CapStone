using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiring = false;

    public void StartFiring()
    {
        isFiring = true;
    }

    // Update is called once per frame
    public void StopFiring()
    {
        isFiring = false;
    }
}
