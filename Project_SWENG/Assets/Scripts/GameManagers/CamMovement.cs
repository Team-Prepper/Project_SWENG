using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMovement : MonoBehaviour
{
    public static CamMovement Instance;
    [SerializeField] float moveSpeed = 5f;
    
    [Header("FOV")]
    private float minFOV = 10.0f;
    private float maxFOV = 100.0f;
    
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isCamMove = false;
    PlayerInputBase playerInput;
    
    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        playerInput = PlayerInputBase.Instance;
    }

    public void CamSetToPlayer(GameObject player)
    {
        isCamMove = false;
        this.player = player;
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

    void CamSet(object sender, IntEventArgs e)
    {
        if(player != null)
            CamSetToPlayer(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCamMove)
            moveCam();
        AdjustFOV(playerInput.scrollValue);
    }

    private void moveCam()
    {
        CamReset();
        transform.Translate(playerInput.moveDirection * moveSpeed * Time.deltaTime, Space.World);
        transform.Translate(MoveCamWithMouse() * moveSpeed * Time.deltaTime, Space.World);
    }

    private Vector3 MoveCamWithMouse()
    {
        Vector3 mouseCamMove = new Vector3();
        Vector3 worldPos = Camera.main.ScreenToViewportPoint(playerInput.mousePos);
        if (worldPos.x < 0.01f)
        {
            mouseCamMove.x = -1;
        }
        if (worldPos.x > 0.99f)
        {
            mouseCamMove.x = 1; 
        }
        if (worldPos.y < 0.01f)
        {
            mouseCamMove.z = -1;
        }
        if (worldPos.y > 0.99f)
        {
            mouseCamMove.z = 1;
        }
        return mouseCamMove;
    }
    private void AdjustFOV(float scrollValue)
    {
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        float newFOV = Mathf.Clamp(currentFOV - (scrollValue/12), minFOV, maxFOV);

        virtualCamera.m_Lens.FieldOfView = newFOV;
    }
}
