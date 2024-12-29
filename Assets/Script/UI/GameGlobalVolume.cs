using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameGlobalVolume : MonoBehaviour
{
    public static GameGlobalVolume Instance { get; private set; }
    public Volume volume { get; private set; }
    public LensDistortion distortion { get; private set; }
    private float smooth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        volume = GetComponent<Volume>();
        LensDistortion tmp;
        if (volume.profile.TryGet(out tmp))
        {
            distortion = tmp;
        }
    }

    public static void DoLensDistortion(bool isDistortion)
    {
        Instance.smooth = 0;
        Instance.StartCoroutine(Instance.LerpDistortion(isDistortion));
    }

    public IEnumerator LerpDistortion(bool isDistortion)
    {
        float intensity, scale;
        float intensityStart = 0f;
        float intensityEnd = -1f;
        float scaleStart = 1f;
        float scaleEnd = 0.001f;
        if (!isDistortion)
        {
            intensityStart = -1f;
            intensityEnd = 0f;
            scaleStart = 0.001f;
            scaleEnd = 1f;
        }
        while (smooth < 1f)
        {
            smooth += Time.deltaTime * 2.5f;
            intensity = Mathf.Lerp(intensityStart, intensityEnd, smooth);
            scale = Mathf.Lerp(scaleStart, scaleEnd, smooth);
            Instance.distortion.intensity.value = intensity;
            Instance.distortion.scale.value = scale;
            yield return null;
        }
        Instance.distortion.intensity.value = intensityEnd;
        Instance.distortion.scale.value = scaleEnd;
        GameManager.ResetActiveScene();
    }
}

