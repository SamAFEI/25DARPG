using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    private float frozenTime, frozdenDeltaTime;
    private bool isFrozening;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }
    public void FixedUpdate()
    {
        if (frozdenDeltaTime > 0)
        {
            frozdenDeltaTime -= Time.deltaTime;
            Time.timeScale = Mathf.Lerp(0.1f, 1f, 1 - (frozdenDeltaTime / frozenTime));
            if (frozdenDeltaTime <= 0) { isFrozening = false; }
        }
    }
    public void SlowFrozenTime(float _time)
    {
        if (isFrozening) { return; }
        frozenTime = _time;
        frozdenDeltaTime = _time;
        isFrozening = true;
    }

    public void DoFrozenTime(float _time = 0.1f)
    {
        if (isFrozening) { return; }
        StartCoroutine(FrozenTime(_time));
    }

    public IEnumerator FrozenTime(float _time = 0.1f)
    {
        isFrozening = true;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_time);
        Time.timeScale = 1.0f;
        isFrozening = false;
    }
}
