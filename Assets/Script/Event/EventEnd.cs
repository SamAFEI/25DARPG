using UnityEngine;

public class EventEnd : MonoBehaviour, IInteractable
{
    public UI_Interactable uiInteractable { get; private set; }
    public InteractableData data;
    public bool isEnd;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }

    private void Update()
    {
        if (isEnd && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.LoadStarScene();
        }
    }

    public void Interact()
    {
        uiInteractable.Message(this);
        isEnd = true;
    }

    public InteractableData GetInteractableData()
    {
        return data;
    }
}
