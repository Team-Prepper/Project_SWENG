using Cinemachine;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class CamMovement : MonoSingleton<CamMovement>
{

    [SerializeField] private CinemachineVirtualCameraBase characterCam;
    [SerializeField] private CinemachineVirtualCameraBase battleCamLeft;
    [SerializeField] private CinemachineVirtualCameraBase battleCamRight;
    [SerializeField] private CinemachineVirtualCameraBase wideCam;
    [SerializeField] private CinemachineFreeLook battleCamera;

    [SerializeField] private Transform _target;

    public bool IsPlayerMove
    {
        get { return isPlayerMove; }
        set
        {
            if(value == true)
            {
                isPlayerMove = true;
            }
            else
            {
                isPlayerMove = false;
            }
        }
    }
    private bool isPlayerMove = false;

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
        StartCoroutine(ResetMode());
    }

    IEnumerator ResetMode()
    {
        yield return new WaitForSeconds(1f);
    }
    void CamReset()
    {
        characterCam.Follow = null;
        characterCam.LookAt = null;
    }

    /*
    // ReSharper disable Unity.PerformanceAnalysis
    private void moveCam()
    {
        wideCam.gameObject.transform.Translate(PlayerInputManager.Instance.moveDirection * moveSpeed * Time.deltaTime, Space.World);
        wideCam.gameObject.transform.Translate(MoveCamWithMouse() * (moveSpeed * Time.deltaTime), Space.World);
    }*/

    private Vector3 MoveCamWithMouse()
    {
        Vector3 mouseCamMove = new Vector3();
        Vector3 worldPos = Camera.main.ScreenToViewportPoint(PlayerInputManager.Instance.mousePos);
        if (worldPos.x < 0.01f)
        {
            if(PlayerOutOfRange() % 2 != 0)
                mouseCamMove.x = -1;
        }
        if (worldPos.x > 0.99f)
        {
            if (PlayerOutOfRange() % 3 != 0)
                mouseCamMove.x = 1; 
        }
        if (worldPos.y < 0.01f)
        {
            if (PlayerOutOfRange() % 7 != 0)
                mouseCamMove.z = -1;
        }
        if (worldPos.y > 0.99f)
        {
            if (PlayerOutOfRange() % 5 != 0)
                mouseCamMove.z = 1;
        }
        return mouseCamMove;
    }

    private void AdjustFOV(float scrollValue)
    {
    }

    // 1 : inside 2 : left, 3 : right, 5 : top, 7 : bottom
    private int PlayerOutOfRange()
    {
        int outValue = 1;
        float px = _target.transform.position.x;
        float pz = _target.transform.position.z;
        //float tx = characterCam.gameObject.transform.position.x;
        //float tz = characterCam.gameObject.transform.position.z;

        float xOffset = px;
        float zOffset = pz;

        if (xOffset > 12)
            outValue *= 2;
        if (xOffset < -12)
            outValue *= 3;

        if (zOffset > 17)
            outValue *= 7;
        if (zOffset < -1)
            outValue *= 5;

        return outValue;
    }

    public void ConvertToCharacterCam() {
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

    public void ConvertWideCamera()
    {
        characterCam.Priority = 0;
        wideCam.Priority = 10;
        battleCamLeft.Priority = 0;
        battleCamRight.Priority = 0;
    }
}
