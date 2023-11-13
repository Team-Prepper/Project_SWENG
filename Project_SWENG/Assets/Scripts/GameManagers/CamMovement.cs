using Cinemachine;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class CamMovement : MonoSingleton<CamMovement>
{
    [SerializeField] float moveSpeed = 5f;

    [Header("FOV")] 
    private float _defFov = 60.0f;
    private float _minFOV = 10.0f;
    private float _maxFOV = 100.0f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineFreeLook battleCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isCamMove = false;

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
                isCamMove = true;
            }
        }
    }
    private bool isPlayerMove = false;
    
    private void Awake()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void CamSetToPlayer(GameObject player)
    {
        isCamMove = false;
        this.player = player;
        virtualCamera.m_Lens.FieldOfView = _defFov;
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        StartCoroutine(ResetMode());
    }

    IEnumerator ResetMode()
    {
        yield return new WaitForSeconds(1f);
        isCamMove = true;
    }
    void CamReset()
    {
        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlayerMove == true)
        {
            isCamMove = false;
            virtualCamera.m_Lens.FieldOfView = _defFov;
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
        else if (isCamMove)
        {
            moveCam();
            AdjustFOV(PlayerInputManager.Instance.scrollValue);
        }
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void moveCam()
    {
        CamReset();
        virtualCamera.gameObject.transform.Translate(PlayerInputManager.Instance.moveDirection * moveSpeed * Time.deltaTime, Space.World);
        virtualCamera.gameObject.transform.Translate(MoveCamWithMouse() * (moveSpeed * Time.deltaTime), Space.World);
    }

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
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        float newFOV = Mathf.Clamp(currentFOV - (scrollValue/12), _minFOV, _maxFOV);

        virtualCamera.m_Lens.FieldOfView = newFOV;
    }

    // 1 : inside 2 : left, 3 : right, 5 : top, 7 : bottom
    private int PlayerOutOfRange()
    {
        int outValue = 1;
        float px = player.transform.position.x;
        float pz = player.transform.position.z;
        float tx = virtualCamera.gameObject.transform.position.x;
        float tz = virtualCamera.gameObject.transform.position.z;

        float xOffset = px - tx;
        float zOffset = pz - tz;

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
        virtualCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
        isCamMove = false;  
        
        if (player != null)
        {
            Transform camRoot = player.transform.Find("CamRoot");
            battleCamera.LookAt = player.transform;
            battleCamera.Follow = camRoot;
        }
    }

    public void ConvertMovementCamera()
    {
        isCamMove = true;
        virtualCamera.gameObject.SetActive(true);
        battleCamera.gameObject.SetActive(false);
    }
}
