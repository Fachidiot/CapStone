using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public CustomInput input;
    public GameObject virtualCamera;

    private void Update()
    {
        if (input.move != new Vector2(0, 0))
        {
            virtualCamera.SetActive(true);
        }
    }
}
