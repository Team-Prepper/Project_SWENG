using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[SelectionBase]
public class NetworkUnit : MonoBehaviourPun
{

    [Header("Network")] 
    private PhotonView _photonView;
    
    [Header("Ref")]
    [SerializeField]
    private Animator animator;

    private CharacterController _characterController;

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    public int dicePoints { get; set; }

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    private void Start()
    {
        glowHighlight = GetComponent<GlowHighlight>();
        animator = GetComponentInChildren<Animator>();

        dicePoints = 0;
        _characterController = GetComponent<CharacterController>();
        _photonView = GetComponent<PhotonView>();
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    public void NewMoveThroughPath(List<Vector3> currentPath)
    {
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration; // 0-1
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation;
        }
        StartCoroutine(NewMovementCoroutine(endPosition));
    }

    private IEnumerator NewMovementCoroutine(Vector3 endPosition)
    {
        _photonView.RPC("SetPlayerOnHex",RpcTarget.All,0);
        HexGrid.Instance.GetTileAt(HexGrid.GetClosestHex(transform.position)).Entity = null;

        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;

        Vector3Int newHexPos = HexGrid.GetClosestHex(endPosition);
        NetworkCloudManager.Instance.CloudActiveFalse(newHexPos);
        Hex goalHex = HexGrid.Instance.GetTileAt(newHexPos);
        dicePoints -= goalHex.cost;


        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            if (animator)
                animator.SetBool("IsWalk", true);

            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement finished!");
            _photonView.RPC("SetPlayerOnHex",RpcTarget.All,1);
        }
        if (animator)
            animator.SetBool("IsWalk", false);
    }

    [PunRPC]
    private void SetPlayerOnHex(int type, Vector3 position)
    {
        if (type == 1)
        {
            HexGrid.Instance.GetTileAt(HexGrid.GetClosestHex(position)).Entity = gameObject;
        }
        else
        {
            HexGrid.Instance.GetTileAt(HexGrid.GetClosestHex(position)).Entity = null;
        }
    }

}
