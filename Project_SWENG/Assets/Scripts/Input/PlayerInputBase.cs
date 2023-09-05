using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputBase : MonoBehaviour
{
    public static PlayerInputBase Instance;
    
    [SerializeField]
    private Camera mainCamera;
    public LayerMask selectionMask;

    public Vector2 inputVector;
    public Vector2 inputMousePos;
    public Vector3 moveDirection;
    public Vector3 mousePos;


    GameManager GM;
    public float scrollValue;
    public bool input1;
    public bool input2;
    public bool input3;
    public bool input4;
    public bool inputQ;
    public bool inputE;
    public bool inputC;
    public bool inputV;
    public bool inputShift;
    public bool inputML;
    public bool inputMR;
    

    [SerializeField]
    private InputActionReference pointerPos, mouseLClickAction, mouseRClickAction, mouseScroll;
    
    [SerializeField]
    private InputActionReference inputMouseDelta;
    [SerializeField]
    private InputActionReference inputAction1;
    [SerializeField]
    private InputActionReference inputAction2;
    [SerializeField]
    private InputActionReference inputAction3;
    [SerializeField]
    private InputActionReference inputAction4;
    [SerializeField]
    private InputActionReference inputActionQ;
    [SerializeField]
    private InputActionReference inputActionE;
    [SerializeField]
    private InputActionReference inputActionC;
    [SerializeField]
    private InputActionReference inputActionV;
    [SerializeField]
    private InputActionReference inputActionShift;
    
    public static event EventHandler EventRollingDice;
    public static event EventHandler EventItemPickup;

    [Header("ClickEvent")]
    public UnityEvent<Vector3> PointerClick;
    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<Vector3> MovePosClick;
    
    GameObject originObj = null;
    [SerializeField] GameObject player = null;
    Hex hex = null;

    public bool view;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        GM = GameManager.Instance;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        SetPlayer();
    }

    private void OnEnable()
    {
        mouseLClickAction.action.performed += InputMouseLClick;
        mouseRClickAction.action.performed += InputMouseRClick;
        mouseScroll.action.performed += x => scrollValue = x.action.ReadValue<float>();
        
        //Actions
        inputAction1.action.started += x => input1 = true;
        inputAction2.action.started += x => input2 = true;
        inputAction3.action.started += x => input3 = true;
        inputAction4.action.started += x => input4 = true;
        inputActionQ.action.started += x => inputQ = true;
        inputActionE.action.started += x => inputE = true;
        inputActionC.action.started += x => inputC = true;
        inputActionV.action.started += x => inputV = true;
        mouseLClickAction.action.started += x => inputML = true;
        mouseRClickAction.action.started += x => inputMR = true;
        inputActionShift.action.started += x => inputShift = true;

        inputAction1.action.canceled += x => input1 = false;
        inputAction2.action.canceled += x => input2 = false;
        inputAction3.action.canceled += x => input3 = false;
        inputAction4.action.canceled += x => input4 = false;
        inputActionQ.action.canceled += x => inputQ = false;
        inputActionE.action.canceled += x => inputE = false;
        inputActionC.action.canceled += x => inputC = false;
        inputActionV.action.canceled += x => inputV = false;
        mouseLClickAction.action.canceled += x => inputML = false;
        mouseRClickAction.action.canceled += x => inputMR = false;
        inputActionShift.action.canceled += x => inputShift = false;
    }

    private void OnDisable()
    { 
        mouseLClickAction.action.performed -= InputMouseLClick;
        mouseRClickAction.action.performed -= InputMouseRClick;
    }

    private void InputMouseLClick(InputAction.CallbackContext context)
    {
        
        PointerClick?.Invoke(mousePos);
    }
    
    private void InputMouseRClick(InputAction.CallbackContext context)
    {
        MovePosClick?.Invoke(mousePos);
    }

    void Update()
    {
        mousePos = pointerPos.action.ReadValue<Vector2>();

        inputMousePos = inputMouseDelta.action.ReadValue<Vector2>();
        MouseMove();
        moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    private void MouseMove()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100, selectionMask))
        {
            GameObject selectedObject = hit.collider.gameObject;
            if (originObj != selectedObject)
            {
                if (hex != null)
                {
                    // origin off
                    hex.OnMouseToggle();
                }
                hex = selectedObject.GetComponent<Hex>();
                if (hex != null)
                {
                    // New Obj on
                    hex.OnMouseToggle();
                }
            }
        }
    }
    

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        inputVector = newMoveDirection;
    }

    public void OnDiceRolling(InputValue value)
    {
        if(GM.gamePhase == GameManager.Phase.DiceRolling)
        {
            if (value.Get<float>() == 1)
            {
                EventRollingDice?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void OnSelect(InputValue value)
    {
        if (value.Get<float>() == 1)
        {
            if(player != null)
            {
                OnUnitSelected?.Invoke(player);
            }
            else
            {
                SetPlayer();
            }
        }
    }

    public void OnItemPickup(InputValue value)
    {
        if (value.Get<float>() == 1)
        {
            EventItemPickup?.Invoke(this, EventArgs.Empty);
        }
    }
    
    

    void SetPlayer()
    {
        player = GM.player;
    }
}
