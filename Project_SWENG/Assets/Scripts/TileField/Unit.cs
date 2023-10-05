using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Unit : MonoBehaviour {

    [Header("Ref")]
    [SerializeField]
    private Animator animator;

    private CharacterController _characterController;

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;
    
    [Header("Movemet")]
    public int dicePoints = 0;
    private Vector3 destination;
    private float moveSpeed = 5;

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    private void Start()
    {
        glowHighlight = GetComponent<GlowHighlight>();
        animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
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
        HexGrid.Instance.GetTileAt(HexGrid.Instance.GetClosestHex(transform.position)).Entity = null;
        
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        
        Vector3Int newHexPos = HexCoordinates.ConvertPositionToOffset(endPosition);
        CloudBox.Instance.CloudActiveFalse(newHexPos);
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
            HexGrid.Instance.GetTileAt(newHexPos).Entity = gameObject;
        }
        if (animator)
            animator.SetBool("IsWalk", false);
    }
    

}
