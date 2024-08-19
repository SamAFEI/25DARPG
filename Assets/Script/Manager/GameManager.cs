using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerObj { get; private set; }
    public Player player { get; private set; }
    public List<Enemy> sexEnemies { get; private set; } = new List<Enemy>();

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
        playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<Player>();
    }

    /// <summary>
    /// Get 朝Player方向向量
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector3 方向 </returns>
    public static Vector3 GetPlayerDirection(Vector3 _position)
    {
        if (Instance.playerObj == null) { return Vector3.zero; }
        Vector3 _source = new Vector3(_position.x, 0, _position.z);
        Vector3 _target = new Vector3(Instance.playerObj.transform.position.x, 0, Instance.playerObj.transform.position.z);
        return _target - _source;
    }

    /// <summary>
    /// Get 與Player的距離
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public static float GetPlayerDistance(Vector3 _position)
    {
        if (Instance.playerObj == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;

    }

    public static float GetPlayerDistanceX(Vector3 _position)
    {
        if (Instance.playerObj == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;
    }

    public static bool CanAttackPlayer()
    {
        if (Instance.playerObj == null) { return false; }
        return !Instance.player.IsStunning && !Instance.player.IsSexing && Instance.player.CurrentHp > 0;
    }
    public static bool CanSexPlayer()
    {
        if (Instance.playerObj == null) { return false; }
        return Instance.player.IsStunning;
    }

    public static void AddSexEnemies(Enemy enemy)
    {
        Instance.sexEnemies.Add(enemy);
        enemy.skeleton.SetActive(false);
    }

    public static void ResetSexEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        enemies.AddRange(Instance.sexEnemies);
        foreach(Enemy enemy in enemies)
        {
            enemy.skeleton.SetActive(true);
            enemy.FSM.ChangeState(enemy.idleState);
            Instance.sexEnemies.Remove(enemy);
        }
    }
}
