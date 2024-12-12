using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public Vector3 point { get; private set; }
    public Material outlineMAT { get; set; }
    public string id;
    public InteractableData data;
    public CameraViewEnum viewType;
    public Collider cameraCollider;
    public UI_Interactable ui_Interactable;
    public float selectedTimeout;

    private void Awake()
    {
        point = transform.Find("Point").transform.position;
        List<Material> materials = GetComponent<MeshRenderer>().materials.ToList();
        foreach (Material mat in materials)
        {
            if (mat.name == "OutlineMAT (Instance)")
            {
                outlineMAT = mat;
                break;
            }
        }
    }
    private void Update()
    {
        selectedTimeout -= Time.deltaTime;
        if (selectedTimeout > 0)
        {
            outlineMAT.SetFloat("_Intensity", 2f);
        }
        else
        {
            outlineMAT.SetFloat("_Intensity", 0.5f);
        }
    }

    public InteractableData GetInteractableData()
    {
        selectedTimeout = 0.2f;
        return data;
    }

    public void Interact()
    {
        ui_Interactable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
        AudioManager.PlayItemPickupSFX(transform.position);
        GameManager.Instance.player.Hurt(-1000000);
        GameManager.DoSaveGame(name);
        ui_Interactable.Message(this);
    }

    public Vector3 LoadPoint()
    {
        CameraManager.ChangeCamera((int)viewType, cameraCollider);
        return point;
    }
}
