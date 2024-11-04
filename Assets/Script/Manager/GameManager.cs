using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static string LoadSceneName;
    public GameObject playerObj { get; private set; }
    public Player player { get; private set; }
    public List<Enemy> sexEnemies { get; private set; } = new List<Enemy>();
    public bool isStartScene;

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
        if (isStartScene)
        {
            player.StartCoroutine(player.DoStartAnimation());
        }
    }

    #region About Player

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
        enemy.FSM.SetNextState(enemy.alertState);
        enemy.skeleton.SetActive(false);
    }
    public static void ResetSexEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        enemies.AddRange(Instance.sexEnemies);
        foreach(Enemy enemy in enemies)
        {
            enemy.skeleton.SetActive(true);
            enemy.LastCatchTime = Random.Range(3f, 5f);
            enemy.FSM.SetNextState(enemy.alertState);
            Instance.sexEnemies.Remove(enemy);
        }
    }

    #endregion

    public void LoadScene(string _sceneName)
    {
        LoadSceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public void RestarScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        LoadScene(scene.name);
    }

    public void LoadStarScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        LoadScene("StartScene");
    }
}
