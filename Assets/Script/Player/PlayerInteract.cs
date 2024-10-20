using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region Components
    public Player player => GetComponent<Player>();
    public PlayerInput input => GetComponent<PlayerInput>();
    public UI_Interactable ui_Interactable;
    #endregion

    public IInteractable interactable;
    public LayerMask interactableLayerMask;

    private void Start()
    {
        ui_Interactable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
        ui_Interactable.Inactive();
    }

    private void FixedUpdate()
    {
        SearchInteractable();
    }

    private void SearchInteractable()
    {
        ui_Interactable.Inactive();
        if (player.IsSexing || player.IsDied) return;
        float range = 5f;
        Collider collider = Physics.OverlapSphere(transform.position, range, interactableLayerMask).FirstOrDefault();
        if (collider != null)
        {
            interactable = collider.transform.root.GetComponent<IInteractable>();
            ui_Interactable.Active(interactable);
        }
    }

    public void DoInteract()
    {
        if (interactable == null) return;
        interactable.Interact();
        ui_Interactable.Inactive();
        interactable = null;
    }
}
