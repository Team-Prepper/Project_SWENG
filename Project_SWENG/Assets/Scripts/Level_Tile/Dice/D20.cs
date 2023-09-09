using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class D20 : MonoBehaviour
{
    [SerializeField] GameObject Dice;
    [Space(20)]
    [SerializeField] float torqueMin = 100f;
    [SerializeField] float torqueMax = 200f;
    [SerializeField] private float throwStrength = 50;
    [SerializeField] private GameObject walls;
   
    float idleTime = 0.1f;
    private Vector3 lastPosition;

    private Dictionary<Vector3Int, int> dicD20 = new Dictionary<Vector3Int, int>();
    private Dictionary<int, bool> d20Rotate = new Dictionary<int, bool>();
    
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
        setDicD20();
    }

    void setDicD20()
    {
        dicD20.Add(new Vector3Int(-3, -9, -2),  1);
        dicD20.Add(new Vector3Int( 8,  6, -2),  2);
        dicD20.Add(new Vector3Int(-5, -4,  8),  3);
        dicD20.Add(new Vector3Int(-2,  6, -8),  4);
        dicD20.Add(new Vector3Int( 5, -4, -8),  5);
        dicD20.Add(new Vector3Int(-8,  6,  2),  6);
        dicD20.Add(new Vector3Int( 3, -9,  2),  7);
        dicD20.Add(new Vector3Int( 2,  6,  8),  8);
        dicD20.Add(new Vector3Int(-10, 0, -2),  9);
        dicD20.Add(new Vector3Int( 6,  0,  8), 10);
        dicD20.Add(new Vector3Int(-6,  0, -8), 11);
        dicD20.Add(new Vector3Int(10,  0,  2), 12);
        dicD20.Add(new Vector3Int(-2, -5, -8), 13);
        dicD20.Add(new Vector3Int(-3,  9, -2), 14);
        dicD20.Add(new Vector3Int( 8, -6, -2), 15);
        dicD20.Add(new Vector3Int(-5,  4,  8), 16);
        dicD20.Add(new Vector3Int( 2, -5,  8), 17);
        dicD20.Add(new Vector3Int( 5,  4, -8), 18);
        dicD20.Add(new Vector3Int(-8, -6,  2), 19);
        dicD20.Add(new Vector3Int( 3,  9,  1), 20);
        d20Rotate.Add( 1,false); d20Rotate.Add( 6,false);
        d20Rotate.Add( 2,false); d20Rotate.Add( 7,false);
        d20Rotate.Add( 3,false); d20Rotate.Add( 8,false);
        d20Rotate.Add( 4,true); d20Rotate.Add( 9,false);
        d20Rotate.Add( 5,true); d20Rotate.Add(10,false);
        d20Rotate.Add(11,true); d20Rotate.Add(16,false);
        d20Rotate.Add(12,false); d20Rotate.Add(17,false);
        d20Rotate.Add(13,true); d20Rotate.Add(18,true);
        d20Rotate.Add(14,false); d20Rotate.Add(19,false);
        d20Rotate.Add(15,false); d20Rotate.Add(20,false);
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
        rb.AddForce(Vector3.forward * (throwStrength + Random.Range(30, 80)), ForceMode.Impulse);
        rb.AddForce(Vector3.right * Random.Range(30, 80), ForceMode.Impulse);
        rb.AddTorque(transform.forward * Random.Range(torqueMin, torqueMax) + transform.up * Random.Range(torqueMin, torqueMax)
                                                                            + transform.right * Random.Range(torqueMin, torqueMax));

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
        int rollValue = -1;
        
        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up) * 10);
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up)* 10);
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up)* 10);

        bool isSucces = dicD20.TryGetValue(new Vector3Int((int)xDot, (int)yDot, (int)zDot), out rollValue);

        if (isSucces)
        {
            initialRotation = transform.rotation;
            bool inverse;
            d20Rotate.TryGetValue(rollValue,out inverse);
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, inverse ? 180 : 0, initialRotation.eulerAngles.z);

            StartCoroutine(RotateSmoothly(rollValue));

        }
        else
        {
            rb.AddForce((Vector3.zero - transform.position).normalized * 10f, ForceMode.Impulse);
            walls.SetActive(false);
            StartCoroutine(CheckIdle());
        }
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
