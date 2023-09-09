using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollDice : MonoBehaviour
{
    [SerializeField] GameObject Dice;
    [Space(20)]
    [SerializeField] float torqueMin = 200f;
    [SerializeField] float torqueMax = 400f;
    [SerializeField] float throwStrength = 5000f;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] NTG ntg;

    Rigidbody rb;
    Unit unit;
    GameManager GM;

    float idleTime = 0.1f;
    private bool isCheckingIdle;
    private Vector3 lastPosition;
    
    

    void Awake()
    {
        GM = GameManager.Instance;
        rb = GetComponent<Rigidbody>();  
        unit = GetComponentInParent<Unit>();
    }

    private void Update()
    {
        if(GM.gamePhase == GameManager.Phase.SetDice)
        {
            SetDice();
        }
    }

    private void OnEnable()
    {
        rb.useGravity = false;
        Dice.SetActive(false);
        Dice.transform.localPosition = Vector3.zero;
        if (text != null)
            text.text = "";
    }

    private void SetDice()
    {
        PlayerInputBase.EventRollingDice += Rolling;
        Dice.SetActive(true);
        rb.AddTorque(transform.forward * Random.Range(torqueMin, torqueMax) + transform.up * Random.Range(torqueMin, torqueMax)
            + transform.right * Random.Range(torqueMin, torqueMax));
        Dice.transform.localPosition = Vector3.zero;
        this.transform.localPosition = new Vector3(0,3,0);
        if (text != null)
            text.text = "";

        GM.NextPhase(); // DiceRolling
    }

    void Rolling(object sender, EventArgs e)
    {
        StartCoroutine(RollingDice());
        PlayerInputBase.EventRollingDice -= Rolling;
    }

    IEnumerator RollingDice()
    {
        yield return new WaitForSeconds(.5f);
        rb.useGravity = true;
        lastPosition = rb.position;
        isCheckingIdle = true;

        unit.JumpAction();
        rb.AddForce(Vector3.up * throwStrength, ForceMode.Impulse);
        rb.AddForce((Vector3.zero - transform.position).normalized, ForceMode.Impulse);
        rb.AddTorque(transform.forward * Random.Range(torqueMin, torqueMax) + transform.up * Random.Range(torqueMin, torqueMax)
            + transform.right * Random.Range(torqueMin, torqueMax));

        if(text != null)
            text.text = "Rolling";

        StartCoroutine(CheckIdle());
    }

    public IEnumerator CheckIdle()
    {
        while (isCheckingIdle)
        {
            yield return new WaitForSeconds(idleTime);

            if (rb.position == lastPosition)
            {
                Debug.Log("is Stop");
                isCheckingIdle = false;
                OnIdle();
            }
            lastPosition = rb.position;
        }
    }

    public void OnIdle()
    {
        ChkRoll();
    }

    public void ChkRoll()
    {
        float yDot, xDot, zDot;
        int rollValue = -1;

        yDot = Mathf.Round(Vector3.Dot(transform.up.normalized, Vector3.up));
        zDot = Mathf.Round(Vector3.Dot(transform.forward.normalized, Vector3.up));
        xDot = Mathf.Round(Vector3.Dot(transform.right.normalized, Vector3.up));

        switch (yDot)
        {
            case 1:
                rollValue = 2;
                break;
            case -1:
                rollValue = 5;
                break;
        }
        switch (xDot)
        {
            case 1:
                rollValue = 4;
                break;
            case -1:
                rollValue = 3;
                break;
        }
        switch (zDot)
        {
            case 1:
                rollValue = 1;
                break;
            case -1:
                rollValue = 6;
                break;
        }
        StartCoroutine(Result(rollValue));
        StartCoroutine(DisableDice());
    }

    IEnumerator Result(int value)
    {
        int dicePoints = value * 2;
        if (text != null)
            text.text = dicePoints.ToString();
        if (unit != null)
            unit.dicePoints = dicePoints;
        ntg.convert(value);
        GM.NextPhase();         // Movement
        yield return null;
    }

    IEnumerator DisableDice()
    {
        rb.useGravity = false;
        yield return new WaitForSeconds(2f);
        Dice.SetActive(false);
        if (text != null)
            text.text = "";
    }
}
