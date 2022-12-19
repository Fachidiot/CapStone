using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Weapon;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log($"in");
    }

    public void Update(AIAgent agent)
    {
    }

    public void Exit(AIAgent agent)
    {
    }
}
