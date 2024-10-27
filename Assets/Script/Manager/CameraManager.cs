using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineBrain;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public CinemachineBrain cameraBrain;
    public CinemachineVirtualCamera activeCamera;
    public CinemachineBasicMultiChannelPerlin cbmPerlin;
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
        cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
    }


    private void Update()
    {
        GetActiveCamera();
        DoShake();
    }

    public void GetActiveCamera()
    {
        Instance.activeCamera = Instance.cameraBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (Instance.activeCamera == null){ return; }
        Instance.cbmPerlin = Instance.activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Instance.cbmPerlin.m_AmplitudeGain = 0f;
        Instance.cbmPerlin.m_FrequencyGain = 0f;
    }

    #region AreaCamera
    public static void ChangeCamera(ref CinemachineVirtualCamera nextCamera)
    {
        Instance.activeCamera.Priority = 10;
        nextCamera.Priority = 11;
        Instance.GetActiveCamera();
    }

    public static void ChangeCamera(ref CinemachineVirtualCamera current, ref CinemachineVirtualCamera next)
    {
        current.Priority = 10;
        next.Priority = 11;
        Instance.GetActiveCamera();
        //Instance.SetActiveCamera(next);
    }

    public void SetActiveCamera(CinemachineVirtualCamera camera)
    {
        activeCamera = camera;
        cbmPerlin = activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Instance.cbmPerlin.m_AmplitudeGain = 0f;
        Instance.cbmPerlin.m_FrequencyGain = 0f;
    }
    #endregion

    #region ShakeCamera
    public static void Shake(float intensity, float time)
    {
        if (Instance.isSharking) { return; }
        if (Instance.shakeTimer <= 0)
        {
            Instance.startingIntensity = intensity;
            Instance.shakeTimer = time;
            Instance.shakeTimerTotal = time;
            Instance.isSharking = true;
        }
    }
    public void DoShake()
    {
        if (shakeTimer > 0)
        {
            GetActiveCamera();
            //SetActiveCamera(cameraBrain.ActiveVirtualCamera as CinemachineVirtualCamera);
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                cbmPerlin.m_AmplitudeGain = 0f;
                cbmPerlin.m_FrequencyGain = 0f;
                isSharking = false;
            }
            else
            {
                cbmPerlin.m_FrequencyGain = 0.1f;
                cbmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / -shakeTimerTotal);
            }
        }
        //Camera.main.transform.rotation = Quaternion.identity;
    }
    #endregion


    #region ¨Ì Camera ¶b¦V
    public static Vector3 GetDirectionByCamera(float vertical, float horizontal)
    {
        Vector3 forward = Vector3.forward;
        forward = Camera.main.transform.TransformDirection(forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        return horizontal * right + vertical * forward;
    }

    public static Vector3 GetDirectionByCamera(Vector3 vector)
    {
        return GetDirectionByCamera(vector.z, vector.x);
    }
    #endregion
}
