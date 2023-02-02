using UnityEngine;

internal class PlayerJumpingState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Jumping");
    }
    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerJump.swimming == true)
        {
            player.SwitchState(player.swimmingState);
        }
        if (player.rigidBody.velocity.y == 0 || player.playerMovement.grounded == true)
        {
            player.SwitchState(player.idleState);
        }
    }
    internal override void OnCollisionEnter(PlayerStateManager player)
    {
        
    }


}
