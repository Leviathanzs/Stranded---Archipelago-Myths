using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLocation : MonoBehaviour
{
    public GameObject loadingScreenUI;
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        // Tampilkan UI loading
        loadingScreenUI.SetActive(true);

        // Mulai load scene secara async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // (Opsional) tunggu sampai selesai
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
