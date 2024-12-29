using UnityEngine;

public class FlowTrigger : MonoBehaviour, IInteractable
{
    public UI_Interactable uiInteractable { get; private set; }
    public FlowTirggerType tirggerType;
    public InteractableData interactableData;
    public ChartIndex chartIndex;

    public void Interact()
    {
        if (GameManager.Instance.isBattleing)
        {
            AudioManager.PlayCancelSFX(transform.position);
            return;
        }
        if (tirggerType != FlowTirggerType.Interactable) return;
        AudioManager.PlayFlowTriggerSFX(transform.position);
        FlowManager.ExecuteChart(chartIndex);
    }

    public InteractableData GetInteractableData()
    {
        return interactableData;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isBattleing) return;
        if (tirggerType != FlowTirggerType.PlayerEnter) return;
        if (other.gameObject.tag == "Player")
        {
            FlowManager.ExecuteChart(chartIndex);
        }
    }
}

public enum FlowTirggerType
{
    PlayerEnter, Interactable,
}
