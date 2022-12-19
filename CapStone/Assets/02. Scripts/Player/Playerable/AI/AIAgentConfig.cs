using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 0.1f;
    public float maxDistance = 2.0f;
    public float dieForce = 10f;
    public float maxSightDistance = 15.0f;
    public float maxPrimaryFireDistance = 10.0f;
    public float maxSecondaryFireDistance = 5.0f;
}
