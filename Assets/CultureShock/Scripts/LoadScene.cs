using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public TMP_Text text;
    public String sceneToLoad;
    private void Start()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void Update()
    {
        StartCoroutine(TextLoad());
    }

    private IEnumerator TextLoad()
    {
        text.text = "Loading.";
        yield return new WaitForSeconds(1);
        text.text = "Loading..";
        yield return new WaitForSeconds(1);
        text.text = "Loading...";
    }
}
