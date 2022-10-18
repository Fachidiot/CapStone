using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class simple_fadeout : MonoBehaviour
{
    public float speed = 1.5f;

    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.color -= new Color(0, 0, 0, Time.deltaTime * speed);
    }
}
