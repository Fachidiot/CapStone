
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWarrior : livingEntity
{
    public NavMeshAgent agent;

    private livingEntity targetEntity; //�������

    private Animator enemyAnimator;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint; //���� ����
    bool walkPointSet; //���� ���� Ȯ���� ����
    public float walkPointRange; //���� ���� ����
    public float timeAfterPatrol;


    //Attacking
    public float damage = 20f; //���ݷ�
    public float attackDelay = 1f; //���� ������
    private float lastAttackTime; //������ ���� ����

    //States
    public float sightRange, attackRange;  //�þ� ����, ���� ����
    public bool playerInSightRange, playerInAttackRange; //boolŸ�� �þ� ����, ���� ����

    //anima
    private bool canMove;
    private bool canAttack;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

    }

    private void Update()
    {

        findTarget(); //target ã��

        //�þ߿� ���� Ȯ��
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);  //�þ� ����
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //���� ����

        if (!playerInSightRange && !playerInAttackRange) //player �þ� ����, ���� ���� ���� ���� ���� ��� (����)
        {
            timeAfterPatrol += Time.deltaTime;
            if (timeAfterPatrol >= 2f) {
                Patroling();
                timeAfterPatrol = 0;
            }
        }
        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); //player �þ� �����ȿ� ������, ���� ���� ���� ���� ���� ��� (������ ����)
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); //player �þ� ����, ���� ���� ���� ���� (����)

        enemyAnimator.SetBool("CanMove", canMove);
        enemyAnimator.SetBool("CanAttack", canAttack);
    }


    private void Patroling()
    {
        if (!walkPointSet)
        {
            canMove = false;
            canAttack = false;
            SearchWalkPoint();
        }


        if (walkPointSet)
        {
            canMove = true;
            canAttack = false;
            agent.SetDestination(walkPoint);
        }


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            canMove = false;
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        // ���� �� ���� ���� ���
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) //���������� ������ �ִ��� �����ɽ�Ʈ�� Ȯ��
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        canMove = true;
        canAttack = false;

        agent.SetDestination(player.position);  //������ ����
    }

    #region attack
    private void AttackPlayer()
    {
        //�ڽ��� ���X
        if (!dead)
        {
            //���� �ݰ� �ȿ� ������ �������� �����.
            agent.SetDestination(transform.position); //�׺������ ����
            canMove = false;

            //���� ��� �ٶ󺸱�         
            this.transform.LookAt(player);

            //�ֱ� ���� �������� attackDelay �̻� �ð��� ������ ���� ����
            if (lastAttackTime + attackDelay <= Time.time)
            {
                canAttack = true;
                OnDamage();
            }

            //���� �ݰ� �ȿ� ������, �����̰� �������� ���
            else
            {
                canMove = false;
                canAttack = false;
            }
        }
    }



    //�������� �Ծ��� �� ������ ó��
    public override void OnDamage(float damage)
    {
        //�ǰ� �ִϸ��̼� ���
        enemyAnimator.SetTrigger("Hit");

        //livingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage);

        //ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (health <= 0 && !dead)
        {
            Die();
            enemyAnimator.SetTrigger("Death");
        }
    }

    //������ �ֱ�
    public void OnDamage()
    {
        livingEntity attackTarget = targetEntity.GetComponent<livingEntity>();

        attackTarget.OnDamage(damage);
        lastAttackTime = Time.time;
    }

    #endregion

    void findTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsPlayer);

        //��� �ݶ��̴��� ��ȸ�ϸ鼭 ��� �ִ� LivingEntity ã��
        for (int i = 0; i < colliders.Length; i++)
        {
            //�ݶ��̴��κ��� LivingEntity ������Ʈ ��������
            livingEntity livingEntity = colliders[i].GetComponent<livingEntity>();

            //LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ��� �ִٸ�
            if (livingEntity != null && !livingEntity.dead)
            {
                //���� ����� �ش� LivingEntity�� ����
                targetEntity = livingEntity;
            }
        }

    }


    private void OnDrawGizmosSelected() //���� Ȯ��
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

    /*    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //���⿡ ���� ��ũ��Ʈ( ����, �ü����)

            //���� ��ũ��Ʈ ��
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }*/

    /*//�� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        //ü�� ����
        startingHealth = newHealth;
        health = newHealth;

        //���ݷ� ����
        damage = newDamage;

        //�׺�޽� ������Ʈ�� �̵� �ӵ� ����
        agent.speed = newSpeed;
    }*/

    /*public void OnDamage() //���� ������
    {
        //���� ����� ������ ���� ����� LivingEntity ������Ʈ ��������
        livingEntity attackTarget = GetComponent<livingEntity>();

        //���� ó��
        attackTarget.OnDamage(damage);

        //�ֱ� ���� �ð� ����
        lastAttackTime = Time.time;
    }*/

    /*//����Ƽ �ִϸ��̼� �̺�Ʈ�� �����ϴ� ����� �� ������ �����Ű��
    public void OnDamageEvent()
    {
        //���� ����� ������ ���� ����� LivingEntity ������Ʈ ��������
        livingEntity attackTarget = targetEntity.GetComponent<livingEntity>();

        //���� ó��
        attackTarget.OnDamage(damage);

        //�ֱ� ���� �ð� ����
        lastAttackTime = Time.time;
    }*/