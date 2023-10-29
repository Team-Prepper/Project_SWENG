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
    private Animator _animator;

    private CharacterController _characterController;

    [SerializeField]
    private float _movementDuration = 1, _rotationDuration = 0.3f;

    private DicePoint _dicePoints;

    private GlowHighlight _glowHighlight;
    private Queue<Vector3> _pathPositions = new Queue<Vector3>();

    private void Start()
    {
        _glowHighlight = GetComponent<GlowHighlight>();
        _animator = GetComponentInChildren<Animator>();

        _dicePoints = GetComponent<DicePoint>();
        _dicePoints.SetPoint(0);
        _characterController = GetComponent<CharacterController>();
        _photonView = GetComponent<PhotonView>();
    }

    public void Deselect()
    {
        _glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        _glowHighlight.ToggleGlow();
    }

    public void NewMoveThroughPath(List<Vector3> currentPath)
    {
        _pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = _pathPositions.Dequeue();
        StartCoroutine(_RotationCoroutine(firstTarget, _rotationDuration));
    }

    private IEnumerator _RotationCoroutine(Vector3 endPosition, float rotationDuration)
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
        StartCoroutine(_NewMovementCoroutine(endPosition));
    }

    private IEnumerator _NewMovementCoroutine(Vector3 endPosition)
    {
        _photonView.RPC("SetPlayerOnHex",RpcTarget.All,0, transform.position);
        HexGrid.Instance.GetTileAt(HexCoordinate.ConvertFromVector3(transform.position)).Entity = null;

        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;

        HexCoordinate newHexPos = HexCoordinate.ConvertFromVector3(endPosition);
        NetworkCloudManager.Instance.CloudActiveFalse(newHexPos);
        Hex goalHex = HexGrid.Instance.GetTileAt(newHexPos);
        _dicePoints.UsePoint(goalHex.Cost);


        float timeElapsed = 0;

        while (timeElapsed < _movementDuration)
        {
            if (_animator)
                _animator.SetBool("IsWalk", true);

            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / _movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if (_pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(_RotationCoroutine(_pathPositions.Dequeue(), _rotationDuration));
        }
        else
        {
            Debug.Log("Movement finished!");
            _photonView.RPC("SetPlayerOnHex",RpcTarget.All,1, transform.position);
        }
        if (_animator)
            _animator.SetBool("IsWalk", false);
    }

    [PunRPC]
    public void SetPlayerOnHex(int type, Vector3 position)
    {
        if (type == 1)
        {
            HexGrid.Instance.GetTileAt(position).Entity = gameObject;
        }
        else
        {
            HexGrid.Instance.GetTileAt(position).Entity = null;
        }
    }

}
