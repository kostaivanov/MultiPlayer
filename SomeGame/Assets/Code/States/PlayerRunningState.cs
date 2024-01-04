using UnityEngine;

internal class PlayerRunningState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Running");
        //Debug.Log($"Playing animation Running!");
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        //if (player.rigidBody.velocity.x == 0 && player.playerMovement.grounded == true)
        //{
        //    player.SwitchState(player.stoppingState);
        //}

        if (player.playerMovement.CanMove == false && player.playerMovement.controller2D.collisions.below == true)
        {
            player.SwitchState(player.stoppingState);
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
