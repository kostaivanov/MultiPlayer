using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerStateManager : PlayerComponents
{
    internal PlayerMovement playerMovement;
    internal PlayerBaseState currentState;
    internal PlayerIdleState idleState = new PlayerIdleState();
    internal PlayerRunningState runningState = new PlayerRunningState();
    internal PlayerSwimmingState swimmingState = new PlayerSwimmingState();
    internal PlayerJumpingState jumpingState = new PlayerJumpingState();
    internal PlayerStoppingState stoppingState = new PlayerStoppingState();

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        currentState = idleState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        Debug.Log(currentState);
    }

    internal void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
