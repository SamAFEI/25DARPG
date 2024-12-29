using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "InteractableData", menuName = "AFEI/InteractableData")]
public class InteractableData : ScriptableObject
{
    public string name;
    public Sprite icon = null;
    public LocalizedString localizedInteractHint;
    public string interactHint => localizedInteractHint.GetLocalizedString();
    [TextArea]
    public string describe;
    [TextArea]
    public string content;
}
