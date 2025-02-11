using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance { get; private set; }
    public static string LoadSceneName;
    public static string SavePointName;
    public static bool IsPaused { get; private set; }
    public GameObject playerObj { get; private set; }
    public Player player { get; private set; }
    public List<Enemy> sexEnemies { get; private set; } = new List<Enemy>();
    public List<Enemy> alertEnemies { get; private set; } = new List<Enemy>();
    public Volume volume { get; private set; }
    public bool isBattleing { get; private set; }

    public LayerMask enemyLayerMask;
    public bool isStartScene;
    private SavePoint lastSavePoint;

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
        SavePointName = ""; 
        LoadSceneName = "";
    }

    private void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
        }
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
    public void DoEarthshatter()
    {
        if (Instance.playerObj == null) { return; }
        Instance.player.DoEarthshatter();
    }
    #endregion

    #region About Enemy
    public static void AddSexEnemies(Enemy enemy)
    {
        Instance.sexEnemies.Add(enemy);
        enemy.FSM.SetNextState(enemy.alertState);
        enemy.IsSexing = true;
        enemy.skeleton.SetActive(false);
    }
    public static void ResetSexEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        enemies.AddRange(Instance.sexEnemies);
        foreach (Enemy enemy in enemies)
        {
            enemy.skeleton.SetActive(true);
            enemy.IsSexing = false;
            enemy.FSM.SetNextState(enemy.alertState);
            Instance.sexEnemies.Remove(enemy);
        }
    }
    public static void ResetSexEnemies(Vector3 postion)
    {
        ResetSexEnemies();
        float range = 10f;
        List<Collider> colliders = Physics.OverlapSphere(postion, range, Instance.enemyLayerMask).ToList();
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            enemy.LastCatchTime = Random.Range(3f, 5f);
        }
    }
    public static void AddAlertEnemy(Enemy enemy)
    {
        if (Instance.alertEnemies.Contains(enemy)) return;
        Instance.alertEnemies.Add(enemy);
        PlayBattleBGM();
    }
    public static void RemoveAlertEnemy(Enemy enemy)
    {
        if (!Instance.alertEnemies.Contains(enemy)) return;
        Instance.alertEnemies.Remove(enemy);
        PlayBattleBGM();
    }
    public static void PlayBattleBGM()
    {
        if (Instance.alertEnemies.Where(x => x.Data.isBoos).Count() > 0)
        {
            Instance.isBattleing = true;
            AudioManager.PlayBGM(2);
            return;
        }
        if (Instance.alertEnemies.Count() > 0)
        {
            Instance.isBattleing = true;
            AudioManager.PlayBGM(1);
            return;
        }
        AudioManager.PlayBGM(0);
        Instance.isBattleing = false;
    }
    #endregion

    #region About Scene
    public static void LoadScene(string _sceneName)
    {
        LoadSceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
        PausedGame(false);
    }
    public static void ResetActiveScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        LoadScene(scene.name);
    }
    public static void LoadStarScene()
    {
        LoadScene("StartScene");
    }
    public static void LoadTitleScene()
    {
        LoadScene("TitleScene");
    }
    public static void LoadGameScene()
    {
        LoadScene("GameScene");
    }
    public static void LoadCGScene()
    {
        LoadScene("Live2DScene");
    }
    #endregion

    #region About SaveLoad
    public static void DoSaveGame(string savePointName)
    {
        SavePointName = savePointName;
        SaveManager.SaveGame();
        GameGlobalVolume.DoLensDistortion(true);
        //ResetActiveScene(); //在GameGlobalVolume.DoLensDistortion(true); 執行
    }

    public void LoadData(GameData _data)
    {
        SavePointName = _data.SavePointName;
        List<SavePoint> savePoints = GameObject.FindObjectsOfType<SavePoint>().ToList();
        foreach (SavePoint savePoint in savePoints)
        {
            if (savePoint.name == SavePointName)
            {
                lastSavePoint = savePoint;
                playerObj.transform.position = lastSavePoint.LoadPoint();
                break;
            }
        }
        SavePointName = "";
    }

    public void SaveData(ref GameData _data)
    {
        _data.SavePointName = SavePointName;
    }
    #endregion

    public static void PausedGame(bool paused)
    {
        IsPaused = paused;
        if (IsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        AudioManager.PausePlay(IsPaused);
    }

}
