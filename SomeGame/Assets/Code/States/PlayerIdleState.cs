using UnityEngine;

internal class PlayerIdleState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("IDLE");
        //Debug.Log("Playing animation IDLE!");
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        if (player.playerMovement.CanMove == true && player.playerMovement.controller2D.collisions.below)
        {
            player.SwitchState(player.runningState);
        }
        if (player.playerMovement.velocity.y > 1f && player.playerMovement.controller2D.collisions.below != true)
        {
            player.SwitchState(player.jumpingState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }

}
