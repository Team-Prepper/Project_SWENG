using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

class DiceValueInfor {
    public int value;
    public bool isReverse;

    public DiceValueInfor(int value, bool isReverse)
    {
        this.value = value;
        this.isReverse = isReverse;
    }
}

public class Dice : MonoBehaviour {

    private Dictionary<Vector3Int, DiceValueInfor> _diceInfor = new Dictionary<Vector3Int, DiceValueInfor>();

    public int Value { get; private set; }
    Vector3 originPos = new Vector3(0, 3, 0);

    bool _isRolling;

    [Space(20)]
    [SerializeField] float torqueMin = 100f;
    [SerializeField] float torqueMax = 200f;
    [SerializeField] private float throwStrength = 50;
    [SerializeField] private GameObject walls;
    [SerializeField] private float _waitTime = 1f;

    [SerializeField] private UnityEvent _diceSet;
    [SerializeField] private UnityEvent _diceStopEvent;

    float idleTime = 0.1f;
    private Vector3 lastPosition;

    Rigidbody rb;

    // rotation 
    private float rotationSpeed = 0.5f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _diceSet.Invoke();
        _isRolling = false;
    }

    public void SetDicD20()
    {
        _diceInfor = new Dictionary<Vector3Int, DiceValueInfor>
        {
            { new Vector3Int(-3, -9, -2), new DiceValueInfor(1, false) },
            { new Vector3Int(8, 6, -2), new DiceValueInfor(2, false) },
            { new Vector3Int(-5, -4, 8), new DiceValueInfor(3, false) },
            { new Vector3Int(-2, 6, -8), new DiceValueInfor(4, true) },
            { new Vector3Int(5, -4, -8), new DiceValueInfor(5, true) },
            { new Vector3Int(-8, 6, 2), new DiceValueInfor(6, false) },
            { new Vector3Int(3, -9, 2), new DiceValueInfor(7, false) },
            { new Vector3Int(2, 6, 8), new DiceValueInfor(8, false) },
            { new Vector3Int(-10, 0, -2), new DiceValueInfor(9, false) },
            { new Vector3Int(6, 0, 8), new DiceValueInfor(10, false) },
            { new Vector3Int(-6, 0, -8), new DiceValueInfor(11, true) },
            { new Vector3Int(10, 0, 2), new DiceValueInfor(12, false) },
            { new Vector3Int(-2, -5, -8), new DiceValueInfor(13, true) },
            { new Vector3Int(-3, 9, -2), new DiceValueInfor(14, false) },
            { new Vector3Int(8, -6, -2), new DiceValueInfor(15, false) },
            { new Vector3Int(-5, 4, 8), new DiceValueInfor(16, false) },
            { new Vector3Int(2, -5, 8), new DiceValueInfor(17, false) },
            { new Vector3Int(5, 4, -8), new DiceValueInfor(18, true) },
            { new Vector3Int(-8, -6, 2), new DiceValueInfor(19, false) },
            { new Vector3Int(3, 9, 1), new DiceValueInfor(20, false) }
        };

    }
    public void SetDicD6()
    {
        _diceInfor = new Dictionary<Vector3Int, DiceValueInfor>
        {
            { new Vector3Int( 0,  0,  1), new DiceValueInfor(1, false) },
            { new Vector3Int( 0,  1,  0), new DiceValueInfor(2, false) },
            { new Vector3Int(-1,  0,  0), new DiceValueInfor(3, false) },
            { new Vector3Int( 1,  0,  0), new DiceValueInfor(4, false) },
            { new Vector3Int( 0, -1,  0), new DiceValueInfor(5, false) },
            { new Vector3Int( 0,  0, -1), new DiceValueInfor(6, false) },
        };
    }

    public void Rolling()
    {
        if (_isRolling) return;

        _isRolling = true;
        walls.SetActive(true);
        this.gameObject.transform.localPosition = originPos;
        rb.useGravity = true;
        lastPosition = rb.position;

        rb.velocity = Vector3.up * throwStrength;
        rb.AddForce(Random.insideUnitSphere * Random.Range(50, 100), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueMax + torqueMin * Vector3.one);


        StartCoroutine(CheckIdle());
    }

    public IEnumerator CheckIdle()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            if(this.transform.localPosition.y < -10 )
            {
                this.transform.localPosition = originPos;
            }

            if (rb.position == lastPosition)
            {
                break;
            }
            lastPosition = rb.position;
        }

        Debug.Log("is Stop");
        ChkRoll();
    }
    public void ChkRoll()
    {
        float yDot, xDot, zDot;

        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up) * 10);
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up) * 10);
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up) * 10);

        if (_diceInfor.TryGetValue(new Vector3Int((int)xDot, (int)yDot, (int)zDot), out DiceValueInfor rollValue))
        {
            initialRotation = transform.rotation;
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, rollValue.isReverse ? 180 : 0, initialRotation.eulerAngles.z);

            Value = rollValue.value;

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
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;

        yield return new WaitForSeconds(_waitTime);

        _diceStopEvent.Invoke();
        _isRolling = false; 
    }

}