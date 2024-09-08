using Cinemachine;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public AreaCheckEnum AreaCheck;
    public CinemachineVirtualCamera AreaCamera01;
    public CinemachineVirtualCamera AreaCamera02;


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 playerPos = other.transform.position;
            if (AreaCheck == AreaCheckEnum.PosZ)
            {
                if (transform.position.z < playerPos.z) //往Z+
                { CameraManager.ChangeCamera(ref AreaCamera01, ref AreaCamera02); }
                else //往Z-
                { CameraManager.ChangeCamera(ref AreaCamera02, ref AreaCamera01); }
            }
            else if (AreaCheck == AreaCheckEnum.PosX)
            {
                if (transform.position.x < playerPos.x) //往X+
                { CameraManager.ChangeCamera(ref AreaCamera01, ref AreaCamera02); }
                else //往X-
                { CameraManager.ChangeCamera(ref AreaCamera02, ref AreaCamera01); }
            }
        }
    }
}

public enum AreaCheckEnum
{
    PosZ, PosX
}
