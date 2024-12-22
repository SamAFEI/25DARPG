using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interactable : MonoBehaviour
{
    public Sprite interactHint;
    public Sprite resistLeft;
    public Sprite resistRight;
    public Image uiInteractIcon => transform.Find("UI_InteractIcon").GetComponent<Image>();
    public TextMeshProUGUI uiInteractName => transform.Find("UI_InteractName").GetComponent<TextMeshProUGUI>();
    public Image uiInteractHint => transform.Find("UI_InteractHint").GetComponent<Image>();
    public bool isMessaging;
    public bool isEnable;
    public Color lightenColor;
    public Color darkenColor;
    public Coroutine ctFlashIcon;

    public void Enable(IInteractable interactable)
    {
        if (isMessaging) { return; }
        gameObject.SetActive(true);
        InteractableData data = interactable.GetInteractableData();
        uiInteractIcon.sprite = data.icon;
        uiInteractName.text = data.interactHint;
        uiInteractHint.sprite = interactHint;
        if (!isEnable)
        {
            ctFlashIcon = StartCoroutine(FlashIcon(uiInteractHint));
        }
        isEnable = true;
    }

    public void Disable()
    {
        if (ctFlashIcon != null) { StopCoroutine(ctFlashIcon); }
        isMessaging = false;
        isEnable = false;
        gameObject.SetActive(false);
    }

    public void Message(IInteractable interactable)
    {
        gameObject.SetActive(true);
        isMessaging = true;
        InteractableData data = interactable.GetInteractableData();
        uiInteractIcon.sprite = data.icon;
        uiInteractName.text = data.content;
    }

    public void Resist()
    {
        gameObject.SetActive(true);
        isMessaging = true;
        uiInteractIcon.sprite = resistLeft;
        uiInteractName.text = " Quick Press";
        uiInteractHint.sprite = resistRight;
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
}
