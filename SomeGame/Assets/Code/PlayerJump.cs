using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
internal class PlayerJump : PlayerComponents
{
    PlayerMovement playerMovement;

    //[SerializeField] private float jumpForce;
    //[SerializeField] private float decayRate;
    //private float jumpForce_2;

    //[SerializeField] private float fallMultiplier = 2.5f;
    //[SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityScale;
    [SerializeField] private float fallGravityScale;
    private bool jumping;
    private float buttonPressedTime;
    [SerializeField] private float buttonPressTime;
    private float jumpTime;
    //private float jumpVelocity;

    //private bool jumpPressed;

    private bool canDoubleJump = false;
    internal bool swimming;

    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();

        //jumpPressed = false;
        swimming = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rigidBody.gravityScale);
        if (jumping == true)
        {
            jumpTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //if (jumpPressed == true)
        //{

        //    jumpPressed = false;
        //}       

        //if (!playerMovement.grounded)
        //{
        //    jumpPressed = false;
        //}
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed == true && playerMovement.grounded)
        {
            jumping = true;
            jumpTime = 0;

            canDoubleJump = true;
            rigidBody.gravityScale = gravityScale;
            float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rigidBody.gravityScale) * -2) * rigidBody.mass;
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumping = true;
            buttonPressedTime = 0;

            //StartCoroutine(JumpProcess());
        }
        else if (context.performed == true)
        {
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
                swimming = true;
            }
        }

        if (jumping == true)
        {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime < buttonPressTime && context.canceled)
            {
                // cancel the jump
                rigidBody.gravityScale = fallGravityScale;
            }

            if (rigidBody.velocity.y < 0)
            {
                rigidBody.gravityScale = fallGravityScale;
                jumping = false;
            }
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
