using System.Collections.Generic;
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
    public LayerMask enemyLayerMask;

    private void Start()
    {
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
        uiInteractable.Disable();
    }

    private void FixedUpdate()
    {
        SearchInteractable();
        CheckBattling();
    }

    private void SearchInteractable()
    {
        if (player.IsSexing || player.IsDied) return;
        float range = 3f;
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
    private void CheckBattling()
    {
        float range = 30f;
        int bgmIndex = 0;
        List<Collider> colliders = Physics.OverlapSphere(transform.position, range, enemyLayerMask).ToList();
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy.IsAlerting) 
            { 
                bgmIndex = 1;
                if (enemy.Data.isBoos)
                {
                    bgmIndex = 2;
                    break;
                }
                break;
            }
        }
        AudioManager.PlayBGM(bgmIndex);
    }
}
