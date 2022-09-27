using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    // ?????? ?????? ????
    public float waitingTime = 0.1f;
    public GameObject bulletPrefab;
    // ????
    public GameObject firePosition;
    public List<AudioClip> audioClips;

    bool m_canFire = true;
    float timer = 0.0f;
    AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (!m_canFire)
            return;
        
        GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        audioPlayer.clip = audioClips[0];
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
