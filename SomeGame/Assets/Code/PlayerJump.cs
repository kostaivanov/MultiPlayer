using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
internal class PlayerJump : PlayerComponents
{
    PlayerMovement playerMovement;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpMultiplier;

    private bool isJumping;
    private float jumpCounter;
    private Vector2 vecGravity;

    private bool canDoubleJump = false;
    internal bool swimming;
    private bool jumpCanceled;

    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        jumpCanceled = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        playerMovement = GetComponent<PlayerMovement>();

        //jumpPressed = false;
        swimming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.y > 0 && isJumping == true)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false;
            }
            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }
            rigidBody.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
            Debug.Log("???????");
        }

        if (jumpCanceled == true)
        {
            isJumping = false;
            jumpCounter = 0;
            if (rigidBody.velocity.y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.6f);
            }
        }
    }

    private void FixedUpdate()
    {

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed == true && playerMovement.grounded)
        {
            jumpCanceled = false;
            canDoubleJump = true;

            //rigidBody.gravityScale = gravityScale;
            //float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody.gravityScale));
            //rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
            isJumping = true;
            jumpCounter = 0;
            Debug.Log("Jump!");
            //StartCoroutine(JumpProcess());
        }
        else if (context.performed == true)
        {
            if (canDoubleJump == true)
            {
                Debug.Log("Double jump!");
                canDoubleJump = false;
                swimming = true;
            }
        }

        if (context.canceled == true)
        { 
            jumpCanceled = true;
            Debug.Log("Canceled jump!");
        }
        //if (rigidBody.velocity.y > 0)
        //{
        //    rigidBody.gravityScale = gravityScale;
        //}
        //else
        //{
        //    rigidBody.gravityScale = fallGravityScale;
        //}
    }

    //public static float CalculateJumpForce(float gravityStrength, float jumpHeight)
    //{
    //    //h = v^2/2g
    //    //2gh = v^2
    //    //sqrt(2gh) = v
    //    return Mathf.Sqrt(2 * gravityStrength * jumpHeight);
    //}

    //private IEnumerator JumpProcess()
    //{
    //    jumpForce_2 = CalculateJumpForce(Physics2D.gravity.magnitude, jumpForce);
    //    //the initial jump
    //    Debug.Log("Jump force = " + jumpForce_2);
    //    rigidBody.AddForce(Vector2.up * (jumpForce_2 * 2) * rigidBody.mass);
    //    yield return null;

    //    //can be any value, maybe this is a start ascending force, up to you
    //    float currentForce = jumpForce_2;

    //    while (Input.GetKey(KeyCode.Space) && currentForce > 0)
    //    {
    //        rigidBody.AddForce(Vector2.up * currentForce * rigidBody.mass);
    //        currentForce -= decayRate * Time.fixedDeltaTime;
    //        yield return null;
    //    }
    //}
}
