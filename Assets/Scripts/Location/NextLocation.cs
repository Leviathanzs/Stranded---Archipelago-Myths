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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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

            // progress dari 0 ke 0.9 → kita buat jadi 0 ke 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            // Syarat ganti scene: progress selesai & delay terpenuhi
            if (operation.progress >= 0.9f && elapsed >= minLoadingTime)
            {
                loadingBar.value = 1f; // penuh
                yield return new WaitForSeconds(0.5f); // jeda sebentar sebelum masuk scene
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
