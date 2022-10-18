using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastWeapon : MonoBehaviour
{
    class RaycastBullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public bool isFiring = false;
    public int fireRate = 25;
    public float bulletPower = 30f;
    public float bulletDistance = 100.0f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public string weaponName;
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public TrailRenderer tracerEffect;
    [Header("Raycast")]
    public Transform raycastOrigin;
    public Transform raycastDestination;
    [Header("UI Radio")]
    public int hitLayer;
    public GameObject hitUI;

    Ray ray;
    RaycastHit hitInfo;
    [SerializeField]
    float accumulatedTime;
    List<RaycastBullet> bullets = new List<RaycastBullet>();
    float maxLifeTime = 5.0f;

    Vector3 GetPosition(RaycastBullet bullet)
    {
        // p + v*t + 0.5 * g * t * t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    RaycastBullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        RaycastBullet bullet = new RaycastBullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        if (isFiring && accumulatedTime < 0.0f)
            return;

        isFiring = true;
        accumulatedTime = 0.0f;
        //if (lastAccumulatedTime < 0)
        //    accumulatedTime = lastAccumulatedTime;
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, RaycastBullet bullet)
    {
        Vector3 direction = end - start;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, bulletDistance))
        {
            GameObject obj = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            obj.transform.parent = hitInfo.transform;
            //Debug.Log(hitInfo.transform.name);
            if (hitInfo.rigidbody != null)
            {
                if (hitInfo.rigidbody.gameObject.layer == hitLayer)
                    hitUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                hitInfo.rigidbody.AddForce(-hitInfo.normal * bulletPower);
            }
            Destroy(obj, 3f);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
        } else {
            bullet.tracer.transform.position = end;
            bullet.time = maxLifeTime;
        }
    }

    void FireBullet()
    {
        muzzleFlash.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    public void StopFiring()
    {
        StartCoroutine(StopFire());
    }

    IEnumerator StopFire()
    {
        yield return new WaitForSeconds(-accumulatedTime);

        isFiring = false;
        StopAllCoroutines();
    }
}
