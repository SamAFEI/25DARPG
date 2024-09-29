using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private CinemachineBrain CameraBrain;
    private CinemachineVirtualCamera CurrentCamera;
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
        CameraBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        CurrentCamera = CameraBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    private void Update()
    {
        DoShake();
    }

    #region AreaCamera
    public static void ChangeCamera(ref CinemachineVirtualCamera current, ref CinemachineVirtualCamera next)
    {
        current.Priority = 10;
        next.Priority = 11;
        Instance.SetCurrentCamera(next);
    }

    public void SetCurrentCamera(CinemachineVirtualCamera camera)
    {
        CurrentCamera = camera;
        cbmPerlin = CurrentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
            SetCurrentCamera(CameraBrain.ActiveVirtualCamera as CinemachineVirtualCamera);
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
