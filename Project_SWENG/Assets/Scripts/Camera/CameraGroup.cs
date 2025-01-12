using Cinemachine;
using UnityEngine;

class CameraGroup : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCameraBase characterCam;
    [SerializeField] private CinemachineVirtualCameraBase battleCamLeft;
    [SerializeField] private CinemachineVirtualCameraBase battleCamRight;
    [SerializeField] private CinemachineVirtualCameraBase wideCam;

    Transform _target;

    public void SetCamTarget(Transform target)
    {
        _target = target;

        characterCam.Follow = target;
        battleCamLeft.LookAt = target;

        battleCamLeft.Follow = target;
        battleCamRight.LookAt = target;

        battleCamRight.Follow = target;
        characterCam.LookAt = target;

        wideCam.Follow = target;
        wideCam.LookAt = target;
    }

    public void OffTheGroup()
    {

        characterCam.Priority = 0;
        wideCam.Priority = 0;
        battleCamLeft.Priority = 0;
        battleCamRight.Priority = 0;
    }

    public void ConvertToCharacterCam()
    {
        characterCam.Priority = 10;
        wideCam.Priority = 0;
        battleCamLeft.Priority = 0;
        battleCamRight.Priority = 0;
    }

    public void ConvertToBattleCam()
    {
        characterCam.Priority = 0;
        wideCam.Priority = 0;

        if (Vector3.Dot(_target.forward, new Vector3(2, 0, 1)) < 0)
            battleCamLeft.Priority = 10;
        else
            battleCamRight.Priority = 10;
    }

    public void ConvertToWideCam()
    {
        characterCam.Priority = 0;
        wideCam.Priority = 10;
        battleCamLeft.Priority = 0;
        battleCamRight.Priority = 0;
    }

}
