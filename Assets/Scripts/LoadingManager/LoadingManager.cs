using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider;
    public string nextSceneName; // The name of the next scene to load

    void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        // Start loading the next scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        
        // While the scene is loading, update the slider if it exists
        while (!operation.isDone)
        {
            if (loadingSlider != null)
            {
                loadingSlider.value = Mathf.Clamp01(operation.progress / 0.9f);
            }

            yield return null;
        }
    }
}
