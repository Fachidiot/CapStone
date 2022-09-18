using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed = 10; // 이동 속도
    Rigidbody m_rigid;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.velocity = transform.forward * speed;
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {

        }    
    }
}
