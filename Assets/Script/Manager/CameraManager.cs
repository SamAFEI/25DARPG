using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private CinemachineVirtualCamera cvCamera;
    private CinemachineBasicMultiChannelPerlin cbmPerlin;
    private float shakeTimer = 0;
    private float shakeTimerTotal;
    private float startingIntensity;
    private bool isSharking;

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
        //cvCamera = GetComponent<CinemachineVirtualCamera>();
        //cbmPerlin = cvCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cvCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        cbmPerlin = cvCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Shake(float intensity, float time)
    {
        if (isSharking) { return; }
        if (shakeTimer <= 0)
        {
            startingIntensity = intensity;
            shakeTimer = time;
            shakeTimerTotal = time;
            isSharking = true;
        }
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                cbmPerlin.m_AmplitudeGain = 0f;
                isSharking = false;
            }
            else
            {
                cbmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / -shakeTimerTotal);
            }
        }
    }
}
