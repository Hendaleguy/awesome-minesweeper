using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Game Management")]
    [SerializeField]
    private GameObject board;
    
    [Header("Movement")]
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float lookSpeed;
    [SerializeField]
    private float jumpForce;

    [Header("Interaction")]
    [SerializeField]
    private float clickRange;
    
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _attackAction;
    private InputAction _jumpAction;
    
    private Rigidbody _playerBody;

    private float _cameraPitch;
    private float _distToGround;

    private GameObject targetCellObject;
    private Cell targetCell;
    private Board gameBoard;
    
    // private void ClickDown()
    // {
    //     if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, clickRange))
    //     {
    //         return;
    //     }
    //     
    //     Debug.Log("Got something");
    //     
    //     if (hit.collider.gameObject != targetCellObject)
    //     {
    //         Debug.Log("New object found: " + targetCellObject);
    //         SetTargetCell(hit.collider.gameObject);
    //     }
    //         
    //     targetCell.Click();
    // }

    // private void OnClickRelease()
    // {
    //     
    // }
    
    // private void SetTargetCell(GameObject newTargetCellObject)
    // {
    //     if (targetCell)
    //     {
    //         targetCell.UnClick();
    //     }
    //     
    //     if (!newTargetCellObject.CompareTag("Cell"))
    //     {
    //         Debug.Log("Not a cell");
    //         targetCellObject = null;
    //         targetCell = null;
    //         return;
    //     }
    //     targetCellObject = newTargetCellObject;
    //     targetCell = newTargetCellObject.GetComponent<Cell>();
    // }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
    }

    private void HandleJump()
    {
        if (!IsGrounded())
        {
            return;
        }
        Vector3 jumpForceVector = Vector3.up * jumpForce;
        _playerBody.AddForce(jumpForceVector, ForceMode.Impulse);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _jumpAction = InputSystem.actions.FindAction("Jump");

        _playerBody = GetComponent<Rigidbody>();
        
        _distToGround = GetComponent<CapsuleCollider>().height / 2;

        gameBoard = board.GetComponent<Board>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 moveVector = _moveAction.ReadValue<Vector2>() * moveSpeed;
        Vector3 forceVector = (transform.forward * moveVector.y + transform.right * moveVector.x);
        _playerBody.AddForce(forceVector, ForceMode.Force);

        UpdateMouseLook();
        
        // if (_attackAction.IsPressed()) ClickDown();

        if (_attackAction.WasReleasedThisFrame()) OnClickRelease();

        if (_attackAction.WasPressedThisFrame()) OnClickDown();

        if (_jumpAction.IsPressed()) HandleJump();
    }

    private void UpdateMouseLook()
    {
        Vector2 mouseDelta = _lookAction.ReadValue<Vector2>() * lookSpeed;
        _cameraPitch = Mathf.Clamp(_cameraPitch - mouseDelta.y, -90f, 90f);
        
        playerCamera.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
        
        transform.Rotate(Vector3.up * mouseDelta.x);
        
    }

    private void OnClickDown()
    {
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, clickRange))
        {
            return;
        }
        // GameObject targetObject = hit.collider.gameObject;
        //
        // if (targetObject.CompareTag("Cell"))
        // {
        //     targetObject.GetComponent<Cell>().Click();
        // }
        GameObject targetObject = hit.collider.gameObject;

        if (targetObject.CompareTag("Cell"))
        {
            gameBoard.HandleClick(hit);
        }
    }

    private void OnClickRelease()
    {
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, clickRange))
        {
            return;
        }
        GameObject targetObject = hit.collider.gameObject;

        if (targetObject.CompareTag("Cell"))
        {
            gameBoard.HandleClickRelease(hit);
        }
    }
    
}
