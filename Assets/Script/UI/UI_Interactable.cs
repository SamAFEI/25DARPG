using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interactable : MonoBehaviour
{
    public Image ui_interactableIcon => transform.Find("UI_InteractIcon").GetComponent<Image>();
    public TextMeshProUGUI UI_InteractName => transform.Find("UI_InteractName").GetComponent<TextMeshProUGUI>();

    public void Active(IInteractable interactable)
    {
        gameObject.SetActive(true);
        InteractableData data = interactable.GetInteractableData();
        ui_interactableIcon.sprite = data.icon;
        UI_InteractName.text = data.name;
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
    }
}
