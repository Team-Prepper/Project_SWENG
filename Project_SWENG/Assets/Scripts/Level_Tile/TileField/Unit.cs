using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField]
    private Animator animator;
    
    NavMeshAgent agent;

    [Header("Movemet")]
    public int dicePoints = 0;

    private Vector3 curPos;
    public Vector3 CurPos { get => curPos; set => curPos = value; }

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;
    public static event EventHandler<IntEventArgs> EventDicePoint;

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighlight>();
        animator = GetComponentInChildren<Animator>();  
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        curPos = transform.position;
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
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(MovementCoroutine(firstTarget));
    }
    
    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3Int curHexPos = HexGrid.Instance.GetClosestHex(transform.position);
        HexGrid.Instance.GetTileAt(curHexPos).Entity = null;

        Debug.Log("Move to " + endPosition);

        Vector3Int newHexPos = HexCoordinates.ConvertPositionToOffset(endPosition);
        CloudBox.Instance?.CloudActiveFalse(newHexPos);
        Hex endHex = HexGrid.Instance.GetTileAt(newHexPos);

        dicePoints -= endHex.cost;
        endHex.Entity = this.gameObject;
        
        EventDicePoint?.Invoke(this, new IntEventArgs(dicePoints));
        agent.ResetPath();
        agent.SetDestination(endPosition);

        while (!isArrive())
        {
            if(animator)
                animator.SetBool("IsWalk", true);

            yield return null;
        }

        transform.position = endPosition;
        curPos = endPosition;

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(MovementCoroutine(pathPositions.Dequeue()));
        }
        else
        {
            Debug.Log("Movement finished!");

            if (animator)
                animator.SetBool("IsWalk", false);

            MovementFinished?.Invoke(this);
        }
    }

    private bool isArrive()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                Debug.Log("isArrive!");
                return true;
            }
        }
        return false;
    }

    private void GetDicePointFromDice(object sender, IntEventArgs e)
    {
        dicePoints = e.Value;
        EventDicePoint?.Invoke(this, new IntEventArgs(dicePoints));
    }

    private void DamagedAction(object sender, IntEventArgs e)
    {
        if(e.Value > 0)
            animator.SetTrigger("Hit");
        else
            animator.SetTrigger("Die");
    }
}
