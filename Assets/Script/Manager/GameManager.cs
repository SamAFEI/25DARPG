using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player { get; private set; }

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
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    /// <summary>
    /// Get 朝Player方向向量
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector3 方向 </returns>
    public Vector3 GetPlayerDirection(Vector3 _position)
    {
        if (player == null) { return Vector3.zero; }
        Vector3 _source = new Vector3(_position.x, 0, _position.z);
        Vector3 _target = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        return _target - _source;
    }

    /// <summary>
    /// Get 與Player的距離
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public float GetPlayerDistance(Vector3 _position)
    {
        if (player == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;

    }

    public float GetPlayerDistanceX(Vector3 _position)
    {
        if (player == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;
    }
}
