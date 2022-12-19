using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    float timer = 0.0f;

    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled) {
            return;
        }

        timer -= Time.deltaTime;
        // if (!agent.navMeshAgent.hasPath) {
        //     agent.navMeshAgent.destination = agent.playerTransform.position;
        // }
        // if (timer < 0.0f) {
        //     Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
        //     direction.y = 0;
        //     Debug.Log($"{direction.sqrMagnitude} / {agent.config.maxDistance * agent.config.maxDistance}");
        //     if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance) {
        //         if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        //             agent.navMeshAgent.destination = agent.playerTransform.position;
        //     }
        //     timer = agent.config.maxTime;
        // }
        if (timer < 0.0f) {
            float sqDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance * agent.config.maxDistance) 
                agent.navMeshAgent.destination = agent.playerTransform.position;
            timer = agent.config.maxTime;
        }

        if (agent.weapons.HasWeapon())
            agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
    }

    public void Exit(AIAgent agent)
    {
    }
}
