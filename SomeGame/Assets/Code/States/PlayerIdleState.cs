using UnityEngine;

internal class PlayerIdleState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {

    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.moving == true && player.playerMovement.grounded)
        {
            player.SwitchState(player.runningState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }

}
