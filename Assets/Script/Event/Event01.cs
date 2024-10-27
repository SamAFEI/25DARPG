using UnityEngine;

public class Event01 : MonoBehaviour, IInteractable
{
    public UI_Interactable uiInteractable { get; private set; }
    public InteractableData data;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }

    public void Interact()
    {
        uiInteractable.Message(this);
    }

    public InteractableData GetInteractableData()
    {
        return data;
    }
}
