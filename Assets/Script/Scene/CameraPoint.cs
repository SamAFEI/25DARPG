using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public AreaCheckEnum AreaCheck;
    public CameraViewEnum CameraViewUp;
    public Collider ColliderUp;
    public CameraViewEnum CameraViewDown;
    public Collider ColliderDown;

    public Collider collider => GetComponent<Collider>();
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 playerPos = other.transform.position;
            Vector3 closestPoint = collider.ClosestPoint(playerPos);
            if (AreaCheck == AreaCheckEnum.PosZ)
            {
                if (closestPoint.z < playerPos.z) //��Z+
                { CameraManager.ChangeCamera((int)CameraViewUp, ColliderUp); }
                else //��Z-
                { CameraManager.ChangeCamera((int)CameraViewDown, ColliderDown); }
            }
            else if (AreaCheck == AreaCheckEnum.PosX)
            {
                if (closestPoint.x < playerPos.x) //��X+
                { CameraManager.ChangeCamera((int)CameraViewUp, ColliderUp); }
                else //��X-
                { CameraManager.ChangeCamera((int)CameraViewDown, ColliderDown); }
            }
        }
    }
}

public enum AreaCheckEnum
{
    PosZ, PosX
}
public enum CameraViewEnum
{
    ViewZ, ViewX
}
