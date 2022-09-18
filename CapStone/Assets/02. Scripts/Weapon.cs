using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    // 총알을 생산할 공장
    public float waitingTime = 0.1f;
    public GameObject bulletPrefab;
    // 총구
    public GameObject firePosition;

    bool m_canFire = true;
    float timer = 0.0f;

    public void Fire()
    {
        if (!m_canFire)
            return;
        // 2. 총알 공장에서 총알을 만든다.
        GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        m_canFire = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime)
        {
            //Action
            timer = 0;
            m_canFire = true;
        }
    }
}
