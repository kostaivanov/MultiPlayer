using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (BoxCollider2D))]
internal class PlayerMovement : PlayerComponents
{
    #region Constants
    private const float minimumVelocity_X = 0.5f;
    private const float minimumFallingVelocity_Y = -2f;
    private const float groundCheckRadius = 0.1f;
    #endregion
    private const float skinWidth = 0.025f;
    [SerializeField] internal int verticalRayCount = 4;
    private float verticalRaySpacing;

    [SerializeField] private float speed;
    [SerializeField] private LayerMask ground;
    private float moveBoxDownValue = 0.1f;

    private float gravity = -20;
    private Vector3 velocity;
    private bool moving;
    private float direction;

    private bool canMove;
    PlayerInputActions playerInputActions;
    private PlayerJump playerJump;
    RaycastOrigins raycastOrigings;
    //private Control control;
    //private InputAction movement;
    bool grounded = false;
    struct RaycastOrigins
    {
        internal Vector2 bottomLeft, bottomRight;
    }
   
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private void Awake()
    {
        //control = new Control();
        //control.Player.Movement.performed += context => direction = context.ReadValue<float>();
        //control.Player.Movement.canceled += context => direction = 0;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerJump = GetComponent<PlayerJump>();
    }

    private void OnEnable()
    {
        //control.Player.Enable();
    }

    private void OnDisable()
    {
        //control.Player.Disable();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moving = false;
        CanMove = true;
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update()
    {
        VerticalCollisions();
        Debug.Log(grounded);

        //velocity.x += gravity * Time.deltaTime;
        //MoveBody(velocity * Time.deltaTime);


        //Debug.Log("grounded = " + CheckIfGrounded());
    }

    private void FixedUpdate()
    {     
        if (playerInputActions.Player.Movement.IsPressed())
        {
            Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            MoveBody(inputVector);
            moving = true;
        }
        else if(playerJump.swimming == true && playerInputActions.Player.Swimming.IsPressed())
        {
            moving = false;
            Vector2 inputVector = playerInputActions.Player.Swimming.ReadValue<Vector2>();
            MoveBody(inputVector);
        }
        else
        {
            moving = false;
        }

        //if (moving == true)
        //{
        //    rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
        //    this.transform.localScale = new Vector2(direction * 0.65f, 0.65f);
        //    //rigidBody.MovePosition(rigidBody.position + new Vector2(direction * speed, 0) * Time.deltaTime);
        //}  
    }
    private void UpdateRaycastOrigins()
    {
        Bounds bounds = base.collider2D.bounds;
        bounds.Expand(skinWidth * -2);
        raycastOrigings.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigings.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = base.collider2D.bounds;
        bounds.Expand(skinWidth * -2);
        Debug.Log(bounds);
        verticalRayCount = Math.Clamp(verticalRayCount, 2, int.MaxValue);

        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private bool VerticalCollisions()
    {
        //float directionY = Math.Sign(velocity.y);
        //float rayLength = Math.Abs(velocity.y) + skinWidth;
        //Debug.Log("raylength = " + rayLength);

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigings.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, skinWidth, ground);

            grounded = hit;

            Debug.DrawRay(raycastOrigings.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
        }
        return grounded;
    }

    private void MoveBody(Vector2 inputVector)
    {
        UpdateRaycastOrigins();
        VerticalCollisions();
        rigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * speed, ForceMode2D.Force);
        //transform.Translate(inputVector);
    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
        Debug.Log("State = " + state);
    }



    //private float FindDirection()
    //{
    //    direction = Input.GetAxisRaw("Horizontal");
    //    return direction;
    //}

    internal bool CheckIfGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(base.collider2D.bounds.center, base.collider2D.bounds.size, 0f, Vector2.down, moveBoxDownValue, base.groundLayer);

        return rayCastHit.collider != null;
    }

    protected void AnimationStateSwitch()
    {

        if (rigidBody.velocity.y > 1f && CheckIfGrounded() != true)
        {
            this.state = PlayerState.jumping;
            //Debug.Log(PlayerState.jumping + " - skachame");
        }
        else if (state == PlayerState.jumping && playerJump.swimming == true)
        {
            state = PlayerState.swimming;
        }
        else if (moving == true && CheckIfGrounded())
        {
            playerJump.swimming = false;
            state = PlayerState.moving;
        }

        else
        {
            if (CheckIfGrounded())
            {
                playerJump.swimming = false;
                state = PlayerState.idle;
            }
        }
    }
}
