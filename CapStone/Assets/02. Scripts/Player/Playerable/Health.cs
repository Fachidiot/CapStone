using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float dieForce = 10f;
    [HideInInspector] public float currentHealth;
    public float blinkIntensity = 10f;
    public float blinkDuration = 0.1f;

    float blinkTimer;
    AIAgent agent;
    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;

    void Start()
    {
        agent = GetComponent<AIAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }
    
    public void TakeDamage(float amount, Vector3 direction) {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f) {
            if (agent)
                Die(direction);
            else {
                Ragdoll ragdoll = GetComponent<Ragdoll>();
                NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
                
                ragdoll.ActivateRagdoll();
                navMeshAgent.enabled = false;
                direction.y = 1;
                ragdoll.ApplyForce(direction * agent.config.dieForce);
                healthBar.gameObject.SetActive(false);
                skinnedMeshRenderer.updateWhenOffscreen = true;
            }
        }

        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction) {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateId.Death);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
