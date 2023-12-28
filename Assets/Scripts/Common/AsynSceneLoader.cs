using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsynSceneLoader : SingletonBehaviour<AsynSceneLoader>
{
    [SerializeField] private Slider loadingSlider;
    public void LoadSceneByName(string sceneName)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsyn(sceneName));
    }

    IEnumerator LoadSceneAsyn(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (loadingSlider != null)
            {
                loadingSlider.value = progressValue;
            }
            yield return null;
        }
    }
}
