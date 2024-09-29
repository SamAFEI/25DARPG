using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas : MonoBehaviour
{
    public static UI_Canvas Instance { get; private set; }
    public Image UI_Die;
    public TextMeshProUGUI UI_DieContent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitUI_Die();
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (UI_DieContent.color.a == 1 && Input.anyKeyDown)
        {
            GameManager.Instance.RestarScene();
        }
    }

    public void FadeInUI_Die()
    {
        StartCoroutine(DoFadeIn(UI_Die));
        StartCoroutine(DoFadeIn(UI_DieContent, 10f, 3f));
    }

    public Color SetColorAlpha(Color color, float alpha)
    {
        Color newColor = new Color(color.r, color.g, color.b, alpha);
        return newColor;
    }

    public void InitUI_Die()
    {
        UI_DieContent.color = SetColorAlpha(UI_DieContent.color, 0);
        UI_Die.color = SetColorAlpha(UI_Die.color, 0);
        UI_Die.gameObject.SetActive(false);
    }

    public IEnumerator DoFadeIn(Graphic graphic, float fadetime = 10f, float delay = 1f)
    {
        yield return new WaitForSeconds(delay);
        float fadecount = 0;
        graphic.gameObject.SetActive(true);
        while (graphic.color.a < 1)
        {
            fadecount++;
            float alpha = Mathf.Lerp(0, 1f, (fadecount / fadetime));
            graphic.color = SetColorAlpha(graphic.color, alpha);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
