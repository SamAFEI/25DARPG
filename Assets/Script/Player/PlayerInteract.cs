using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region Components
    public Player player => GetComponent<Player>();
    public PlayerInput input => GetComponent<PlayerInput>();
    public UI_Interactable uiInteractable;
    #endregion

    public IInteractable interactable;
    public LayerMask interactableLayerMask;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
        uiInteractable.Disable();
    }

    private void FixedUpdate()
    {
        SearchInteractable();
    }

    private void SearchInteractable()
    {
        if (player.IsSexing || player.IsDied) return;
        float range = 5f;
        Collider collider = Physics.OverlapSphere(transform.position, range, interactableLayerMask).FirstOrDefault();
        if (collider != null)
        {
            interactable = collider.transform.GetComponentInParent<IInteractable>();
            uiInteractable.Enable(interactable);
        }
        else
        {
            interactable = null;
        }
        if (interactable == null && !uiInteractable.isMessaging) { uiInteractable.Disable(); }
    }

    public void DoInteract()
    {
        if (uiInteractable.isMessaging)
        {
            uiInteractable.Disable();
            return;
        }
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}
