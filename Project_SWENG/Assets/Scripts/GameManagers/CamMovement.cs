using Cinemachine;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class CamMovement : MonoSingleton<CamMovement>
{

    [SerializeField] private CinemachineVirtualCameraBase characterCam;
    [SerializeField] private CinemachineVirtualCameraBase wideCam;
    [SerializeField] private CinemachineFreeLook battleCamera;

    [SerializeField] private GameObject player;

    private bool _isAttackPhase;

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
    
    private void Awake()
    {
        if (characterCam == null)
            characterCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetCamTarget(GameObject player)
    {
        this.player = player;
        characterCam.Follow = player.transform;
        characterCam.LookAt = player.transform;
        wideCam.Follow = player.transform;
        wideCam.LookAt = player.transform;
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
        float px = player.transform.position.x;
        float pz = player.transform.position.z;
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

    public void ConvertBattleCamera()
    {
        //characterCam.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
        
        if (player != null)
        {
            Transform camRoot = player.transform.Find("CamRoot");
            battleCamera.LookAt = player.transform;
            battleCamera.Follow = camRoot;
        }
    }

    public void ConvertCharacterCam() {
        characterCam.Priority = 10;
        wideCam.Priority = 0;
    }

    public void ConvertWideCamera()
    {
        characterCam.Priority = 0;
        wideCam.Priority = 10;
    }
}
