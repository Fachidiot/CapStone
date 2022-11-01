using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string loadSceneName;
    public float time;

    IEnumerator WaitForSecondAndLoad(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(loadSceneName);
        StopCoroutine("WaitForSecondAndLoad");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            StartCoroutine(WaitForSecondAndLoad(time));
    }
}
