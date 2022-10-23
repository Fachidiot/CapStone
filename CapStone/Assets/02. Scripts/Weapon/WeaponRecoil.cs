using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Cinemachine.CinemachineFreeLook playerCamera;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShake;
    public float verticalRecoil;
    public float duration;

    float time;

    void Awake()
    {
        cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public void GenerateRecoil()
    {
        time = duration;

        cameraShake.GenerateImpulse(Camera.main.transform.forward);
    }

    void Update()
    {
        if (time > 0)
        {
            playerCamera.m_YAxis.Value -= ((verticalRecoil / 1000) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
        
    }
}
