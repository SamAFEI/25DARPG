using Cinemachine;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public AreaCheckEnum AreaCheck;
    public CinemachineVirtualCamera AreaCamera01;
    public CinemachineVirtualCamera AreaCamera02;
    public Collider collider => GetComponent<Collider>();


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 playerPos = other.transform.position;
            Vector3 closestPoint = collider.ClosestPoint(playerPos);
            if (AreaCheck == AreaCheckEnum.PosZ)
            {
                if (closestPoint.z < playerPos.z) //往Z+
                { CameraManager.ChangeCamera(ref AreaCamera02); }
                else //往Z-
                { CameraManager.ChangeCamera(ref AreaCamera01); }
            }
            else if (AreaCheck == AreaCheckEnum.PosX)
            {
                if (closestPoint.x < playerPos.x) //往X+
                { CameraManager.ChangeCamera(ref AreaCamera02); }
                else //往X-
                { CameraManager.ChangeCamera(ref AreaCamera01); }
            }
        }
    }
}

public enum AreaCheckEnum
{
    PosZ, PosX, RightTo2RightTo1, RightTo2LeftTo1
}
