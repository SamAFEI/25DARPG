using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : MonoBehaviour
{
    public TextMeshProUGUI tmpContent => transform.Find("TMP_Content").GetComponent<TextMeshProUGUI>();
    public Image imHint;
    public Image imItem;
    public bool isMessaging;
    public bool isEnable;
    public Color lightenColor;
    public Color darkenColor;
    public Coroutine ctFlashIcon;
    public Coroutine ctDoTalk;

    public void Awake()
    {
        imHint.enabled = false;
        imItem.enabled = false;
        //StartCoroutine(DoTalk());
        //ctFlashIcon = StartCoroutine(FlashIcon(imHint));
    }

    public IEnumerator FlashIcon(Graphic graphic)
    {
        while (true)
        {
            graphic.color = lightenColor;
            yield return new WaitForSeconds(0.4f);
            graphic.color = darkenColor;
            yield return new WaitForSeconds(0.4f);
        }
    }

    public void SetContentFlip(int facingDir)
    {
        float x = tmpContent.rectTransform.localScale.y * facingDir * -1;
        Vector3 scale = new Vector3(x, tmpContent.rectTransform.localScale.y, tmpContent.rectTransform.localScale.z);
        tmpContent.rectTransform.DOScale(scale, 0f);
    }

    public void SetContent(string content)
    {
        tmpContent.text = "";
        tmpContent.DOText(content, 0.5f);
    }

    private void OnEnable()
    {
        ctDoTalk = StartCoroutine(DoTalk());
    }
    private void OnDisable()
    {
        StopCoroutine(ctDoTalk);
    }

    public IEnumerator DoTalk()
    {
        while (true)
        {
            SetContent("......");
            yield return new WaitForSeconds(1f);
        }
    }
}
