using UnityEngine;
using DG.Tweening;
public class UI_PlayerCanvas : MonoBehaviour
{
    public Player player => gameObject.GetComponentInParent<Player>();
    public UI_Dialog uiDialogPlayer;
    public UI_Dialog uiDialogSword;

    private void Start()
    {
        //uiDialogPlayer.SetContent("�]�C������ Start");
    }

    private void LateUpdate()
    {
        //uiDialogPlayer.SetContentFlip(player.FacingDir);
        //uiDialogSword.SetContentFlip(player.FacingDir);
    }
}
