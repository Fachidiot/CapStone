using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomAnimationEvent : UnityEvent<string>
{
}

public class WeaponAnimationEvents : MonoBehaviour
{
    public CustomAnimationEvent WeaponAnimationEvent = new CustomAnimationEvent();

    public void OnAnimationEvent(string eventName)
    {
        WeaponAnimationEvent.Invoke(eventName);
    }
}
