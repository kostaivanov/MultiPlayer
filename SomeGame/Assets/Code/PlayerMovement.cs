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

    [SerializeField] private float speed;
 
    private float extrHeightText = 0.1f;

    private bool moving;
    private float direction;

    private bool canMove;
    PlayerInputActions playerInputActions;
    private PlayerJump playerJump;
    //private Control control;
    //private InputAction movement;

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
    }

    // Update is called once per frame
    void Update()
    {
        //if (direction != 0)
        //{
        //    moving = true;
        //    //direction = control.Player.Movement.ReadValue<float>();
        //    //Debug.Log("control " + control.Player.Movement.ReadValue<float>());

        //}
        ////if (Input.GetAxisRaw("Horizontal") != 0 && CanMove == true)
        ////{
        ////    direction = FindDirection();
        ////    moving = true;
        ////}
        //else
        //{
        //    moving = false;
        //
        Debug.Log("grounded = " + CheckIfGrounded());
        Debug.DrawRay(base.collider2D.bounds.center, Vector2.down, Color.green, base.collider2D.bounds.size.y);
    }

    private void FixedUpdate()
    {     
        if (playerInputActions.Player.Movement.IsPressed())
        {
            Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            rigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * speed, ForceMode2D.Force);
            moving = true;
        }
        else if(playerJump.swimming == true && playerInputActions.Player.Swimming.IsPressed())
        {
            moving = false;
            Vector2 inputVector = playerInputActions.Player.Swimming.ReadValue<Vector2>();
            rigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * speed, ForceMode2D.Force);
            Debug.Log("playing");
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
  

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
    }

    private float FindDirection()
    {
        direction = Input.GetAxisRaw("Horizontal");
        return direction;
    }

    internal bool CheckIfGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(base.collider2D.bounds.center, base.collider2D.bounds.size, 0f, Vector2.down, extrHeightText, base.groundLayer);

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
