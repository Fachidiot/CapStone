using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class livingEntity : MonoBehaviour  //livingEntity -> ����ִ� ��ü
{
    //EntityŬ������ ����,�÷��̾����� ���� ����Ŭ����

    public float startingHealth; //���� ü��
    public float health;  //���� ü��
    public bool dead;     //��� ����
    public event Action onDeath; //��� �� �ߵ��� �̺�Ʈ
    public Transform player;

    //����ü�� Ȱ��ȭ�� �� ���¸� ����
    public virtual void OnEnable() //OnEnable�� ȣ��Ǹ� ������  ex)gameObject.SetActive(true);
    {     
        dead = false ; //������� ���� ���·� ����
        health = startingHealth; //ü���� ���� ü������ �ʱ�ȭ    
    }

    //���ظ� �޴� ���
    public virtual void OnDamage(float damage) //virtual�� �� ������ ��� �޴� Ŭ�������� ������ �Ҷ�
    {
        //��������ŭ ü�� ����
        health -= damage; // health = health - damage;

        //ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    //��� ó��
    public virtual void Die()
    {      
        if (onDeath != null) onDeath(); //onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        dead = true;
        Destroy(gameObject);
    }

}
