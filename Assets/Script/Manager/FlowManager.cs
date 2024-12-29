using Fungus;
using UnityEngine;

public class FlowManager : MonoBehaviour, ISaveManager
{
    public static FlowManager Instance { get; private set; }
    public PlayerInput playerInput;
    public Flowchart chart;
    public ForestFlow forestFlow = new ForestFlow();
    public GameObject FirstEncounterEnemyFlowTrigger;
    public GameObject boss1;
    public GameObject playerDialog;
    public GameObject swordDialog;
    private Localization localization;
    private DialogInput dialogInput;
    private GameObject sayDialog;
    private bool isDialogActive;

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
        localization = chart.GetComponent<Localization>();
        dialogInput = chart.GetComponentInChildren<DialogInput>();
        sayDialog = dialogInput.gameObject;
    }

    private void Start()
    {
        playerDialog.SetActive(false);
        swordDialog.SetActive(false);
    }

    private void Update()
    {
        ShowDialog();
        if (!forestFlow.isStartEvent)
        {
            ExecuteChart(ChartIndex.GameStart);
            forestFlow.isStartEvent = true;
        }
        if (sayDialog.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            dialogInput.SetNextLineFlag();
        }
        if (!sayDialog.activeSelf && isDialogActive)
        {
            isDialogActive = false;
            Instance.playerInput.inputHandle.Character.Enable();
        }
    }

    public static void ExecuteChart(ChartIndex index)
    {
        Instance.localization.SetActiveLanguage(SettingManager.Instance.language.ToString());
        Instance.chart.ExecuteIfHasBlock(index.ToString());
        Instance.isDialogActive = true;
        Instance.playerInput.inputHandle.Character.Disable();

        if (index == ChartIndex.LearnEarthshatter)
        {
            Instance.forestFlow.isLearnEarthshatter = true;
        }
        else if (index == ChartIndex.FirstEncounterEnemy)
        {
            Instance.forestFlow.isFirstEncounterEnemy = true; 
            Destroy(Instance.FirstEncounterEnemyFlowTrigger);
        }
    }

    public static void SetBooleanVariable(string key, bool value)
    {
        Instance.chart.SetBooleanVariable(key, value);
    }

    public void ShowDialog()
    {
        if (!sayDialog.activeSelf)
        {
            playerDialog.SetActive(false);
            swordDialog.SetActive(false);
            return;
        }
        if (sayDialog.GetComponent<SayDialog>().SpeakingCharacter == null) return;

        string characherName = sayDialog.GetComponent<SayDialog>().SpeakingCharacter.name;
        if (characherName == "Elf")
        {
            playerDialog.SetActive(true);
            swordDialog.SetActive(false);
        }
        else if (characherName == "Sword")
        {
            playerDialog.SetActive(false);
            swordDialog.SetActive(true);
        }
    }

    public void LoadData(GameData _data)
    {
        forestFlow = _data.forestFlow;

        if (forestFlow.isFirstEncounterEnemy)
        {
            Destroy(FirstEncounterEnemyFlowTrigger);
        }
        if (forestFlow.isLearnEarthshatter)
        {
            SetBooleanVariable(ChartIndex.LearnEarthshatter.ToString(), true);
            Destroy(boss1);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.forestFlow = forestFlow;
    }
}

public enum ChartIndex
{
    GameStart, DungeonEntrance, FirstEncounterEnemy, InvestigateRocks, BOSS1,
    LearnEarthshatter, DemoEnd, InvestigateThorns,
}
