using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public GameObject CameraViewZ;
    public GameObject CameraViewX;
    public Collider InitCameraCollider;
    public CinemachineVirtualCamera VCameraViewZ { get; private set; }
    public CinemachineVirtualCamera VCameraViewX { get; private set; }
    public CinemachineConfiner ConfinerViewZ;
    public CinemachineConfiner ConfinerViewX;
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
        VCameraViewZ = CameraViewZ.GetComponent<CinemachineVirtualCamera>();
        ConfinerViewZ = CameraViewZ.GetComponent<CinemachineConfiner>();
        VCameraViewX = CameraViewX.GetComponent<CinemachineVirtualCamera>();
        ConfinerViewX = CameraViewX.GetComponent<CinemachineConfiner>();
        ConfinerViewZ.m_BoundingVolume = InitCameraCollider;
        ConfinerViewX.m_BoundingVolume = InitCameraCollider;
    }

    private void Update()
    {
        GetActiveCamera();
        DoShake();
    }

    private void OnDisable()
    {
        ConfinerViewZ.m_BoundingVolume = InitCameraCollider;
        ConfinerViewX.m_BoundingVolume = InitCameraCollider;
    }

    private void OnValidate()
    {
        ConfinerViewZ.m_BoundingVolume = InitCameraCollider;
        ConfinerViewX.m_BoundingVolume = InitCameraCollider;
    }

    public void GetActiveCamera()
    {
        Instance.activeCamera = Instance.cameraBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (Instance.activeCamera == null) { return; }
        Instance.cbmPerlin = Instance.activeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Instance.cbmPerlin.m_AmplitudeGain = 0f;
        Instance.cbmPerlin.m_FrequencyGain = 0f;
    }

    #region AreaCamera
    public static void ChangeCamera(int index, Collider collider)
    {
        Instance.activeCamera.Priority = 10;
        if (index == 0)
        {
            Instance.ConfinerViewZ.m_BoundingVolume = collider;
            Instance.VCameraViewZ.Priority = 11;
        }
        else if (index == 1)
        {
            Instance.ConfinerViewX.m_BoundingVolume = collider;
            Instance.VCameraViewX.Priority = 11;
        }
        Instance.GetActiveCamera();
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
