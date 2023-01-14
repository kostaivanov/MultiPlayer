using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (PlayerMovement))]
internal class PlayerJump : PlayerComponents
{
    PlayerMovement playerMovement;

    [SerializeField] private float jumpForce;
    [SerializeField] private float decayRate;

    private float jumpForce_2;

    private bool jumpPressed;
    private bool canDoubleJump = false;
    internal bool swimming;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private void Awake()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }
    
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        jumpPressed = false;
        swimming = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (jumpPressed == true)
        {
            StartCoroutine(JumpProcess());

            jumpPressed = false;
        }
        if (!playerMovement.CheckIfGrounded())
        {
            jumpPressed = false;

        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed == true && playerMovement.CheckIfGrounded())
        {
            Debug.Log("Jump! " + context.phase);
            canDoubleJump = true;
            StartCoroutine(JumpProcess());
        }
        else if(context.performed == true)
        {
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
                swimming = true;
            }
        }
    }

    public static float CalculateJumpForce(float gravityStrength, float jumpHeight)
    {
        //h = v^2/2g
        //2gh = v^2
        //sqrt(2gh) = v
        return Mathf.Sqrt(2 * gravityStrength * jumpHeight);
    }
  
    private IEnumerator JumpProcess()
    {
        jumpForce_2 = CalculateJumpForce(Physics2D.gravity.magnitude, jumpForce);
        //the initial jump
        Debug.Log("Jump force = " + jumpForce_2);
        rigidBody.AddForce(Vector2.up * (jumpForce_2 * 2) * rigidBody.mass);
        yield return null;

        //can be any value, maybe this is a start ascending force, up to you
        float currentForce = jumpForce_2;

        while (Input.GetKey(KeyCode.Space) && currentForce > 0)
        {
            rigidBody.AddForce(Vector2.up * currentForce * rigidBody.mass);
            currentForce -= decayRate * Time.fixedDeltaTime;
            yield return null;
        }
    }
}
