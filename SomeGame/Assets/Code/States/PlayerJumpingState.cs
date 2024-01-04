using UnityEngine;

internal class PlayerJumpingState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Jumping");
    }
    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.swimming == true)
        {
            player.SwitchState(player.swimmingState);
        }
        if (player.playerMovement.velocity.y == 0 || player.playerMovement.controller2D.collisions.below == true)
        {
            player.SwitchState(player.idleState);
        }
    }
    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }


}
