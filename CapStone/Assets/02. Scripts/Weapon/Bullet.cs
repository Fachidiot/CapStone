using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed = 20f; // 이동 속도
    Rigidbody m_rigid;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.velocity = transform.forward * speed;
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.tag == "Monster")
        {

        }    
    }
}
