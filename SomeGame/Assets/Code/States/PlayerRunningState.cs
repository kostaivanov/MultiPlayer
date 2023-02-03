using UnityEngine;

internal class PlayerRunningState : PlayerBaseState
{
    internal override void EnterState(PlayerStateManager player)
    {
        player.animator.Play("Running");
        Debug.Log($"Playing animation Running!");
    }

    internal override void UpdateState(PlayerStateManager player)
    {
        //if (player.rigidBody.velocity.x == 0 && player.playerMovement.grounded == true)
        //{
        //    player.SwitchState(player.stoppingState);
        //}

        if (player.playerMovement.moving == false && player.playerMovement.grounded == true)
        {
            player.SwitchState(player.stoppingState);
        }

        if (player.rigidBody.velocity.y > 1f && player.playerMovement.grounded != true)
        {
            player.SwitchState(player.jumpingState);
        }
    }

    internal override void OnCollisionEnter(PlayerStateManager player)
    {

    }
}
