using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(500)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private string fileName;
    [SerializeField] private bool encrptData = true;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

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
            return;
        }
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrptData);
    }
    private void Start()
    {
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public static void NewGame()
    {
        Instance.gameData = new GameData();
    }
    public static void LoadGame()
    {
        if (Instance.dataHandler == null) { return; }
        Instance.saveManagers = FindAllSaveManagers();
        Instance.gameData = Instance.dataHandler.LoadFiled();
        if (Instance.gameData == null)
        {
            NewGame();
        }
        foreach (ISaveManager _saveManager in Instance.saveManagers)
        {
            _saveManager.LoadData(Instance.gameData);
        }
    }
    public static void SaveGame()
    {
        if (Instance.dataHandler == null) { return; }
        Instance.saveManagers = FindAllSaveManagers();
        foreach (ISaveManager _saveManager in Instance.saveManagers)
        {
            _saveManager.SaveData(ref Instance.gameData);
        }
        Instance.dataHandler.SaveFiled(Instance.gameData);
    }
    public static void SaveSetting()
    {
        if (Instance.dataHandler == null) { return; }
        ISaveManager _saveManager = GameObject.FindObjectOfType<SettingManager>();
        _saveManager.SaveData(ref Instance.gameData);
        Instance.dataHandler.SaveFiled(Instance.gameData);
    }

    public static bool CheckHaveSaveData()
    {
        if (Instance.dataHandler == null) { return false; }
        GameData data = Instance.dataHandler.LoadFiled();
        return data != null;
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
    private static List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    [ContextMenu("Delete Save File")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrptData);
        dataHandler.DeleteFiled();
    }
}
