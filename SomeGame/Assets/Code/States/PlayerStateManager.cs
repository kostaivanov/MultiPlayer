using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    internal PlayerMovement playerMovement;
    internal PlayerBaseState currentState;
    internal PlayerIdleState idleState;
    internal PlayerRunningState runningState;
    internal PlayerSwimmingState swimmingState;
    internal PlayerJumpingState jumpingState;
    internal PlayerStoppingState stoppingState;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentState = idleState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
