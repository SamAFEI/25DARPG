using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "AFEI/InteractableData")]
public class InteractableData : ScriptableObject
{
    public string name;
    public Sprite icon = null;
    public string interactHint;
    public string content;
}
