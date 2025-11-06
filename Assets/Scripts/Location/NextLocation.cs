using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class NextLocation : MonoBehaviour
{
    public GameObject loadingScreenUI;
    public string sceneToLoad;
    public Slider loadingBar;

    private StageController stageController;

    void Awake()
    {
        stageController = FindObjectOfType<StageController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // panggil stage selesai sebelum load scene
            if(stageController != null)
            {
                stageController.OnStageEnd();
            }

            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        loadingScreenUI.SetActive(true);

        yield return new WaitForSeconds(0.5f); // delay tampil awal

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float minLoadingTime = 2f;
        float elapsed = 0f;

        while (!operation.isDone)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            if (operation.progress >= 0.9f && elapsed >= minLoadingTime)
            {
                loadingBar.value = 1f;
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}