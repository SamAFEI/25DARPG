using SharpUI.Source.Common.UI.Elements.Loading;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private LoadingBar loadingBar;
    AsyncOperation _async;
    int progress = 0;

    private void Awake()
    {
        loadingBar = GetComponentInChildren<LoadingBar>();
    }
    private void Start()
    {
        StartCoroutine(LoadSecne(GameManager.LoadSceneName));
    }

    private void OnGUI()
    {
        loadingBar.UpdatePercentage(progress);
    }

    private IEnumerator LoadSecne(string sceneName)
    {
        int _targetValue;
        _async = SceneManager.LoadSceneAsync(sceneName);
        _async.allowSceneActivation = false;

        while (_async.progress < 0.9f)
        {
            _targetValue = (int)_async.progress * 100;
            while (progress < _targetValue)
            {
                ++progress;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        _targetValue = 100;
        while (progress < _targetValue)
        {
            ++progress;
            yield return new WaitForEndOfFrame();
        }

        if (progress >= 100)
        {
            yield return new WaitForSeconds(0.5f);
            _async.allowSceneActivation = true;
        }
    }
}
