using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIManager : MonoBehaviour
{
    public GameObject[] viewContents;

    ScrollRect scrollRect; 

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void ToggleOn(int index) {
        Reset();
        viewContents[index].SetActive(true);
        scrollRect.content = viewContents[index].GetComponent<RectTransform>();
    }

    void Reset() {
        foreach (var content in viewContents)
        {
            content.SetActive(false);
        }
    }
}
