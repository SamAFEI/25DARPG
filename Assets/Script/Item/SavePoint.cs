using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public InteractableData data;
    public UI_Interactable ui_Interactable;

    public InteractableData GetInteractableData()
    {
        return data;
    }

    public void Interact()
    {
        ui_Interactable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
        ui_Interactable.Message(this);
    }

}
