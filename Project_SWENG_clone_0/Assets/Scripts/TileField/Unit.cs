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

    private NavMeshAgent agent;

    [Header("Movemet")]
    public int dicePoints = 0;

    private GlowHighlight glowHighlight;

    public event Action<Unit> MovementFinished;
    public static event EventHandler<IntEventArgs> EventDicePoint;

    private void Start()
    {
        glowHighlight = GetComponent<GlowHighlight>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    public void MoveThroughPath(List<Vector3> currentPath)
    {
        Queue<Vector3> pathPositions = new Queue<Vector3>(currentPath);
        StartCoroutine(MovementCoroutine(pathPositions));
    }

    private IEnumerator MovementCoroutine(Queue<Vector3> pathPositions)
    {
        HexGrid.Instance.GetTileAt(HexGrid.Instance.GetClosestHex(transform.position)).Entity = null;

        Debug.Log("Move to " + pathPositions);

        if (animator)
            animator.SetBool("IsWalk", true);

        while (true)
        {
            Vector3 goalPos = pathPositions.Dequeue();
            EventDicePoint?.Invoke(this, new IntEventArgs(dicePoints));
            agent.ResetPath();
            agent.SetDestination(goalPos);

            Vector3Int newHexPos = HexCoordinates.ConvertPositionToOffset(goalPos);
            CloudBox.Instance.CloudActiveFalse(newHexPos);
            Hex goalHex = HexGrid.Instance.GetTileAt(newHexPos);
            dicePoints -= goalHex.cost;

            while (!_IsArrive())
            {
                yield return null;
            }

            transform.position = goalPos;

            if (pathPositions.Count == 0)
            {
                Debug.Log("Movement finished!");
                HexGrid.Instance.GetTileAt(newHexPos).Entity = gameObject;

                break;
            }

            Debug.Log("Selecting the next position!");
        }

        if (animator)
            animator.SetBool("IsWalk", false);

        MovementFinished?.Invoke(this);
    }

    private bool _IsArrive()
    {
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) return false;

        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        {
            Debug.Log("isArrive!");
            return true;
        }

        return false;
    }

}
