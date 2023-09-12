using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

class DiceInfor {
    public int value;
    public bool isReverse;

    public DiceInfor(int value, bool isReverse)
    {
        this.value = value;
        this.isReverse = isReverse;
    }
}

public class Dice : MonoBehaviour
{
    [Space(20)]
    [SerializeField] float torqueMin = 100f;
    [SerializeField] float torqueMax = 200f;
    [SerializeField] private float throwStrength = 50;
    [SerializeField] private GameObject walls;

    [SerializeField] private UnityEvent _diceSet;
   
    float idleTime = 0.1f;
    private Vector3 lastPosition;

    private Dictionary<Vector3Int, DiceInfor> _diceInfor = new Dictionary<Vector3Int, DiceInfor>();

    Rigidbody rb;

    // rotation 
    private float rotationSpeed = 0.5f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    public static event EventHandler<IntEventArgs> EventDiceStop;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _SetDicD20();
    }

    void _SetDicD20()
    {
        _diceInfor = new Dictionary<Vector3Int, DiceInfor>();

        _diceInfor.Add(new Vector3Int(-3, -9, -2), new DiceInfor(1, false));
        _diceInfor.Add(new Vector3Int(8, 6, -2), new DiceInfor(2, false));
        _diceInfor.Add(new Vector3Int(-5, -4, 8), new DiceInfor(3, false));
        _diceInfor.Add(new Vector3Int(-2, 6, -8), new DiceInfor(4, true));
        _diceInfor.Add(new Vector3Int(5, -4, -8), new DiceInfor(5, true));
        _diceInfor.Add(new Vector3Int(-8, 6, 2), new DiceInfor(6, true));
        _diceInfor.Add(new Vector3Int(3, -9, 2), new DiceInfor(7, false));
        _diceInfor.Add(new Vector3Int(2, 6, 8), new DiceInfor(8, false));
        _diceInfor.Add(new Vector3Int(-10, 0, -2), new DiceInfor(9, false));
        _diceInfor.Add(new Vector3Int(6, 0, 8), new DiceInfor(10, false));
        _diceInfor.Add(new Vector3Int(-6, 0, -8), new DiceInfor(11, false));
        _diceInfor.Add(new Vector3Int(10, 0, 2), new DiceInfor(12, false));
        _diceInfor.Add(new Vector3Int(-2, -5, -8), new DiceInfor(13, true));
        _diceInfor.Add(new Vector3Int(-3, 9, -2), new DiceInfor(14, false));
        _diceInfor.Add(new Vector3Int(8, -6, -2), new DiceInfor(15, false));
        _diceInfor.Add(new Vector3Int(-5, 4, 8), new DiceInfor(16, false));
        _diceInfor.Add(new Vector3Int(2, -5, 8), new DiceInfor(17, false));
        _diceInfor.Add(new Vector3Int(5, 4, -8), new DiceInfor(18, true));
        _diceInfor.Add(new Vector3Int(-8, -6, 2), new DiceInfor(19, false));
        _diceInfor.Add(new Vector3Int(3, 9, 1), new DiceInfor(20, false));

    }
    void _SetDicD6()
    {
        _diceInfor = new Dictionary<Vector3Int, DiceInfor>();

        // 주사위 6에 관련된 딕셔너리

    }

    public void Rolling()
    {
        walls.SetActive(true);
        StartCoroutine(RollingDice());
    }
    IEnumerator RollingDice()
    {
        yield return new WaitForSeconds(.5f);
        rb.useGravity = true;
        lastPosition = rb.position;
        
        rb.AddForce(Vector3.up * throwStrength, ForceMode.Impulse);
        rb.AddForce(Vector3.forward * (throwStrength + Random.Range(-100, 100)), ForceMode.Impulse);
        rb.AddForce(Vector3.right * Random.Range(-100, 100), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueMax + torqueMin * Vector3.one);


        StartCoroutine(CheckIdle());
    }
    
    public IEnumerator CheckIdle()
    {
        bool isCheckingIdle = true;
        while (isCheckingIdle)
        {
            yield return new WaitForSeconds(idleTime);

            if (rb.position == lastPosition)
            {
                Debug.Log("is Stop");
                isCheckingIdle = false;
                ChkRoll();
            }
            lastPosition = rb.position;
        }
    }
    public void ChkRoll()
    {
        float yDot, xDot, zDot;
        
        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up) * 10);
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up)* 10);
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up)* 10);

        if (_diceInfor.TryGetValue(new Vector3Int((int)xDot, (int)yDot, (int)zDot), out DiceInfor rollValue))
        {
            initialRotation = transform.rotation;
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, rollValue.isReverse ? 180 : 0, initialRotation.eulerAngles.z);

            StartCoroutine(RotateSmoothly(rollValue.value));

            return;

        }

        rb.AddForce(Vector3.one, ForceMode.Impulse);
        walls.SetActive(false);
        StartCoroutine(CheckIdle());
    }
    
    private IEnumerator RotateSmoothly(int rollValue)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < rotationSpeed)
        {
            float t = elapsedTime / rotationSpeed;
            Quaternion newRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            transform.rotation = newRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        EventDiceStop?.Invoke(this, new IntEventArgs(rollValue));
    }
    
}
