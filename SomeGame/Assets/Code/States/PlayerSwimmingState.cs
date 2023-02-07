using UnityEngine;

internal class PlayerSwimmingState : PlayerBaseState
{
    private float initialGravity;
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Swimming");
        initialGravity = player.rigidBody.gravityScale;
        player.rigidBody.gravityScale = 0;
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.grounded == true && player.playerMovement.moving == true)
        {
            player.playerJump.swimming = false;
            player.SwitchState(player.runningState);
            player.rigidBody.gravityScale = initialGravity;
        }

        else if(player.playerMovement.grounded == true)
        {
            player.playerJump.swimming = false;
            player.SwitchState(player.idleState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }


}
