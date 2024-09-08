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
                if (transform.position.z < playerPos.z) //��Z+
                { CameraManager.ChangeCamera(ref AreaCamera01, ref AreaCamera02); }
                else //��Z-
                { CameraManager.ChangeCamera(ref AreaCamera02, ref AreaCamera01); }
            }
            else if (AreaCheck == AreaCheckEnum.PosX)
            {
                if (transform.position.x < playerPos.x) //��X+
                { CameraManager.ChangeCamera(ref AreaCamera01, ref AreaCamera02); }
                else //��X-
                { CameraManager.ChangeCamera(ref AreaCamera02, ref AreaCamera01); }
            }
        }
    }
}

public enum AreaCheckEnum
{
    PosZ, PosX
}
