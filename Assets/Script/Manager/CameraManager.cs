using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public List<CinemachineVirtualCamera> DollyCameraList;
    private CinemachineBasicMultiChannelPerlin cbmPerlin;
    private CinemachineVirtualCamera CurrentCamera;
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
        CurrentCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        cbmPerlin = CurrentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        if (DollyCameraList.Count > 0)
        {
            CurrentCamera = DollyCameraList[0];
            ChangeCamera(0);
        }
    }

    public void ChangeCamera(int index)
    {
        if (DollyCameraList.Count <= index) { return; }
        CurrentCamera.Priority = 10;
        DollyCameraList[index].Priority = 11;
        CurrentCamera = DollyCameraList[index];
        cbmPerlin = CurrentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
