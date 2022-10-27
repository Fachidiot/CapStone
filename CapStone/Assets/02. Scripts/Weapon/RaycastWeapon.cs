using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaycastWeapon : MonoBehaviour
{
    class RaycastBullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }

    public ActiveWeapon.WeaponSlot weaponSlot;
    [HideInInspector] public bool isFiring = false;
    [HideInInspector] public Animator rigController;
    [SerializeField]
    LayerMask bulletColliderLayerMask = new LayerMask();
    [Header("Weapon Informations")]
    public int fireRate = 25;
    public float bulletPower = 30f;
    public float bulletDistance = 100.0f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public int maxBounces = 0;
    public string weaponName;

    int ammoCount;
    public int maxAmmoCount;
    [Header("Recoil System")]
    [Range(0, 10f)] public float maxRecoilTime;
    [Range(0, 7f)] public float recoilAmountVertical;
    [Range(0, 3f)] public float recoilAmountHorizontal;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public TrailRenderer tracerEffect;
    public AudioSource fireSound;
    public bool loopSound;

    [Header("Raycast")]
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public GameObject magazine;

    [Header("UI Radio")]
    public int hitLayer;
    public GameObject ammoCountUI;
    public GameObject hitUI;

    Ray ray;
    RaycastHit hitInfo;
    List<RaycastBullet> bullets = new List<RaycastBullet>();
    ThirdPersonShooterController m_tpsController;
    float m_accumulatedTime;
    float m_maxLifeTime = 5.0f;
    Vector2 m_currentRecoilPos;
    public float m_timePressed;

    void Awake()
    {
        ammoCount = maxAmmoCount;
    }

    void Start()
    {
        if (ammoCountUI)
            ammoCountUI.GetComponent<TextMeshProUGUI>().text = ammoCount + " / " + maxAmmoCount;
    }

    public void Fire()
    {
        StartFiring();
        if (isFiring)
            UpdateFiring(Time.deltaTime);
        UpdateBullets(Time.deltaTime);
    }

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
        bullet.bounce = maxBounces;
        return bullet;
    }

    void StartFiring()
    {
        if (isFiring && m_accumulatedTime < 0.0f || ammoCount <= 0)
            return;

        if (loopSound)
            fireSound.Play();
        isFiring = true;
        m_accumulatedTime = 0.0f;
        //if (lastm_accumulatedTime < 0)
        //    m_accumulatedTime = lastm_accumulatedTime;
    }

    void UpdateFiring(float deltaTime)
    {
        if (ammoCount <= 0)
            return;
        m_accumulatedTime += deltaTime;
        m_timePressed += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (m_accumulatedTime >= 0.0f)
        {
            if (!loopSound)
                fireSound.Play();
            FireBullet();
            m_accumulatedTime -= fireInterval;
        }

        ammoCountUI.GetComponent<TextMeshProUGUI>().text = ammoCount + " / " + maxAmmoCount;
    }

    void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    public void Reload()
    {
        if (ammoCount != maxAmmoCount)
        {
            rigController.SetTrigger("reload_weapon");

            ammoCount = maxAmmoCount;
            ammoCountUI.GetComponent<TextMeshProUGUI>().text = ammoCount + " / " + maxAmmoCount;
        }
    }

    void RecoilMath()
    {
        m_currentRecoilPos = new Vector2(
            ((Random.value - 0.5f) / 2) * recoilAmountHorizontal,
            ((Random.value - 0.5f) / 2) * (m_timePressed >= maxRecoilTime ? recoilAmountVertical / 4 : recoilAmountVertical));
        // m_tpsController.recoilCameraYRotation -= Mathf.Abs(m_currentRecoilPos.y);
        // m_tpsController.recoilCameraXRotation -= m_currentRecoilPos.x;

        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
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
        bullets.RemoveAll(bullet => bullet.time >= m_maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, RaycastBullet bullet)
    {
        Vector3 direction = end - start;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, bulletDistance, bulletColliderLayerMask))
        {
            GameObject effect = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            effect.transform.parent = hitInfo.transform;
            if (hitInfo.rigidbody != null)
            {
                if (hitInfo.rigidbody.gameObject.layer == hitLayer)
                    hitUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                hitInfo.rigidbody.AddForce(-hitInfo.normal * bulletPower);
            }
            Destroy(effect, 3f);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = m_maxLifeTime;

            if (bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPosition = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
        }
        else
        {
            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = m_maxLifeTime;
        }
    }

    void FireBullet()
    {
        muzzleFlash.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        RecoilMath();

        bullets.Add(bullet);
        ammoCount--;
    }

    void OnReload(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            ammoCount = maxAmmoCount;
            ammoCountUI.GetComponent<TextMeshProUGUI>().text = ammoCount + " / " + maxAmmoCount;
        }
    }

    public void StopFiring()
    {
        StartCoroutine(StopFire());
    }

    IEnumerator StopFire()
    {
        yield return new WaitForSeconds(-m_accumulatedTime);

        isFiring = false;
        m_timePressed = 0;
        StopAllCoroutines();

        if (loopSound)
            fireSound.Stop();
    }
}
