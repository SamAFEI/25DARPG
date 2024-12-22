using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogCanvas : MonoBehaviour
{
    private static UI_DialogCanvas instance;
    public static UI_DialogCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UI_DialogCanvas>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }

    public GameObject UI_Area { get; private set; }
    public bool isShowing { get { return UI_Area.activeSelf; } }
    public TextMeshProUGUI TX_Header;
    public TextMeshProUGUI TX_Content;
    public Button BT_Close;
    public Button RBT_Yes;
    public Button RBT_No;
    public Button RBT_Ok;

    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/UI/UI_DialogCanvas");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<UI_DialogCanvas>();
        Instance.UI_Area = Instance.transform.Find("UI_Area").gameObject;
        Instance.UI_Area.SetActive(false);
    }

    public static void Show(string content, Action yesAction, Action noAction = null)
    {
        Instance.UI_Area.SetActive(true);
        Instance.TX_Content.text = content;
        Instance.RBT_Yes.onClick.RemoveAllListeners();
        Instance.RBT_No.onClick.RemoveAllListeners();
        Instance.RBT_Ok.onClick.RemoveAllListeners();
        Instance.RBT_Yes.gameObject.SetActive(false);
        Instance.RBT_No.gameObject.SetActive(false);
        Instance.RBT_Ok.gameObject.SetActive(false);
        if (noAction != null)
        {
            Instance.RBT_Yes.gameObject.SetActive(true);
            Instance.RBT_No.gameObject.SetActive(true);
            Instance.RBT_No.onClick.AddListener(() =>
            {
                AudioManager.PlaySelectSE();
                Instance.UI_Area.SetActive(false);
                noAction();
            });
            Instance.RBT_Yes.onClick.AddListener(() =>
            {
                AudioManager.PlaySelectSE();
                Instance.UI_Area.SetActive(false);
                yesAction();
            });
        }
        else
        {
            Instance.RBT_Ok.gameObject.SetActive(true);
            Instance.RBT_Ok.onClick.AddListener(() =>
            {
                AudioManager.PlaySelectSE();
                Instance.UI_Area.SetActive(false);
                yesAction();
            });
        }
    }
    
    public static void Close()
    {
        Instance.UI_Area.SetActive(false);
    }
}
