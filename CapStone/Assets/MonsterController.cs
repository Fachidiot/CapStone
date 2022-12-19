using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MonsterController : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] MultiAimConstraint[] targetList;

    [Header("Status")]
    [SerializeField] bool findTarget;

    public Animator rigController;

    void Start()
    {
        foreach (var component in targetList)
        {
            var data = component.data.sourceObjects;
            data.SetTransform(0, playerPos);
            component.data.sourceObjects = data;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (findTarget)
        {
        }
    }
}
