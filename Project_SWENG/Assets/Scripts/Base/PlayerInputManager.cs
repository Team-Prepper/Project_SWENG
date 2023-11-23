using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoSingleton<PlayerInputManager>
{
    // LOCAL PLAYER
    public LayerMask selectionMask;
    private Hex hex = null;
    GameObject originObj = null;

    PlayerInputSystem playerControls;

    Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    Vector2 movementInput;
    public Vector3 mousePos;
    public Vector3 moveDirection;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    public float scrollValue;

    public bool inputQ = false;
    public bool inputE = false;
    public bool inputC = false;
    public bool inputV = false;
    public bool input1 = false;
    public bool input2 = false;
    public bool input3 = false;
    public bool input4 = false;

    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<Vector3> PointerClick;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputSystem();

            playerControls.MouseInput.MouseLClick.performed += InputMouseLClick;
            playerControls.BaseInput.Select.performed += HandleUnitSelect;

            playerControls.BaseInput.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.MouseInput.MouseDelta.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.MouseInput.PointerPosition.performed += i => mousePos = i.ReadValue<Vector2>();
            playerControls.MouseInput.MouseScroll.performed += i => scrollValue = i.ReadValue<float>();


            playerControls.KeyInput.ActionQ.performed += i => inputQ = true;
            playerControls.KeyInput.ActionQ.canceled += i => inputQ = false;
            playerControls.KeyInput.ActionE.performed += i => inputE = true;
            playerControls.KeyInput.ActionE.canceled += i => inputE = false;
            playerControls.KeyInput.ActionC.performed += i => inputC = true;
            playerControls.KeyInput.ActionC.canceled += i => inputC = false;
            playerControls.KeyInput.ActionV.performed += i => inputV = true;
            playerControls.KeyInput.ActionV.canceled += i => inputV = false;

            playerControls.KeyInput.Action1.performed += i => input1 = true;
            playerControls.KeyInput.Action1.canceled += i => input1 = false;
            playerControls.KeyInput.Action2.performed += i => input2 = true;
            playerControls.KeyInput.Action2.canceled += i => input2 = false;
            playerControls.KeyInput.Action3.performed += i => input3 = true;
            playerControls.KeyInput.Action3.canceled += i => input3 = false;
            playerControls.KeyInput.Action4.performed += i => input4 = true;
            playerControls.KeyInput.Action4.canceled += i => input4 = false;
        }

        playerControls.Enable();
    }

    private void Update()
    {
        HandleAllInputs();
        MouseMove();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void InputMouseLClick(InputAction.CallbackContext context)
    {
        PointerClick?.Invoke(mousePos);
    }

    public void HandleUnitSelect(InputAction.CallbackContext context)
    {
        
        Debug.Log("SpaceBar");
        Debug.Log(PhotonNetwork.NetworkClientState);
        
        if (!GameManager.IsNull())
        {
            OnUnitSelected?.Invoke(GameManager.Instance.player);
        }
        
    }


    private void MouseMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100, selectionMask)) return;

        GameObject selectedObject = hit.collider.gameObject;

        if (originObj == selectedObject) return;

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
