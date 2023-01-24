using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
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

    private float gravity = -20;
    private Vector3 velocity;
    private bool moving;
    private float direction;

    private bool canMove;
    PlayerInputActions playerInputActions;
    private PlayerJump playerJump;
    RaycastOrigins raycastOrigings;

    internal bool grounded = false;
    private Vector2 inputVector;
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
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerJump = GetComponent<PlayerJump>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moving = false;
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        grounded = Physics2D.OverlapArea(new Vector2(collider2D.bounds.center.x - collider2D.bounds.extents.x, collider2D.bounds.center.y - collider2D.bounds.extents.y),
            new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x, collider2D.bounds.center.y - (collider2D.bounds.extents.y + 0.1f)), groundLayer);


        if (playerInputActions.Player.Movement.IsPressed())
        {
            inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            //MoveBody(inputVector);
            moving = true;
        }
        else if (playerJump.swimming == true && playerInputActions.Player.Swimming.IsPressed())
        {
            moving = false;
            inputVector = playerInputActions.Player.Swimming.ReadValue<Vector2>();
            //MoveBody(inputVector);
        }
        else
        {
            moving = false;
        }

        if (moving == true)
        {
            MoveBody(inputVector);
        }
    }

    private void FixedUpdate()
    {
        //if (moving == true)
        //{
        //    MoveBody(inputVector);
        //}

        //if (moving == true)
        //{
        //    rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
        //    this.transform.localScale = new Vector2(direction * 0.65f, 0.65f);
        //    //rigidBody.MovePosition(rigidBody.position + new Vector2(direction * speed, 0) * Time.deltaTime);
        //}  
    }
    //internal void UpdateRaycastOrigins()
    //{
    //    Bounds bounds = base.collider2D.bounds;
    //    bounds.Expand(skinWidth * -5);
    //    raycastOrigings.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    //    raycastOrigings.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    //}

    private void OnDrawGizmos()
    {
        if (collider2D != null)
        {
            Gizmos.color = new Color(1, 0, 1, 0.5f);
            Gizmos.DrawCube(new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y - (collider2D.bounds.extents.y + 0.005f)), new Vector2(collider2D.bounds.size.x, 0.05f));
        }
    }


    private void MoveBody(Vector2 inputVector)
    {
       // rigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * speed, ForceMode2D.Force);
        this.transform.localScale = new Vector2(inputVector.x, 1);
        transform.Translate(inputVector * (Time.deltaTime * speed));
    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
        Debug.Log("State = " + state);
    }

    protected void AnimationStateSwitch()
    {

        if (rigidBody.velocity.y > 1f && grounded != true)
        {
            this.state = PlayerState.jumping;
            //Debug.Log(PlayerState.jumping + " - skachame");
        }
        else if (state == PlayerState.jumping && playerJump.swimming == true)
        {
            state = PlayerState.swimming;
        }
        else if (moving == true && grounded)
        {
            playerJump.swimming = false;
            state = PlayerState.moving;
        }

        else
        {
            if (grounded)
            {
                playerJump.swimming = false;
                state = PlayerState.idle;
            }
        }
    }
}
