using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Vector3 initPos;
    public Vector3 openPos;

    public float openDelay;

    [SerializeField] bool isOpen = false;
    public bool Open
    {
        set { isOpen = value; }
    }

    void Awake()
    {
        initPos = transform.position;
        openPos = transform.position + openPos;
    }

    private void Update()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        if (isOpen)
        {
            if (transform.position.Equals(openPos))
                return;
            transform.position = Vector3.Lerp(transform.position, openPos, 0.001f * openDelay);
        }
        else
        {
            if (transform.position.Equals(initPos))
                return;
            transform.position = Vector3.Lerp(transform.position, initPos, 0.001f * openDelay);
        }
    }
}
