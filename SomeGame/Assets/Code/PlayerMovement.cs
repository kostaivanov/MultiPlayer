using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller2D))]
internal class PlayerMovement : PlayerComponents
{
    internal Vector2 inputVector;
    private float timeSinceJumpPressed = 0f;
    #region Jump variables
    [SerializeField] float timeToJumpApex = .4f;
    [SerializeField] float jumpApexHeight = 14;
    [SerializeField] float jumpCancelHeight = 2;
    [SerializeField] internal float jumpHeight = 8;

    private float jumpVelocity;
    internal float gravity;

    internal bool swimming;
    internal bool doubleJump;
    private bool jumpCanceled;
    private bool isJumping;
    #endregion

    #region Movement variables

    [SerializeField] internal float moveSpeed = 16;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.005f;
    internal Vector3 velocity;

    float velocityXSmoothing;

    private bool canMove;
    #endregion
    //private float jumpGravity;

    private bool canDoubleJump = false;





    #region Input actions
    [SerializeField] private InputActionReference jumpAction;
    //internal PlayerInput playerInput;
    internal PlayerInputActions playerInputActions;
    private InputActionMap playerGround;
    private InputActionMap playerWater;
    #endregion

    private PlayerStateManager playerStateManager;
    internal Controller2D controller2D;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        //playerInput = GetComponent<PlayerInput>();
        playerInputActions.PlayerGround.Enable();
        playerStateManager = GetComponent<PlayerStateManager>();
        //playerGround = playerInput.actions.FindActionMap("PlayerGround");
        //playerWater = playerInput.actions.FindActionMap("PlayerWater");
    }
    internal bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        controller2D = GetComponent<Controller2D>();
        playerInputActions.PlayerWater.Disable();
        CalculateGravityAndInitialVelocity();
        canMove = false;
    }
    private void CalculateGravityAndInitialVelocity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        if (jumpAction.action.ReadValue<float>() > 0 && controller2D.collisions.below)
        {
            timeSinceJumpPressed += Time.deltaTime;
        }
        else
        {
            timeSinceJumpPressed = 0f;
        }

        if (timeSinceJumpPressed > 0 && timeSinceJumpPressed < timeToJumpApex)
        {
            float t = timeSinceJumpPressed / timeToJumpApex;
            jumpVelocity = Mathf.Lerp(jumpApexHeight, jumpCancelHeight, t);
        }
        else
        {
            jumpVelocity = jumpApexHeight;
        }

        if (controller2D.collisions.above || controller2D.collisions.below)
        {
            velocity.y = 0;
        }
        if (playerInputActions.PlayerGround.Movement.IsPressed())
        {
            playerInputActions.PlayerGround.Enable();
            playerInputActions.PlayerWater.Disable();
            inputVector = playerInputActions.PlayerGround.Movement.ReadValue<Vector2>();
            canMove = true;
            this.transform.localScale = new Vector2(inputVector.x, 1);
        }
        else
        {
            inputVector = playerInputActions.PlayerGround.Movement.ReadValue<Vector2>();
            canMove = false;
        }

        if (jumpAction.action.triggered)
        {
            if (controller2D.collisions.below)
            {
                velocity.y = jumpVelocity;
                doubleJump = true;
                jumpCanceled = false;

            }
            else if (doubleJump && !jumpCanceled)
            {
                velocity.y = jumpVelocity;
                doubleJump = false;
                jumpCanceled = true;
            }
        }
        else
        {
            jumpCanceled = true;
        }
        float targetVelocityX = inputVector.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller2D.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller2D.Move(velocity * Time.deltaTime);
    }

}
